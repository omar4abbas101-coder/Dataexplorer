using System.Collections;
using UnityEngine;
using TMPro;

public class ComboPopFX : MonoBehaviour
{
    public TextMeshProUGUI text;

    [Header("Pop Settings")]
    public float popScale = 1.25f;
    public float popUpTime = 0.08f;
    public float popDownTime = 0.10f;

    Coroutine routine;
    Vector3 baseScale;

    void Awake()
    {
        if (text == null) text = GetComponent<TextMeshProUGUI>();
        baseScale = text.rectTransform.localScale;
    }

    public void Pop()
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(PopRoutine());
    }

    IEnumerator PopRoutine()
    {
        Vector3 target = baseScale * popScale;

        // pop up
        float t = 0f;
        while (t < popUpTime)
        {
            t += Time.unscaledDeltaTime;
            float a = t / Mathf.Max(popUpTime, 0.0001f);
            text.rectTransform.localScale = Vector3.Lerp(baseScale, target, a);
            yield return null;
        }

        // pop down
        t = 0f;
        while (t < popDownTime)
        {
            t += Time.unscaledDeltaTime;
            float a = t / Mathf.Max(popDownTime, 0.0001f);
            text.rectTransform.localScale = Vector3.Lerp(target, baseScale, a);
            yield return null;
        }

        text.rectTransform.localScale = baseScale;
        routine = null;
    }
}
