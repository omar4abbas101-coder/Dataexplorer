using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;
    public string mainMenuSceneName = "Mainmenu";

    void Start()
    {
        int score = PlayerPrefs.GetInt("FinalScore", 0);

        if (finalScoreText != null)
            finalScoreText.text = $"FINAL SCORE: {score}";
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}