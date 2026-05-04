using UnityEngine;
using System.Collections;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] float flashInterval = 0.1f;

    SpriteRenderer[] sprites;
    Color[] originalColors;
    Coroutine routine;

    void Awake()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>(true);
        originalColors = new Color[sprites.Length];

        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i] != null)
                originalColors[i] = sprites[i].color;
        }
    }

    // Call this when you want the flash to run for a duration (e.g. invincibility time)
    public void Play(float duration)
    {
        if (routine != null)
            StopCoroutine(routine);

        // recache colors right when we start (handles animators / runtime tint changes)
        sprites = GetComponentsInChildren<SpriteRenderer>(true);
        originalColors = new Color[sprites.Length];
        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i] != null)
                originalColors[i] = sprites[i].color;
        }

        routine = StartCoroutine(FlashRoutine(duration));
    }

    IEnumerator FlashRoutine(float duration)
    {
        float endTime = Time.time + duration;
        bool white = false;

        while (Time.time < endTime)
        {
            white = !white;

            for (int i = 0; i < sprites.Length; i++)
            {
                if (sprites[i] == null) continue;
                sprites[i].color = white ? Color.white : originalColors[i];
            }

            yield return new WaitForSeconds(flashInterval);
        }

        // restore
        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i] == null) continue;
            sprites[i].color = originalColors[i];
        }

        routine = null;
    }
}
