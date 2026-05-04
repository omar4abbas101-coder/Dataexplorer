using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    [HideInInspector] public float moveSpeed = 3f;
    public float movementMargins = 0;
    float leftX;
    float rightX;
    int moveDirection = 1;
    float fixedY = 0;

    [Header("Dramatic appearance")]
    [HideInInspector] public bool appearing = false;
    [SerializeField] float screenToShipOffset = 0;
    [SerializeField] float shipToShipOffset = 0;
    [SerializeField] float appearingTime = 0;


    [Header("attributes")]
    public int hp = 2;
    public int scoreValue = 50;

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
    }

    IEnumerator AppearFromTop()
    {
        // calculating future fixedY
        float randomYoffset = Random.Range(-shipToShipOffset, shipToShipOffset);
        fixedY = GameManager.Instance.GetScreenTop() - screenToShipOffset + randomYoffset;

        // setting variables for the move
        float t = 0;
        Vector3 startPos = transform.position;
        Vector3 targetPos = new Vector3(transform.position.x, fixedY, transform.position.z);

        while (t < appearingTime)
        {
            // calculating t (entering animation progress for current frame)
            t += Time.deltaTime; // progressing t
            float clampedT = t / appearingTime; // clamping so t is from '0' to '1'
            float coolT = 1f - (1f - clampedT) * (1f - clampedT); // applying math to make the movement 'fast > slow'

            transform.position = Vector3.Lerp(startPos, targetPos, coolT); // moving the spaceship
            yield return null;
        }

        // appearence complete, spaceship can start moving and shooting
        appearing = false;
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

        if (newX >= rightX)
            moveDirection = -1;
        else if (newX <= leftX)
            moveDirection = 1;
    }

    public void TakeDamage(int damage)
    {
        // invincible when appearing
        if (appearing) return;

        hp -= damage;

        if (hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // adding score
        GameManager.Instance.AddScore(scoreValue);

        // removing enemy object from game
        GameManager.Instance.enemySpawner.EnemyDead(this);
        Destroy(gameObject);
    }
}