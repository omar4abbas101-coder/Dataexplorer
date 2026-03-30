using UnityEngine;

public class MagnetEvery250 : MonoBehaviour
{
    [SerializeField] GameObject magnetPickupPrefab;
    [SerializeField] Vector2 spawnMin = new Vector2(-8f, -4f);
    [SerializeField] Vector2 spawnMax = new Vector2(8f, 4f);
    [SerializeField] int stepScore = 250;

    GameManager gm;
    int nextScore;

    void Start()
    {
        gm = GameManager.Instance != null ? GameManager.Instance : FindObjectOfType<GameManager>();
        int s = gm != null ? gm.Score : 0;
        nextScore = ((s / stepScore) + 1) * stepScore;
    }

    void Update()
    {
        if (magnetPickupPrefab == null) return;
        if (gm == null) gm = GameManager.Instance != null ? GameManager.Instance : FindObjectOfType<GameManager>();
        if (gm == null) return;

        while (gm.Score >= nextScore)
        {
            Vector2 pos = new Vector2(
                Random.Range(spawnMin.x, spawnMax.x),
                Random.Range(spawnMin.y, spawnMax.y)
            );

            Instantiate(magnetPickupPrefab, pos, Quaternion.identity);
            nextScore += stepScore;
        }
    }
}
