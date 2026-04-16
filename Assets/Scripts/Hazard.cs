using UnityEngine;

public class Hazard : MonoBehaviour
{
    public float baseSpeed = 4f;
    public bool useRightAxis = true; // true = move along transform.right, false = transform.up

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        float speed = baseSpeed;

        if (DifficultySettings.Instance != null)
            speed *= DifficultySettings.Instance.GetTuning().hazardSpeedMultiplier;

        Vector2 dir = useRightAxis ? (Vector2)transform.right : (Vector2)transform.up;
        dir = dir.normalized;

        // If you have Rigidbody2D, use velocity (best for collisions)
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.velocity = dir * speed;
        }
    }

    void Update()
    {
        // Fallback if no Rigidbody2D is present
        if (rb == null)
        {
            float speed = baseSpeed;
            if (DifficultySettings.Instance != null)
                speed *= DifficultySettings.Instance.GetTuning().hazardSpeedMultiplier;

            Vector2 dir = useRightAxis ? (Vector2)transform.right : (Vector2)transform.up;
            transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
        }
    }
public int GetScoreValue()
{
    if (DifficultySettings.Instance != null)
        return DifficultySettings.Instance.GetTuning().hazardScoreValue;

    return 10;
}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (GameManager.Instance != null)
            GameManager.Instance.PlayerHit(1);

        Destroy(gameObject);
    }
}
