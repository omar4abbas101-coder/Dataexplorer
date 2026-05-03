using Unity.VisualScripting;
using UnityEngine;

public class SpawnHazard : MonoBehaviour
{
    public GameObject hazardPrefab;
    public Transform[] spawnPoints;
    public int maxHazards = 50;
    float interval = 1.5f;
    float asteroidSpeed = 0;

    float timer;
    float asteroidTimer = 0;

    public void SetSpawnerParams(WaveScrObj currentWave)
    {
        asteroidTimer = currentWave.asteroidTime;
        interval = currentWave.asteroidIntervals;
        asteroidSpeed = currentWave.asteroidSpeed;
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        timer += Time.deltaTime;
        WaveAsteroidsTimer();

        if (timer >= interval)
        {
            timer = 0f;
            Spawn();
        }
    }

    void WaveAsteroidsTimer()
    {
        if (asteroidTimer > 0) asteroidTimer -= Time.deltaTime;
        else if (asteroidTimer <= 0 && GameManager.Instance.waveManager.asteroidsDone != true) GameManager.Instance.waveManager.asteroidsDone = true;
    }

    void Spawn()
    {
        if (hazardPrefab == null || spawnPoints == null || 
            spawnPoints.Length == 0 || GameManager.Instance.pause || asteroidTimer <= 0) return;

        GameObject[] hazards = GameObject.FindGameObjectsWithTag("Hazard");
        if (hazards.Length >= maxHazards) return;

        Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Hazard newHazard = Instantiate(hazardPrefab, sp.position, sp.rotation).GetComponent<Hazard>();

        // setting the speed
        newHazard.baseSpeed = asteroidSpeed;
    }
}