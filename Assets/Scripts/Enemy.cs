using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("refs")]
    [SerializeField] GameObject dodgePrefab;
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

        // setting fixedY to spaceship's position if it already is placed in the level when start. Otherwise fixedY is set in the spawner
        if (transform.position.y < GameManager.Instance.GetScreenTop()) fixedY = transform.position.y;
        else StartCoroutine(AppearFromTop());

        // spawning the dodge aura if the ship is dodging type
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

        // calculating future fixedY
        float randomYoffset = Random.Range(-shipToShipOffset, shipToShipOffset);
        fixedY = GameManager.Instance.GetScreenTop() - screenToShipOffset + randomYoffset;

        // setting variables for the move
        float t = 0;
        float blinkingT = 0;
        Vector3 startPos = transform.position;
        Vector3 targetPos = new Vector3(transform.position.x, fixedY, transform.position.z);

        while (t < appearingTime)
        {
            // calculating t (entering animation progress for current frame)
            t += Time.deltaTime; // progressing t
            float clampedT = t / appearingTime; // clamping so t is from '0' to '1'
            float coolT = 1f - (1f - clampedT) * (1f - clampedT); // applying math to make the movement 'fast > slow'

            transform.position = Vector3.Lerp(startPos, targetPos, coolT); // moving the spaceship

            // spaceship blinking
            blinkingT += Time.deltaTime;
            if (blinkingT > blinkingInterval) { InvisibilityBlink(); }

            yield return null;
        }

        // appearence complete, spaceship can start moving and shooting
        appearing = false;
        InvisibilityBlink();
    }

    /// <summary>
    /// Making the spaceship blink, signifying temporary invisibility
    /// </summary>
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
        // do not move left and right when entering the screen
        if (appearing) return;

        float newX = transform.position.x + moveDirection * moveSpeed * Time.deltaTime;
        newX = Mathf.Clamp(newX, leftX, rightX);

        transform.position = new Vector3(newX, fixedY, transform.position.z);

        if (newX >= rightX || newX <= leftX) { ChangeDirection(); dodgeAura.StopDodge(); }            
    }

    public void ChangeDirection()
    {
        moveDirection *= -1;
    }

    public void TakeDamage(int damage)
    {
        // invincible when appearing
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

        // adding score
        GameManager.Instance.AddScore(scoreValue);

        // removing enemy object from game
        GameManager.Instance.enemySpawner.EnemyDead(this);
        GameManager.Instance.powerUpManager.SpawnPowerUpCheck(EnemyType.ENEMY, transform.position);
        Destroy(gameObject);
    }
}