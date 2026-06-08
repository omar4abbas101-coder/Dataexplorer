using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Text References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI comboText;

    [Header("Mute Button")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource musicSource;
    [SerializeField] Image muteButton;
    [SerializeField] Sprite mutedSprite;
    [SerializeField] Sprite unmutedSprite;

    [Header("Boss refs")]
    public GameObject bossHpObj;
    public Image bossHpFill;
    public GameObject dashIndicator;

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
            scoreText.text = score.ToString();

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

    public void MuteButton()
    {
        // muting the sound and music
        musicSource.mute = !musicSource.mute;

        // setting correct visuals of the mute button
        muteButton.sprite = (musicSource.mute) ? mutedSprite : unmutedSprite;
    }
}
