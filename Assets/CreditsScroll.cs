using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScroll : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 50f;
    [SerializeField] float creditsDuration = 20f;
    [SerializeField] string startingScene = "Starting screen";

    RectTransform rectTransform;
    float timer;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        timer = creditsDuration;
    }

    void Update()
    {
        rectTransform.anchoredPosition +=
            Vector2.up * scrollSpeed * Time.deltaTime;

        timer -= Time.deltaTime;

        Debug.Log("Time Remaining: " + timer.ToString("F1"));

        if (timer <= 0f)
        {
            SceneManager.LoadScene(startingScene);
        }
    }
}