using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSpawner : MonoBehaviour
{
    [Header("Spawner attributes")]
    float laserMinInterval = 100;
    float laserMaxInterval = 30;
    [SerializeField] float nextLaserInterval = 0;
    float minLaserSize = 1;
    float maxLaserSize = 6;
    [SerializeField] float[] possibleRotations = new float[0];
    float laserRotationChance = 0.2f;
    int lasersLeft = 10;
    float laserSpeed = 0;

    [Header("Prefabs")]
    [SerializeField] GameObject laserPrefab;

    [SerializeField] float t = 0;

    public void SetSpawnerParams(WaveScrObj currentWave)
    {
        laserMinInterval = currentWave.laserMinInterval;
        laserMaxInterval = currentWave.laserMaxInterval;
        minLaserSize = currentWave.laserMinSize;
        maxLaserSize = currentWave.laserMaxSize;
        lasersLeft = currentWave.laserAmount;
        laserRotationChance = currentWave.laserRotationChance;
        laserSpeed = currentWave.laserSpeed;

        NewLaserInterval();
    }

    private void Update()
    {
        LaserTimer();
    }

    void LaserTimer()
    {
        if (laserPrefab == null || GameManager.Instance.pause || lasersLeft <= 0 || nextLaserInterval == 0) return;

        // increasing timer
        t += Time.deltaTime;

        // checking if next interval is reached
        if (t > nextLaserInterval)
        {
            // resetting timer
            t = 0;

            // spawning a laser
            SpawnLaser();
        }
    }

    void SpawnLaser()
    {
        // LASER POSITION
        // calculating random starting x position
        float minX = GameManager.Instance.GetScreenLeft() +1;
        float maxX = GameManager.Instance.GetScreenRight() -1;

        float startingX = Random.Range(minX, maxX);
        float startingY = GameManager.Instance.GetScreenTop() + 2;
        Vector2 laserSpawnPos = new Vector2(startingX, startingY);

        // Spawning the laser object
        GameObject newLaser = Instantiate(laserPrefab, laserSpawnPos, Quaternion.identity);

        // LASER SIZE
        // calculating random laser size
        float laserLength = Random.Range(minLaserSize, maxLaserSize);
        float laserThickness = newLaser.GetComponent<SpriteRenderer>().size.y;
        Vector2 laserSize = new Vector2(laserLength, laserThickness);

        // Applying laser size
        newLaser.GetComponent<SpriteRenderer>().size = laserSize;
        newLaser.GetComponent<BoxCollider2D>().size = laserSize;

        // LASER ROTATION
        // check if laser is rotated
        if (laserRotationChance > Random.value)
        {
            // if the change triggers we rotate the laser to one of possible rotations
            int randomRotation = Random.Range(0, possibleRotations.Length);
            float chosenRotation = possibleRotations[randomRotation];

            // chance of making the angle negative
            chosenRotation = (0.5f > Random.value) ? chosenRotation : -chosenRotation;

            // applying the rotation
            newLaser.transform.Rotate(0, 0, chosenRotation);
        }

        // LASER SPEED
        // setting laser speed
        newLaser.GetComponent<Laser>().movementSpeed = laserSpeed;

        // decreasing the amount of lasers left
        lasersLeft--;

        // setting when next laser will be spawned
        NewLaserInterval();
    }

    void NewLaserInterval()
    {
        // calculating next random laser interval
        nextLaserInterval = Random.Range(laserMinInterval, laserMaxInterval);
    }
}
