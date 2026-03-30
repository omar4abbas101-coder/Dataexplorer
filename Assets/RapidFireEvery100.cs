using UnityEngine;

public class RapidFireEvery100 : MonoBehaviour
{
    [Header("Pickup Prefab")]
    [SerializeField] GameObject rapidFirePickupPrefab;

    [Header("Spawn Area (world coords)")]
    [SerializeField] Vector2 spawnMin = new Vector2(-8f, -4f);
    [SerializeField] Vector2 spawnMax = new Vector2(8f, 4f);

    [Header("Trigger")]
    [SerializeField] int stepScore = 100;

    GameManager gm;
    int nextScore = 100;

    void Start()
    {
        gm = GameManager.Instance;
        if (gm == null) gm = FindObjectOfType<GameManager>();

        // Start at the next 100 boundary so it works mid-run too
        if (gm != null)
        {
            int s = gm.Score;
            nextScore = ((s / stepScore) + 1) * stepScore;
        }
        else
        {
            nextScore = stepScore;
        }
    }

    void Update()
    {
        if (rapidFirePickupPrefab == null) return;

        if (gm == null)
        {
            gm = GameManager.Instance;
            if (gm == null) gm = FindObjectOfType<GameManager>();
            if (gm == null) return;
        }

        int s = gm.Score;

        while (s >= nextScore)
        {
            SpawnPickup();
            nextScore += stepScore;
        }
    }

    void SpawnPickup()
    {
        Vector2 pos = new Vector2(
            Random.Range(spawnMin.x, spawnMax.x),
            Random.Range(spawnMin.y, spawnMax.y)
        );

        Instantiate(rapidFirePickupPrefab, pos, Quaternion.identity);
    }
}
