using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "Wave")]
public class WaveScrObj : ScriptableObject
{
    [Header("Asteroid parameters")]
    public float asteroidTime;
    public float asteroidIntervals;

    [Header("Enemy Spaceship parameters")]
    public int enemyAmount;
    public float enemyIntervals;
    public int maxEnemyAmount;

    //[Header("Lazer parameters")]
    // lazer parameters here
}
