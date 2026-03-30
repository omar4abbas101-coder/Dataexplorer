using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 12f;
    public float lifeTime = 2f;

    Vector2 direction = Vector2.up;

    public void Init(Vector2 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Hazard hazard = other.GetComponentInParent<Hazard>();
        if (hazard == null) return;

        // 1️⃣ base score from hazard
        int score = hazard.GetScoreValue();

        // 2️⃣ register combo hit
        if (ComboManager.Instance != null)
        {
           ComboManager.Instance.RegisterHit();
            score = ComboManager.Instance.ApplyMultiplier(score);
        }

        // 3️⃣ add score
        if (GameManager.Instance != null)
            GameManager.Instance.AddScore(score);
if (CameraShake.Instance != null)
    CameraShake.Instance.Shake();
        Destroy(hazard.gameObject);
        Destroy(gameObject);
    }
}
