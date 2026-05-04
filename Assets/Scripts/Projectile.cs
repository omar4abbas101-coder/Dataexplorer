using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("properties")]
    [SerializeField] float speed = 4f;
    public float lifeTime = 2f;

    private void Start()
    {
        Init();
    }
    public void Init()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        Movement();
    }

    // Making projectile move forward
    void Movement()
    {
        // moving the projectile
        transform.Translate(0f, speed * Time.deltaTime, 0f, Space.Self);
    }

   void OnTriggerEnter2D(Collider2D other)
{
    // HIT ENEMY
    Enemy enemy = other.GetComponentInParent<Enemy>();
    if (enemy != null)
    {
        enemy.TakeDamage(1);

        if (CameraShake.Instance != null)
            CameraShake.Instance.Shake();

        Destroy(gameObject);
        return;
    }

    // HIT METEOR (existing system)
    Hazard hazard = other.GetComponentInParent<Hazard>();
    if (hazard != null)
    {
        int score = hazard.GetScoreValue();

        if (ComboManager.Instance != null)
        {
            ComboManager.Instance.RegisterHit();
            score = ComboManager.Instance.ApplyMultiplier(score);
        }

        if (GameManager.Instance != null)
            GameManager.Instance.AddScore(score);

        if (CameraShake.Instance != null)
            CameraShake.Instance.Shake();

        Destroy(hazard.gameObject);
        Destroy(gameObject);
    }
}
}