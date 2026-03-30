using System.Collections;
using UnityEngine;
using TMPro;

public class ComboShakeFX : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI comboText;

    [Header("Shake Settings")]
    [SerializeField] float shakeDuration = 0.15f;
    [SerializeField] float shakeStrength = 8f;

    RectTransform rect;
    Vector2 originalPos;
    Coroutine shakeRoutine;

    void Awake()
    {
        if (comboText == null)
            comboText = GetComponent<TextMeshProUGUI>();

        rect = comboText.rectTransform;
        originalPos = rect.anchoredPosition;
    }

    public void PlayShake()
    {
        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        shakeRoutine = StartCoroutine(ShakeRoutine());
    }

    IEnumerator ShakeRoutine()
    {
        float t = 0f;

        while (t < shakeDuration)
        {
            Vector2 offset = Random.insideUnitCircle * shakeStrength;
            rect.anchoredPosition = originalPos + offset;

            t += Time.unscaledDeltaTime;
            yield return null;
        }

        rect.anchoredPosition = originalPos;
        shakeRoutine = null;
    }
}
