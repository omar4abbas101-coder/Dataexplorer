using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    [Header("Default Shake (editable in Inspector)")]
    [SerializeField] float defaultDuration = 0.12f;
    [SerializeField] float defaultStrength = 0.15f;

    Transform camTransform;
    Vector3 startLocalPos;
    Coroutine routine;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        camTransform = transform;
        startLocalPos = camTransform.localPosition;
    }

    public void Shake() => Shake(defaultDuration, defaultStrength);

    public void Shake(float duration, float strength)
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(ShakeRoutine(duration, strength));
    }

    IEnumerator ShakeRoutine(float duration, float strength)
    {
        float t = 0f;

        while (t < duration)
        {
            Vector2 offset = Random.insideUnitCircle * strength;
            camTransform.localPosition = startLocalPos + new Vector3(offset.x, offset.y, 0f);

            t += Time.unscaledDeltaTime;
            yield return null;
        }

        camTransform.localPosition = startLocalPos;
        routine = null;
    }
}
