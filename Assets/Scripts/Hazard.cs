using UnityEngine;

public class Hazard : MonoBehaviour
{
    [Header("references")]
    [SerializeField] SpriteRenderer sprite;

    [Header("properties")]
    [SerializeField] float baseSpeed = 4f;
    [SerializeField] float rotationRange = 10f;
    [SerializeField] float lifeTime = 20f;
    [SerializeField] float rotationSpeed = 1f;

    private void Start()
    {
        Init();
    }
    private void Init()
    {
        // settings random rotation offset
        float randomRotationOffset = Random.Range(-rotationRange, rotationRange);
        transform.Rotate(0, 0, randomRotationOffset);

        // autodestroying the asteroids after lifetime expires
        Destroy(this.gameObject, lifeTime);
    }

    void Update()
    {
        Movement();
        Rotation();
    }

    // rotating the asteroid around its own axis
    void Rotation()
    {
        if (sprite != null) sprite.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    // moving the asteroid
    void Movement()
    {
        float speed = baseSpeed;
        if (DifficultySettings.Instance != null)
            speed *= DifficultySettings.Instance.GetTuning().hazardSpeedMultiplier;

        transform.Translate(0, speed * Time.deltaTime, 0, Space.Self); // << this is the only line you need to make asteroids move
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
