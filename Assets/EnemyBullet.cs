using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 8f;
    public float lifeTime = 4f;

   void Start()
{
    Debug.Log("Enemy bullet spawned from prefab: " + gameObject.name);

    SpriteRenderer sr = GetComponent<SpriteRenderer>();
    if (sr != null && sr.sprite != null)
        Debug.Log("Enemy bullet sprite: " + sr.sprite.name);

    Destroy(gameObject, lifeTime);
}
    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D other)
{
    if (!other.CompareTag("Player"))
        return;

    if (GameManager.Instance != null)
    {
        GameManager.Instance.TakeDamage(1);
    }

    Destroy(gameObject);
}






}