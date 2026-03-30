using UnityEngine;

public class SpawnHazard : MonoBehaviour
{
    public GameObject hazardPrefab;      // <-- GameObject fixes "Type mismatch"
    public Transform[] spawnPoints;

    float timer;

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
            return;

        float interval = 1.5f;
        if (DifficultySettings.Instance != null)
            interval = DifficultySettings.Instance.GetTuning().hazardSpawnInterval;

        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer = 0f;
            Spawn();
        }
    }

    void Spawn()
    {
        if (hazardPrefab == null) return;
        if (spawnPoints == null || spawnPoints.Length == 0) return;

        Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(hazardPrefab, sp.position, sp.rotation);
    }
}
