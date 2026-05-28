using System.Collections;
using UnityEngine;
using UnityEngine.TerrainUtils;
using UnityEngine.UI;

enum BossPhase
{
    APPEARING,
    DEFAULT,
    DASHING
}

public class Boss : MonoBehaviour
{
    [Header("refs")]
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] GameObject firePoint;
    [SerializeField] GameObject projectilePrefab;

    [Header("Dramatic appearance")]
    [HideInInspector] public bool appearing = true;
    [SerializeField] Vector2 bossPosition;
    [SerializeField] float appearingTime = 0;
    [SerializeField] float blinkingInterval;

    [Header("circular movement")]
    float angle;
    [SerializeField] float width;
    [SerializeField] float height;
    float moveSpeed;

    [Header("shooting")]
    float shootingIntervals;
    int shotsInRow;
    int bulletCount;
    [SerializeField] float bulletAngle;
    bool shooting = false;
    float shootingT = 0;

    [Header("HP bar")]
    GameObject hpBarObj;
    Image hpBarFill;
    int hpTotal;
    int hpCurrent;
    bool invincibile;
    [SerializeField] float yellowBarCap;
    [SerializeField] float redBarCap;


    BossPhase currentPhase = BossPhase.APPEARING;

    private void Start()
    {
        InitBoss(GameManager.Instance.currentWave);
        StartCoroutine(AppearFromTop(true));
    }

    public void InitBoss(WaveScrObj wave)
    {
        hpTotal = wave.hpTotal;
        shotsInRow = wave.shotsInRow;
        bulletCount = wave.bulletCount;
        shootingIntervals = wave.bulletCount;
        moveSpeed = wave.moveSpeed;

        hpCurrent = hpTotal;
        StartCoroutine(AppearHP());
    }

    IEnumerator AppearHP()
    {
        // get refs to hp bar
        hpBarObj = GameManager.Instance.uiManager.bossHpObj;
        hpBarFill = GameManager.Instance.uiManager.bossHpFill;

        hpBarObj.SetActive(true);

        float t = 0;
        while (t < appearingTime)
        {
            t += Time.deltaTime;
            float clampedT = t / appearingTime;

            hpBarFill.fillAmount = clampedT;
            UpdateBarColor();
            yield return null;
        }
        hpBarFill.fillAmount = 1;
    }

    IEnumerator AppearFromTop(bool invincibility)
    {
        // switching phase to APPEARING
        currentPhase = BossPhase.APPEARING;

        // invincibility on appearing
        invincibile = invincibility;

        float t = 0;
        float blinkingT = 0;
        Vector3 startPos = transform.position;
        Vector3 targetPos = new Vector3(bossPosition.x, bossPosition.y - height, transform.position.z);

        while (t < appearingTime)
        {
            t += Time.deltaTime;
            float clampedT = t / appearingTime;
            float coolT = 1f - (1f - clampedT) * (1f - clampedT);

            transform.position = Vector3.Lerp(startPos, targetPos, coolT);

            if (invincibility)
            {
                blinkingT += Time.deltaTime;
                if (blinkingT > blinkingInterval)
                {
                    InvincibilityBlink();
                    blinkingT = 0f;
                }
            }

            yield return null;
        }

        // switching phase to DEFAULT
        currentPhase = BossPhase.DEFAULT;
        invincibile = false;
        InvincibilityBlink();
    }

    void InvincibilityBlink()
    {
        float alpha = (sprite.color.a != 1 || appearing == false) ? 1f : 0.3f;
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
    }

    private void Update()
    {
        MoveInCircle();
        ShootingCheck();
    }

    void MoveInCircle()
    {
        if (currentPhase != BossPhase.DEFAULT) return;

        // Increase the angle over time based on speed and framerate
        angle += moveSpeed * Time.deltaTime;

        // Calculate the new X and Y offset using Sine and Cosine
        float x = Mathf.Cos(angle) * width;
        float y = Mathf.Sin(angle) * height;

        // Apply the new position relative to the center point
        transform.position = bossPosition + new Vector2(x, y);
    }

    void ShootingCheck()
    {
        if (currentPhase != BossPhase.DEFAULT || shooting) return;

        // rotating the firePoint object towards player spaceship
        Vector2 directionToPlayer = GameManager.Instance.player.transform.position - firePoint.transform.position;
        firePoint.transform.up = directionToPlayer;

        // shooting timer
        shootingT += Time.deltaTime;

        // shooting a projectile when an interval is reached
        if (shootingT > shootingIntervals)
        {
            shootingT = 0;

            StartCoroutine(BurstShot());
        }
    }

    IEnumerator BurstShot()
    {
        shooting = true;

        // shooting a series of bullets
        for (int a = 0; a < shotsInRow; a++)
        {
            for (int i = 0; i < bulletCount; i++)
            {
                // calculating bullet angle based on how many bullets are shooting at once
                float angle = (bulletAngle / -2 * (bulletCount - 1)) + bulletAngle * i;

                GameObject bullet = Instantiate(projectilePrefab, firePoint.transform.position, firePoint.transform.rotation);
                bullet.transform.Rotate(0, 0, angle);

                // playing sound effect
                AudioManager.instance.PlaySFX("laser_shooting_sfx");
            }

            // pause between each spread of bullets
            yield return new WaitForSeconds(0.2f);
        }

        shooting = false;
    }

    // BOSS HEALTH 
    // ================
    // registering bullet hit
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Projectile") { TakeHit(); Destroy(collision.gameObject); }
    }

    void TakeHit()
    {
        if (invincibile) return;
        hpCurrent--;

        // updating size of bar
        hpBarFill.fillAmount = (float)hpCurrent / (float)hpTotal;
        UpdateBarColor();

        // checking if there's hp remaining
        DeathCheck();

        // blinking red when hit
        StartCoroutine(HitEffect());
    }

    void DeathCheck()
    {
        if (hpCurrent <= 0) BossDeath();
    }

    void BossDeath()
    {
        // communication to wave manager that wave is done
        GameManager.Instance.waveManager.bossDone = true;
        GameManager.Instance.waveManager.enemiesDone = true;
        GameManager.Instance.waveManager.lasersDone = true;
        GameManager.Instance.waveManager.asteroidsDone = true;

        hpBarObj.SetActive(false);
        this.gameObject.SetActive(false);
    }

    IEnumerator HitEffect()
    {
        Color ogColor = sprite.color;
        sprite.color = Color.red;

        yield return new WaitForSeconds(0.1f);
        sprite.color = ogColor;
    }

    void UpdateBarColor()
    {
        // updating color
        if (hpBarFill.fillAmount > yellowBarCap) hpBarFill.color = Color.green;
        else if (hpBarFill.fillAmount > redBarCap) hpBarFill.color = Color.yellow;
        else hpBarFill.color = Color.red;
    }
}
