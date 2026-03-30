using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private string mainSceneName = "Main";

    public void Easy()
    {
        Time.timeScale = 1f;
        DifficultySettings.Instance.SetDifficulty(Difficulty.Easy);
        SceneManager.LoadScene(mainSceneName);
        
    }

    public void Normal()
    {
        Time.timeScale = 1f;
        DifficultySettings.Instance.SetDifficulty(Difficulty.Normal);
        SceneManager.LoadScene(mainSceneName);
    }

    public void Hard()
    {
        Time.timeScale = 1f;
        DifficultySettings.Instance.SetDifficulty(Difficulty.Hard);
        SceneManager.LoadScene(mainSceneName);
    }
}
