using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;

    [Header("refs")]
    [SerializeField] GameObject transitionFog;
    [SerializeField] GameObject loadingObj;
    [SerializeField] GameObject loadingIcon;

    [Header("transition params")]
    [SerializeField] float transitionTime;
    [SerializeField] bool fakeLoading;
    [SerializeField] float loadingSpinIntensity;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(ScreenFade(false));
    }

    private void Update()
    {
        Loading();
    }

    void Loading()
    {
        if (loadingObj.activeSelf == false) return;

        loadingIcon.transform.Rotate(0, 0, loadingSpinIntensity);
    }

    /// <summary>
    /// Scene transition visuals
    /// </summary>
    /// <param name="fadeIn"></param>
    IEnumerator ScreenFade(bool fadeIn)
    {
        // enabling the fog
        transitionFog.SetActive(true);

        // preparing the variables
        Vector3 startingPos = transitionFog.transform.localPosition;
        float cameraHeight = 1200f;
        float yOffset = (fadeIn) ? cameraHeight : -cameraHeight;
        Vector3 targetPos = startingPos + new Vector3(0f, yOffset, 0f);
        float transitionT = (fadeIn) ? transitionTime : transitionTime * 2;

        float t = 0;

        // moving the fog
        while (t < transitionT)
        {
            t += Time.deltaTime;

            float clampedT = t / transitionT;
            float coolT = (fadeIn) ? 1 - (1 - clampedT) * (1 - clampedT) : clampedT * clampedT;

            transitionFog.transform.localPosition = Vector3.Lerp(startingPos, targetPos, coolT);
            yield return null;
        }

        // disabling the fog if it was fade out
        transitionFog.SetActive(fadeIn);
    }

    /// <summary>
    /// Loads a new scene with fade in effect
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadScene(string sceneName, float loadingTime = 0f)
    {
        StartCoroutine(LoadNewScene(sceneName, loadingTime));
    }

    IEnumerator LoadNewScene(string sceneName, float loadingTime)
    {
        // transition fog effect
        yield return StartCoroutine(ScreenFade(true));

        // enabling "fake loading" if loading time is > 0
        if (loadingTime > 0f && fakeLoading)
        {
            loadingObj.SetActive(true);
            yield return new WaitForSeconds(loadingTime);
            loadingObj.SetActive(false);
        }

        // loading new scene
        SceneManager.LoadScene(sceneName);
    }
}
