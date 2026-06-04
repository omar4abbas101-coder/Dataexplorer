using System.Collections;
using UnityEngine;
using UnityEngine.UI;

enum BossPhase
{
    APPEARING,
    DEFAULT,
    DASHING,
    PREPARING_DASH,
    EXPLODING
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

    [Header("dash attack")]
    float dashInterval;
    int dashCount;
    float offScreenOffset = 1f;
    float dashingT = 0;
    float dashSpeed = 3f;
    [SerializeField] float dashTime = 2f;
    float pauseBeforeDash = 2.5f;
    [SerializeField] float pauseBetweenDashes = 2.5f;
    [SerializeField] bool dashBlinking;

    [Header("HP bar")]
    GameObject hpBarObj;
    Image hpBarFill;
    int hpTotal;
    int hpCurrent;
    bool invincibile;
    [SerializeField] float yellowBarCap;
    [SerializeField] float redBarCap;

    [Header("Death animation")]
    [SerializeField] float deathAnimTime;
    [SerializeField] float floatingDownSpeed;
    [SerializeField] float particleInterval;
    [SerializeField] GameObject deathParticle;

    BossPhase currentPhase = BossPhase.APPEARING;

    private void Start()
    {
        InitBoss(GameManager.Instance.currentWave);
        StartCoroutine(AppearFromTop(true));
    }

    public void InitBoss(WaveScrObj wave)
    {
        // hp
        hpTotal = wave.hpTotal;

        // shooting
        shotsInRow = wave.shotsInRow;
        bulletCount = wave.bulletCount;
        shootingIntervals = wave.bulletCount;
        moveSpeed = wave.moveSpeed;

        // dashing
        dashInterval = wave.dashInterval;
        dashCount = wave.dashCount;
        dashSpeed = wave.dashSpeed;
        pauseBeforeDash = wave.pauseBeforeDash;

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

        // setting correct boss rotation
        transform.up = new Vector3(0f, 0f, 0f);

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
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
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
        Dashing();
        DashCheck();
    }

    void DashCheck()
    {
        if (currentPhase != BossPhase.DEFAULT || shooting) return;

        dashingT += Time.deltaTime;

        if (dashingT > dashInterval)
        {
            dashingT = 0;

            StartCoroutine(StartDash());
        }
    }

    IEnumerator StartDash()
    {
        int dashesCompleted = 0;

        while (dashesCompleted < dashCount)
        {
            if (currentPhase == BossPhase.EXPLODING) yield break;

            // DASH PREPARATIION
            currentPhase = BossPhase.PREPARING_DASH;

            yield return new WaitForSeconds(pauseBeforeDash * 0.5f);
            // spawning the dash indicator in front of the boss ship
            GameManager.Instance.uiManager.dashIndicator.SetActive(true);
            GameManager.Instance.uiManager.dashIndicator.transform.position = Vector3.MoveTowards(transform.position, GameManager.Instance.player.transform.position, 3f);
            yield return new WaitForSeconds(pauseBeforeDash);

            // DASHING AT PLAYER
            currentPhase = BossPhase.DASHING;
            StartCoroutine(DashingEffect());

            GameManager.Instance.uiManager.dashIndicator.SetActive(false);

            // waiting until the boss dashed off screen
            yield return new WaitForSeconds(dashTime);

            // stops dashing
            currentPhase = BossPhase.PREPARING_DASH;

            // positioning boss to random off screen side
            int randomSide = Random.Range(0,4);
            Vector3 newDashStartPos = Vector3.zero;

            switch (randomSide)
            {
                case 0:
                    newDashStartPos = new Vector3(Level.instance.GetScreenLeft() - offScreenOffset, 0f, 0f); // left
                    break;
                case 1:
                    newDashStartPos = new Vector3(Level.instance.GetScreenRight() + offScreenOffset, 0f, 0f); // right
                    break;
                case 2:
                    newDashStartPos = new Vector3(0f, Level.instance.GetScreenBottom() - offScreenOffset, 0f); // bottom
                    break;
                case 3:
                    newDashStartPos = new Vector3(0f, Level.instance.GetScreenTop() + offScreenOffset, 0f); // top
                    break;
            }
            transform.position = newDashStartPos;

            // increasing completed dash count
            dashesCompleted++;

            // pause before next dash
            yield return new WaitForSeconds(pauseBetweenDashes);
        }

        // make the spaceship appear from top of the screen after all the dashing
        StartCoroutine(AppearFromTop(false));        
    }

    void Dashing()
    {
        if (currentPhase == BossPhase.PREPARING_DASH && GameManager.Instance.uiManager.dashIndicator.activeSelf != true)
        {
            // Looking at player
            Vector2 directionToPlayer = transform.position - GameManager.Instance.player.transform.position;
            transform.up = directionToPlayer;
        }
        else if (currentPhase == BossPhase.DASHING)
        {
            // Dashing forward
            transform.Translate(0f, -dashSpeed * Time.deltaTime, 0f);
        }
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
        else if (collision.tag == "Player" && currentPhase != BossPhase.EXPLODING) { GameManager.Instance.TakeDamage(1); }
    }

    void TakeHit()
    {
        if (invincibile || currentPhase == BossPhase.DASHING) return;
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
        if (hpCurrent <= 0)
        {  
            // stops dashing and shooting when dies
            StopAllCoroutines();
            GameManager.Instance.uiManager.dashIndicator.SetActive(false);

            StartCoroutine(DeathAnimation());
        }
    }

    IEnumerator DeathAnimation()
    {
        currentPhase = BossPhase.EXPLODING;

        invincibile = true;
        hpBarObj.SetActive(false);
        transform.up = new Vector3(0f, 0f, 0f);
        Color targetColor = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0f);
        Color startingColor = sprite.color;
        float t = 0;
        float particleT = 0;

        while (t < deathAnimTime)
        {
            t += Time.deltaTime;
            float clapmedT = t / deathAnimTime;

            // lerping the color
            Color spriteColor = Color.Lerp(startingColor, targetColor, clapmedT);
            sprite.color = spriteColor;

            // moving the ship down slowly
            transform.Translate(0f, floatingDownSpeed * Time.deltaTime, 0f, Space.World);

            // spawning explostion particles
            particleT += Time.deltaTime;
            if (particleT > particleInterval)
            {
                // resetting the timer
                particleT = 0;

                // spawning a particle
                SpawnDeathParticle();

                // playing shake animation
                StartCoroutine(ShakeAnimation(this.gameObject, 0.2f, 0.35f));

                // playing the sound effect 
                AudioManager.instance.PlaySFX("BossExplosionSFX");
            }

            yield return null;
        }

        // spawning big explosion particle
        GameObject finalParticle = Instantiate(deathParticle, transform.position, Quaternion.identity);
        finalParticle.transform.localScale *= 2.5f;
        // playing the sound effect 
        AudioManager.instance.PlaySFX("BossExplosionSFX");
        AudioManager.instance.PlaySFX("BossExplosionSFX");
        AudioManager.instance.PlaySFX("BossExplosionSFX");

        // after the animation is finished triggering end of the wave
        BossDeath();
    }

    IEnumerator ShakeAnimation(GameObject obj, float shakeLength, float shakeIntensity)
    {
        // shaking the healthbar for extra juice
        float t = 0;
        float maxIntensity = shakeIntensity;
        Vector3 startingPosition = obj.transform.localPosition;

        while (t < shakeLength)
        {
            // checking if object still exists
            if (obj == null) yield break;

            // Gradually decreasing the intensity
            t += Time.deltaTime;
            float actualT = t / shakeLength;
            float currentIntensity = Mathf.Lerp(maxIntensity, 0f, actualT);

            // Random position offset
            float xOffset = UnityEngine.Random.Range(-1, 1) * currentIntensity;
            float yOffset = UnityEngine.Random.Range(-1, 1) * currentIntensity;

            obj.transform.localPosition += new Vector3(xOffset, yOffset, 0);

            yield return null;

            // Reverting the offset
            obj.transform.localPosition = startingPosition;
        }
    }

    void SpawnDeathParticle()
    {
        // Getting particle position
        float particleX = Random.Range(transform.position.x - sprite.size.x / 2, transform.position.x + sprite.size.x / 2);
        float particleY = Random.Range(transform.position.y - sprite.size.y / 2, transform.position.y + sprite.size.y / 2);

        Vector3 particlePos = new Vector3(particleX, particleY, 0f);

        // Instantiating the particle
        GameObject newParticle = Instantiate(deathParticle, particlePos, Quaternion.identity);
    }

    /// <summary>
    /// Communicating to wave manager that the boss is dead and the wave can be over
    /// </summary>
    void BossDeath()
    {
        // communication to wave manager that wave is done
        GameManager.Instance.waveManager.bossDone = true;
        GameManager.Instance.waveManager.enemiesDone = true;
        GameManager.Instance.waveManager.lasersDone = true;
        GameManager.Instance.waveManager.asteroidsDone = true;

        this.gameObject.SetActive(false);
    }

    IEnumerator HitEffect()
    {
        Color ogColor = sprite.color;
        sprite.color = Color.red;

        yield return new WaitForSeconds(0.1f);
        sprite.color = ogColor;
    }

    IEnumerator DashingEffect()
    {
        while (currentPhase == BossPhase.DASHING && dashBlinking)
        {
            yield return new WaitForSeconds(0.1f);
            Color ogColor = sprite.color;
            sprite.color = Color.yellow;

            yield return new WaitForSeconds(0.1f);
            sprite.color = ogColor;
        }
    }

    void UpdateBarColor()
    {
        // updating color
        if (hpBarFill.fillAmount > yellowBarCap) hpBarFill.color = Color.green;
        else if (hpBarFill.fillAmount > redBarCap) hpBarFill.color = Color.yellow;
        else hpBarFill.color = Color.red;
    }
}
