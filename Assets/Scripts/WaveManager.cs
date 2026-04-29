using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Animations;
using Unity.PlasticSCM.Editor.WebApi;

public class WaveManager : MonoBehaviour
{
    [Header("refs")]
    public List<WaveScrObj> waves = new List<WaveScrObj>();
    int currentWave = 0;
    public TextMeshProUGUI waveText;

    [Header("transition params")]
    [SerializeField] float pauseBeforeNextWave;
    [SerializeField] float textFadeTime;
    [SerializeField] float textStayTime;
    [HideInInspector] public bool transitioning;

    [Header("transition checks")]
    public bool asteroidsDone = false;
    public bool enemiesDone = false;

    void NewWave()
    {
        // passing new wave object to game manager
        GameManager.Instance.currentWave = waves[currentWave];

        // resetting the spawners
        GameManager.Instance.hazardSpawner.SetSpawnerParams(waves[currentWave]);
        GameManager.Instance.enemySpawner.SetSpawnerParams(waves[currentWave]);
        GameManager.Instance.laserSpawner.SetSpawnerParams(waves[currentWave]);

        // resetitng the checks
        enemiesDone = waves[currentWave].enemyAmount == 0;
        asteroidsDone = waves[currentWave].asteroidTime == 0;

        // unpausing the spawning
        GameManager.Instance.pause = false;
    }

    private void Update()
    {
        NextWaveCheck();
    }

    void NextWaveCheck()
    {
        // checks if current wave is done.
        if (asteroidsDone && enemiesDone && GameManager.Instance.pause == false) FinishWave();
    }

    void FinishWave()
    {
        // pausing the things from spawning
        GameManager.Instance.pause = true;

        // increasing current wave id
        currentWave++;

        // if there are no more waves trigger end of the game
        if (currentWave == waves.Count)
        {
            GameManager.Instance.GameFinished();
            return;
        }

        // enable the animation for wave transition 
        StartCoroutine(NextWaveTransition());
    }
    public IEnumerator NextWaveTransition()
    {
        // PAUSE BEFORE WAVE STARTS
        yield return new WaitForSeconds(pauseBeforeNextWave);

        // SETTING THE VARIABLES
        waveText.gameObject.SetActive(true);
        waveText.text = waves[currentWave].name; // text displayed
        float t = 0;

        // TEXT FADING IN
        while (t < textFadeTime)
        {
            // calculating t
            t += Time.deltaTime;
            float clamptedT = t / textFadeTime;
            float alpha = Mathf.SmoothStep(0, 1, clamptedT);

            // setting color
            waveText.color = new Color(waveText.color.r, waveText.color.g, waveText.color.b, alpha);
            yield return null;
        }

        // TEXT STAYING ON THE SCREEN
        yield return new WaitForSeconds(textStayTime);
        t = 0;

        // TEXT FADING OUT
        while (t < textFadeTime)
        {
            // calculating t
            t += Time.deltaTime;
            float clamptedT = t / textFadeTime;
            float alpha = Mathf.SmoothStep(1, 0, clamptedT);

            // setting color
            waveText.color = new Color(waveText.color.r, waveText.color.g, waveText.color.b, alpha);
            yield return null;
        }

        // disabling the wave text
        waveText.color = new Color(waveText.color.r, waveText.color.g, waveText.color.b, 1f);
        waveText.gameObject.SetActive(false);

        // SWITCHING TO NEXT WAVE
        NewWave();
    }
}
