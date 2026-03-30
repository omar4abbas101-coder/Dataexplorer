using System.Collections;
using UnityEngine;

public class PlayerPowerUps : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerShooter shooter;
    [SerializeField] private SpriteRenderer shipRenderer;

    [Header("Rapid Fire Visuals")]
    [SerializeField] private Color rapidFireColor = Color.red;
    [SerializeField] private float pulseSpeed = 8f;

    [Header("Magnet")]
    [SerializeField] private int magnetMaxHits = 64;

    [Header("Gizmos")]
    [SerializeField] private bool showMagnetGizmos = true;
    [SerializeField] private float magnetPreviewRadius = 3f;

    private Color baseShipColor;

    private bool magnetActive;
    private float magnetRadius;

    private Coroutine rapidRoutine;
    private Coroutine magnetRoutine;

    private Collider2D[] magnetHits;

    void Awake()
    {
        if (shooter == null) shooter = GetComponentInChildren<PlayerShooter>();
        if (shipRenderer == null) shipRenderer = GetComponentInChildren<SpriteRenderer>();

        if (shipRenderer != null) baseShipColor = shipRenderer.color;

        magnetHits = new Collider2D[Mathf.Max(8, magnetMaxHits)];
    }

    // =========================
    // RAPID FIRE
    // =========================
    public void ActivateRapidFire(float duration, float cooldownMultiplier)
    {
        if (shooter == null) return;

        if (rapidRoutine != null) StopCoroutine(rapidRoutine);
        rapidRoutine = StartCoroutine(RapidFireRoutine(duration, cooldownMultiplier));
    }

    private IEnumerator RapidFireRoutine(float duration, float cooldownMultiplier)
    {
        if (shipRenderer != null) baseShipColor = shipRenderer.color;

        shooter.fireCooldown = shooter.baseFireCooldown * cooldownMultiplier;

        float t = 0f;
        while (t < duration)
        {
            if (shipRenderer != null)
            {
                float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;
                shipRenderer.color = Color.Lerp(baseShipColor, rapidFireColor, pulse);
            }

            t += Time.deltaTime;
            yield return null;
        }

        shooter.fireCooldown = shooter.baseFireCooldown;

        if (shipRenderer != null) shipRenderer.color = baseShipColor;

        rapidRoutine = null;
    }

    // =========================
    // MAGNET
    // =========================
    public void ActivateMagnet(float duration, float radius, float pullSpeed, LayerMask pickupLayer)
    {
        if (magnetRoutine != null) StopCoroutine(magnetRoutine);
        magnetRoutine = StartCoroutine(MagnetRoutine(duration, radius, pullSpeed, pickupLayer));
    }

    private IEnumerator MagnetRoutine(float duration, float radius, float pullSpeed, LayerMask pickupLayer)
    {
        magnetActive = true;
        magnetRadius = radius;

        float t = 0f;
        while (t < duration)
        {
            int count = Physics2D.OverlapCircleNonAlloc(transform.position, magnetRadius, magnetHits, pickupLayer);

            // Debug check (optional): uncomment to verify detection
            // if (Time.frameCount % 60 == 0) Debug.Log("Magnet hits: " + count);

            Vector2 playerPos2D = transform.position;

            for (int i = 0; i < count; i++)
            {
                var c = magnetHits[i];
                if (!c) continue;

                // IMPORTANT: each collectible MUST have a Collider2D or it can’t be detected here.

                Rigidbody2D rb = c.attachedRigidbody;
                if (rb != null)
                {
                    Vector2 newPos = Vector2.MoveTowards(rb.position, playerPos2D, pullSpeed * Time.deltaTime);
                    rb.MovePosition(newPos);
                }
                else
                {
                    c.transform.position = Vector3.MoveTowards(c.transform.position, transform.position, pullSpeed * Time.deltaTime);
                }
            }

            t += Time.deltaTime;
            yield return null;
        }

        magnetActive = false;
        magnetRoutine = null;
    }

    // =========================
    // GIZMOS
    // =========================
    void OnDrawGizmos()
    {
        if (!showMagnetGizmos) return;

        float r = Application.isPlaying ? magnetRadius : magnetPreviewRadius;
        if (Application.isPlaying && !magnetActive) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, r);
    }
}
