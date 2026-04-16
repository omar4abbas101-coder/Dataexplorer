using System.Collections;
using TMPro;
using UnityEngine;

public class HPTextFX : MonoBehaviour
{
    [Header("Reference")]
    public TextMeshProUGUI hpText;

    [Header("Colors")]
    public Color hp3Color = Color.green;
    public Color hp2Color = Color.yellow;
    public Color hp1Color = Color.red;
    public Color warningPulseColor = Color.white;

    [Header("Glow")]
    public bool enableGlow = true;
    public float glowAt3 = 0.15f;
    public float glowAt2 = 0.35f;

    [Header("⚠️ HP = 1 WARNING PULSE")]
    public float minGlow = 0.2f;
    public float maxGlow = 1.3f;
    public float pulseSpeed = 6f;
    [Range(1f, 6f)] public float pulseSharpness = 3f;

    const string GLOW_POWER = "_GlowPower";

    Material runtimeMat;
    Coroutine pulseRoutine;

    void Awake()
    {
        if (!hpText)
            hpText = GetComponent<TextMeshProUGUI>();

        // VERY IMPORTANT: unique material instance
        runtimeMat = Instantiate(hpText.fontMaterial);
        hpText.fontMaterial = runtimeMat;
    }

    /// <summary>
    /// This is the ONLY method GameManager should ever call
    /// </summary>
    public void SetHP(int hp)
    {
        StopPulse();

        hpText.text = $"HP: {hp}";

        if (hp >= 3)
        {
            hpText.color = hp3Color;
            SetGlow(glowAt3);
        }
        else if (hp == 2)
        {
            hpText.color = hp2Color;
            SetGlow(glowAt2);
        }
        else if (hp == 1)
        {
            pulseRoutine = StartCoroutine(DangerPulse());
        }
        else
        {
            hpText.color = hp1Color;
            SetGlow(0f);
        }
    }

    IEnumerator DangerPulse()
    {
        while (true)
        {
            float t = (Mathf.Sin(Time.unscaledTime * pulseSpeed) + 1f) * 0.5f;
            t = Mathf.Pow(t, pulseSharpness);

            // COLOR pulse (red <-> white)
            hpText.color = Color.Lerp(hp1Color, warningPulseColor, t);

            // GLOW pulse
            float glow = Mathf.Lerp(minGlow, maxGlow, t);
            SetGlow(glow);

            yield return null;
        }
    }

    void StopPulse()
    {
        if (pulseRoutine != null)
        {
            StopCoroutine(pulseRoutine);
            pulseRoutine = null;
        }
    }

    void SetGlow(float value)
    {
        if (!enableGlow || runtimeMat == null) return;

        if (runtimeMat.HasProperty(GLOW_POWER))
            runtimeMat.SetFloat(GLOW_POWER, value);
    }
}
