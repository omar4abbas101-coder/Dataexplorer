using UnityEngine;
using System.Collections;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] float pauseBeforeBoss;
    [SerializeField] GameObject bossPrefab;
    private void Start()
    {
        // adding reference to this spawner to game manager
        GameManager.Instance.bossSpawner = this;
    }

    public void SetSpawnerParams(WaveScrObj currentWave)
    {
        if (currentWave.bossWave) StartCoroutine(SpawnBoss());
    }

    IEnumerator SpawnBoss()
    {
        yield return new WaitForSeconds(pauseBeforeBoss);

        Vector3 bossSpawnPos = new Vector3(0f, GameManager.Instance.GetScreenTop() + 1f, 0f);
        Boss newBoss = Instantiate(bossPrefab, bossSpawnPos, Quaternion.identity).GetComponent<Boss>();
    }
}
