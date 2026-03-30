using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Text References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI comboText;

    void Awake()
    {
        // Optional auto-find by name if you forgot to assign in Inspector
        if (scoreText == null)
        {
            var go = GameObject.Find("ScoreText");
            if (go) scoreText = go.GetComponent<TextMeshProUGUI>();
        }

        if (healthText == null)
        {
            var go = GameObject.Find("HealthText");
            if (go) healthText = go.GetComponent<TextMeshProUGUI>();
        }

        if (comboText == null)
        {
            var go = GameObject.Find("ComboText");
            if (go) comboText = go.GetComponent<TextMeshProUGUI>();
        }
    }

    // GameManager calls this
    public void Refresh(int score, int hp)
    {
        if (scoreText != null)
            scoreText.text = $"SCORE {score}";

        if (healthText != null)
            healthText.text = $"HP: {hp}";
    }

    // ComboManager can call this (optional)
    public void SetCombo(string text)
    {
        if (comboText == null) return;
        comboText.text = text;
    }

    // Optional helpers
    public void ClearCombo()
    {
        if (comboText == null) return;
        comboText.text = "";
    }
}
