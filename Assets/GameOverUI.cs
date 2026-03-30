using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;
    public string mainMenuSceneName = "Mainmenu"; // <- put your menu scene name here

    void Start()
    {
        int score = (GameManager.Instance != null) ? GameManager.Instance.Score : 0;
        if (finalScoreText != null)
            finalScoreText.text = $"FINAL SCORE: {score}";
    }

    public void Retry()
    {
        Time.timeScale = 1f;                 // IMPORTANT
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
