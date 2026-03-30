using UnityEngine;

public class SpawnPickup : MonoBehaviour
{
    public GameObject pickupPrefab;
    public float spawnInterval = 2f;

    float timer;

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            Spawn();
        }
    }

    void Spawn()
    {
        if (Camera.main == null) return;

        float vert = Camera.main.orthographicSize;
        float horz = vert * Camera.main.aspect;

        Vector2 spawnPos = new Vector2(
            Random.Range(-horz + 0.5f, horz - 0.5f),
            Random.Range(-vert + 0.5f, vert - 0.5f)
        );

        Instantiate(pickupPrefab, spawnPos, Quaternion.identity);
    }
}
