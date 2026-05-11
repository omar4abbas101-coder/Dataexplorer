using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    ENEMY,
    HAZARD,
    LASER
}
public class PowerUpManager : MonoBehaviour
{
    [Header("refs")]
    [SerializeField] List<GameObject> powerUpList;

    [Header("spawn chances")]
    public float hazardChance;
    public float enemyChance;
    public void SpawnPowerUpCheck(EnemyType enemyType, Vector3 position)
    {
        float spawnChance = 0f;

        // deciding on spawn chance
        switch (enemyType)
        {
            case EnemyType.ENEMY: { spawnChance = enemyChance; break; }
            case EnemyType.HAZARD: { spawnChance = hazardChance; break; }
        }

        // randomly deciding if power up is spawned based on chance
        if (spawnChance > Random.value) SpawnPowerUp(position);   
    }

    void SpawnPowerUp(Vector3 position)
    { 
        GameObject randomPowerUp = powerUpList[Random.Range(0, powerUpList.Count)];
        GameObject newPowerUp = Instantiate(randomPowerUp, position, Quaternion.identity);

        Debug.Log("PowerUpManager: powerUp spawned: " + newPowerUp.name);
    }
}
