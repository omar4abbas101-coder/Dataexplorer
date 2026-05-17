using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public enum SpaceShipType
{
    BASIC,
    DODGING
}

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner attributes")]
    int maxEnemyAmount = 0;
    float enemySpawnIntervals = 100;
    float enemySpeed;
    int enemiesLeft = 0;
    int nextEnemyID = 0;
    List<SpaceShipType> remainingEnemies = new List<SpaceShipType>();
    List<Enemy> enemies = new List<Enemy>();

    [Header("Prefabs")]
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject dodgingEnemyPrefab;

    float t = 0;

    private void Start()
    {
        // adding reference to this spawner to game manager
        GameManager.Instance.enemySpawner = this;
    }

    public void SetSpawnerParams(WaveScrObj currentWave)
    {
        enemySpawnIntervals = currentWave.enemyIntervals;
        remainingEnemies = currentWave.enemies;
        enemiesLeft = currentWave.enemies.Count;
        maxEnemyAmount = currentWave.maxEnemyAmount;
        enemySpeed = currentWave.enemySpeed;

        // reset values for new wave
        nextEnemyID = 0;
        enemies.Clear();
        t = 0;
    }

    private void Update()
    {
        SpawnCheck();
    }

    void SpawnCheck()
    {
        // stop if paused
        if (GameManager.Instance.pause)
            return;

        // stop if max enemies are already alive
        if (enemies.Count >= maxEnemyAmount)
            return;

        // stop if no more enemies left to spawn
        if (nextEnemyID >= remainingEnemies.Count)
            return;

        t += Time.deltaTime;

        if (t >= enemySpawnIntervals)
        {
            t = 0;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        // extra safety check
        if (nextEnemyID >= remainingEnemies.Count)
            return;

        // SETTING VARIABLES
        float borderOffset = enemyPrefab.GetComponent<Enemy>().movementMargins;
        float minX = GameManager.Instance.GetScreenLeft() + borderOffset;
        float maxX = GameManager.Instance.GetScreenRight() - borderOffset;
        float randomX = Random.Range(minX, maxX);

        float startingY = GameManager.Instance.GetScreenTop() + 1f;

        Vector3 spawnPos = new Vector3(randomX, startingY, 0);

        // CHOOSING ENEMY TYPE
        GameObject enemyToSpawn = enemyPrefab;

        switch (remainingEnemies[nextEnemyID])
        {
            case SpaceShipType.DODGING:
                enemyToSpawn = dodgingEnemyPrefab;
                break;
        }

        nextEnemyID++;

        // SPAWNING
        Enemy newEnemy = Instantiate(enemyToSpawn, spawnPos, Quaternion.identity).GetComponent<Enemy>();

        newEnemy.moveSpeed = enemySpeed;
        enemies.Add(newEnemy);
    }

    public void EnemyDead(Enemy enemy)
    {
        enemies.Remove(enemy);

        enemiesLeft--;

        Debug.Log("Enemy Spawner: enemies left: " + enemiesLeft);

        if (enemiesLeft == 0)
        {
            GameManager.Instance.waveManager.enemiesDone = true;
            Debug.Log("Enemy Spawner: no more enemies left");
        }
    }
}