using TMPro;
using UnityEngine;

public class ComboUI : MonoBehaviour
{
    public TextMeshProUGUI comboText;

    void Awake()
    {
        if (comboText == null)
            comboText = GetComponent<TextMeshProUGUI>();

        Hide();
    }

    public void Show(int combo, int mult, float normalizedTimeLeft)
    {
        comboText.gameObject.SetActive(true);
        comboText.text = $"COMBO x{mult}";

        // fade a bit as it’s about to expire
        Color c = comboText.color;
        c.a = Mathf.Lerp(0.35f, 1f, normalizedTimeLeft);
        comboText.color = c;
    }

    public void Hide()
    {
        if (comboText != null)
            comboText.gameObject.SetActive(false);
    }
}
