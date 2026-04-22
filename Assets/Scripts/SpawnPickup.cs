using UnityEngine;

public class SpawnPickup : MonoBehaviour
{
    public GameObject pickupPrefab;
    public Transform[] spawnPoints;
    public int maxPickups = 20;

    float timer;

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
            return;

        float interval = 3f;
        if (DifficultySettings.Instance != null)
            interval = DifficultySettings.Instance.GetTuning().pickupSpawnInterval;

        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer = 0f;
            Spawn();
        }
    }

    void Spawn()
    {
        if (pickupPrefab == null || spawnPoints == null 
            || spawnPoints.Length == 0 || GameManager.Instance.pause) return;

        GameObject[] pickups = GameObject.FindGameObjectsWithTag("Pickup");
        if (pickups.Length >= maxPickups) return;

        Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(pickupPrefab, sp.position, sp.rotation);
    }
}