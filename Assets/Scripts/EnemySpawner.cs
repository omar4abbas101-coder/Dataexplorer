using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner attributes")]
    public int maxEnemyAmount = 0;
    float enemySpawnIntervals = 100;
    public int enemiesLeft = 10;
    List<Enemy> enemies = new List<Enemy>();

    [Header("Prefabs")]
    [SerializeField] GameObject enemyPrefab;

    public float t = 0;

    private void Start()
    {
        // adding reference to this spawner to game manager
        GameManager.Instance.enemySpawner = this;
    }
    public void SetSpawnerParams(WaveScrObj currentWave)
    {
        enemySpawnIntervals = currentWave.enemyIntervals;
        enemiesLeft = currentWave.enemyAmount;
        maxEnemyAmount = currentWave.maxEnemyAmount;
    }

    private void Update()
    {
        SpawnCheck();
    }

    void SpawnCheck()
    {
        // checking if more enemies are left in this wave and if max is reached
        if (enemies.Count >= maxEnemyAmount || enemiesLeft == 0 || GameManager.Instance.pause) return;

        t += Time.deltaTime; // progressing timer

        if (t >= enemySpawnIntervals)
        {
            t = 0; // resetting timer

            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        // SETTING VARIABLES
        // calculating starting X
        float borderOffset = enemyPrefab.GetComponent<Enemy>().movementMargins;
        float minX = GameManager.Instance.GetScreenLeft() + borderOffset;
        float maxX = GameManager.Instance.GetScreenRight() - borderOffset;
        float randomX = Random.Range(minX, maxX);
        // calculating starting Y
        float startingY = GameManager.Instance.GetScreenTop() + 1f;
        // calculating spawning position
        Vector3 spawnPos = new Vector3(randomX, startingY, 0);

        // SPAWNING THE ENEMY
        Enemy newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity).GetComponent<Enemy>();
        enemies.Add(newEnemy);
    }

    public void EnemyDead(Enemy enemy)
    {
        // removing enemy from list
        enemies.Remove(enemy);

        // decreasing number of enemies left for current wave
        enemiesLeft--;

        // checking if this was the last enemy
        if (enemiesLeft == 0) GameManager.Instance.waveManager.enemiesDone = true;
    }
}
