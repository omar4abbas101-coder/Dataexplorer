using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DifficultyButton : MonoBehaviour
{
    [Header("refs")]
    [SerializeField] GameSettings gameSettings;
    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] Image buttonImage;

    [Header("difficulty obj")]
    [SerializeField] GameDifficultyScrObj difficulty;

    private void Start()
    {
        InitButton();
    }

    void InitButton()
    {
        // setting visuals of the button to reflect the difficulty
        buttonImage.color = difficulty.color;
        buttonText.text = difficulty.name;
    }

    /// <summary>
    /// Swiches to game scene
    /// </summary>
    public void StartGame()
    {
        // selects chosen difficulty
        gameSettings.difficulty = difficulty;

        // loads the scene
        SceneManager.LoadScene("Main");
    }
}
