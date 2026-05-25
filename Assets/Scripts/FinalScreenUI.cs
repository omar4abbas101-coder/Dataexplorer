using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinalScreenUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    public void GoToMenu()
    {
        SceneLoader.instance.LoadScene("Starting screen");
    }

    private void Start()
    {
        // shows the score player got while playing
        SetScore();
    }

    void SetScore()
    {
        int score = PlayerPrefs.GetInt("FinalScore", 0);
        scoreText.text = "Final score: " + score.ToString();
    }
}
