using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    public GameObject pickupPrefab;

    public Vector2 spawnMin = new Vector2(-8, -4);
    public Vector2 spawnMax = new Vector2(8, 4);

    float timer;

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
            return;

        float interval = 2.5f;
        if (DifficultySettings.Instance != null)
            interval = DifficultySettings.Instance.GetTuning().pickupSpawnInterval;

        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer = 0f;
            SpawnPickup();
        }
    }

    void SpawnPickup()
    {
        if (pickupPrefab == null) return;

        Vector2 pos = new Vector2(
            Random.Range(spawnMin.x, spawnMax.x),
            Random.Range(spawnMin.y, spawnMax.y)
        );

        Instantiate(pickupPrefab, pos, Quaternion.identity);
    }
}
