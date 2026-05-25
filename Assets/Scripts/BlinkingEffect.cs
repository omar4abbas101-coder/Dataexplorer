using UnityEngine;
using System.Collections;
using TMPro;

public class BlinkingEffect : MonoBehaviour
{
    [SerializeField] float blinkingIntervals = 1;
    TextMeshProUGUI sprite;
    private void OnEnable()
    {
        StartCoroutine(Blinking());
        sprite = GetComponent<TextMeshProUGUI>();
        sprite.enabled = true;
    }

    IEnumerator Blinking()
    {
        float t = 0;

        while (gameObject.activeSelf)
        {
            t += Time.deltaTime;

            if (t >  blinkingIntervals)
            {
                t = 0;
                sprite.enabled = !sprite.enabled;
            }

            yield return null;
        }
    }
}
