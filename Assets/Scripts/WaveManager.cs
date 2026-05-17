using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Animations;

public class WaveManager : MonoBehaviour
{
    [Header("refs")]
    public List<WaveScrObj> waves = new List<WaveScrObj>();
    int currentWave = 0;
    public TextMeshProUGUI waveText;
    public MatrixEffect matrixEffect;

    [Header("transition params")]
    [SerializeField] float pauseBeforeNextWave;
    [SerializeField] float textFadeTime;
    [SerializeField] float textStayTime;
    [HideInInspector] public bool transitioning;

    [Header("transition checks")]
    public bool asteroidsDone = false;
    public bool enemiesDone = false;
    public bool lasersDone = false;

    void NewWave()
    {
        GameManager.Instance.currentWave = waves[currentWave];

        GameManager.Instance.hazardSpawner.SetSpawnerParams(waves[currentWave]);
        GameManager.Instance.enemySpawner.SetSpawnerParams(waves[currentWave]);
        GameManager.Instance.laserSpawner.SetSpawnerParams(waves[currentWave]);

        asteroidsDone = waves[currentWave].asteroidTime == 0;
      enemiesDone = waves[currentWave].maxEnemyAmount == 0;
        lasersDone = waves[currentWave].laserAmount == 0;

        GameManager.Instance.pause = false;
    }

    private void Update()
    {
        NextWaveCheck();
    }

    void NextWaveCheck()
    {
        if (asteroidsDone && enemiesDone && lasersDone && GameManager.Instance.pause == false)
            FinishWave();
    }

    void FinishWave()
    {
        GameManager.Instance.pause = true;

        currentWave++;

        if (currentWave == waves.Count)
        {
            GameManager.Instance.GameFinished();
            return;
        }

        StartCoroutine(NextWaveTransition());
    }

    public IEnumerator NextWaveTransition()
    {
        if (currentWave > 0 && matrixEffect != null)
        {
            matrixEffect.speedUpMod = 5f;
        }

        yield return new WaitForSeconds(pauseBeforeNextWave);

        waveText.gameObject.SetActive(true);
        waveText.text = waves[currentWave].name;
        float t = 0;

        while (t < textFadeTime)
        {
            t += Time.deltaTime;
            float clamptedT = t / textFadeTime;
            float alpha = Mathf.SmoothStep(0, 1, clamptedT);

            waveText.color = new Color(
                waveText.color.r,
                waveText.color.g,
                waveText.color.b,
                alpha
            );

            yield return null;
        }

        yield return new WaitForSeconds(textStayTime);
        t = 0;

        while (t < textFadeTime)
        {
            t += Time.deltaTime;
            float clamptedT = t / textFadeTime;
            float alpha = Mathf.SmoothStep(1, 0, clamptedT);

            waveText.color = new Color(
                waveText.color.r,
                waveText.color.g,
                waveText.color.b,
                alpha
            );

            yield return null;
        }

        waveText.color = new Color(
            waveText.color.r,
            waveText.color.g,
            waveText.color.b,
            1f
        );

        waveText.gameObject.SetActive(false);

        if (currentWave > 0 && matrixEffect != null)
        {
            matrixEffect.speedUpMod = 1f;
        }

        NewWave();
    }
}