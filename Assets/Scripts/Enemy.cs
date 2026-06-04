using UnityEngine;
using System.Collections;


public class Enemy : MonoBehaviour
{
    [Header("refs")]
    [SerializeField] GameObject dodgePrefab;
    [SerializeField] AudioClip deathSound;
    

    [SerializeField] GameObject destroyEffectPrefab;
    BulletDodge dodgeAura;
    [SerializeField] SpriteRenderer sprite;

    [Header("Movement")]
    [HideInInspector] public float moveSpeed = 3f;
    public float movementMargins = 0;
    float leftX;
    float rightX;
    [HideInInspector] public int moveDirection = 1;
    float fixedY = 0;

    [Header("Dramatic appearance")]
    [HideInInspector] public bool appearing = true;
    [SerializeField] float screenToShipOffset = 0;
    [SerializeField] float shipToShipOffset = 0;
    [SerializeField] float appearingTime = 0;
    [SerializeField] float blinkingInterval;

    [Header("attributes")]
    public int hp = 2;
    public int scoreValue = 50;
    public bool dodging;

    void Start()
    {
        Init();
    }

    void Init()
    {
        // setting left and right margins for movement
        leftX = GameManager.Instance.GetScreenLeft() + movementMargins;
        rightX = GameManager.Instance.GetScreenRight() - movementMargins;

        // setting fixedY
        if (transform.position.y < GameManager.Instance.GetScreenTop())
            fixedY = transform.position.y;
        else
            StartCoroutine(AppearFromTop());

        // spawning dodge aura
        if (dodging)
        {
            BulletDodge newDodge = Instantiate(dodgePrefab, transform.position, Quaternion.identity).GetComponent<BulletDodge>();
            newDodge.enemy = this;
            dodgeAura = newDodge;
        }
    }

    IEnumerator AppearFromTop()
    {
        appearing = true;

        float randomYoffset = Random.Range(-shipToShipOffset, shipToShipOffset);
        fixedY = GameManager.Instance.GetScreenTop() - screenToShipOffset + randomYoffset;

        float t = 0;
        float blinkingT = 0;
        Vector3 startPos = transform.position;
        Vector3 targetPos = new Vector3(transform.position.x, fixedY, transform.position.z);

        while (t < appearingTime)
        {
            t += Time.deltaTime;
            float clampedT = t / appearingTime;
            float coolT = 1f - (1f - clampedT) * (1f - clampedT);

            transform.position = Vector3.Lerp(startPos, targetPos, coolT);

            blinkingT += Time.deltaTime;
            if (blinkingT > blinkingInterval)
            {
                InvisibilityBlink();
                blinkingT = 0f;
            }

            yield return null;
        }

        appearing = false;
        InvisibilityBlink();
    }

    void InvisibilityBlink()
    {
        float alpha = (sprite.color.a != 1 || appearing == false) ? 1f : 0.3f;
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
    }

    void Update()
    {
        MoveLeftRight();
    }

    void MoveLeftRight()
    {
        if (appearing) return;

        float newX = transform.position.x + moveDirection * moveSpeed * Time.deltaTime;
        newX = Mathf.Clamp(newX, leftX, rightX);

        transform.position = new Vector3(newX, fixedY, transform.position.z);

        if (newX >= rightX || newX <= leftX)
        {
            ChangeDirection();

            if (dodging)
                dodgeAura.StopDodge();
        }
    }

    public void ChangeDirection()
    {
        moveDirection *= -1;
    }

    public void TakeDamage(int damage)
    {
        if (appearing) return;

        hp -= damage;

        if (CameraShake.Instance != null)
            CameraShake.Instance.Shake();

        if (hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (appearing) return;

        // play death sound
        if (deathSound != null)
        {
            AudioManager.instance.PlaySFX("enemyshipdeathsound",0.9f);
        }

        // score
        GameManager.Instance.AddScore(scoreValue);

        // enemy cleanup
        GameManager.Instance.enemySpawner.EnemyDead(this);
        GameManager.Instance.powerUpManager.SpawnPowerUpCheck(EnemyType.ENEMY, transform.position);
        Instantiate(destroyEffectPrefab,transform.position,Quaternion.identity);
        Destroy(gameObject);
    }
}