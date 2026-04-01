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
    [SerializeField] private int magnetMaxHits = 25;
    [SerializeField] private float magnetUpdateInterval = 0.05f;

    [Header("Gizmos")]
    [SerializeField] private bool showMagnetGizmos = true;
    [SerializeField] private float magnetPreviewRadius = 3f;

    private Color baseShipColor;

    private bool rapidFireActive;
    private bool magnetActive;

    private float rapidFireTimeRemaining;
    private float magnetTimeRemaining;

    private float magnetRadius;
    private float magnetPullSpeed;
    private LayerMask magnetPickupLayer;

    private Coroutine rapidRoutine;
    private Coroutine magnetRoutine;

    private Collider2D[] magnetHits;

    void Awake()
    {
        if (shooter == null) shooter = GetComponentInChildren<PlayerShooter>();
        if (shipRenderer == null) shipRenderer = GetComponentInChildren<SpriteRenderer>();

        if (shipRenderer != null)
            baseShipColor = shipRenderer.color;

        magnetHits = new Collider2D[Mathf.Max(8, magnetMaxHits)];
    }

    // =========================
    // RAPID FIRE
    // =========================
    public void ActivateRapidFire(float duration, float cooldownMultiplier)
    {
        if (shooter == null) return;

        rapidFireTimeRemaining += duration;

        if (rapidFireActive) return;

        rapidRoutine = StartCoroutine(RapidFireRoutine(cooldownMultiplier));
    }

    private IEnumerator RapidFireRoutine(float cooldownMultiplier)
    {
        rapidFireActive = true;

        if (shipRenderer != null)
            baseShipColor = shipRenderer.color;

        shooter.fireCooldown = shooter.baseFireCooldown * cooldownMultiplier;

        while (rapidFireTimeRemaining > 0f)
        {
            if (shipRenderer != null)
            {
                float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;
                shipRenderer.color = Color.Lerp(baseShipColor, rapidFireColor, pulse);
            }

            rapidFireTimeRemaining -= Time.deltaTime;
            yield return null;
        }

        rapidFireTimeRemaining = 0f;
        rapidFireActive = false;

        shooter.fireCooldown = shooter.baseFireCooldown;

        if (shipRenderer != null)
            shipRenderer.color = baseShipColor;

        rapidRoutine = null;
    }

    // =========================
    // MAGNET
    // =========================
    public void ActivateMagnet(float duration, float radius, float pullSpeed, LayerMask pickupLayer)
    {
        magnetTimeRemaining += duration;
        magnetRadius = radius;
        magnetPullSpeed = pullSpeed;
        magnetPickupLayer = pickupLayer;

        if (magnetActive) return;

        magnetRoutine = StartCoroutine(MagnetRoutine());
    }

    private IEnumerator MagnetRoutine()
    {
        magnetActive = true;

        WaitForSeconds wait = new WaitForSeconds(magnetUpdateInterval);

        while (magnetTimeRemaining > 0f)
        {
            int count = Physics2D.OverlapCircleNonAlloc(
                transform.position,
                magnetRadius,
                magnetHits,
                magnetPickupLayer
            );

            Vector2 playerPos2D = transform.position;

            for (int i = 0; i < count; i++)
            {
                Collider2D c = magnetHits[i];
                if (c == null) continue;

                Rigidbody2D rb = c.attachedRigidbody;

                if (rb != null)
                {
                    Vector2 newPos = Vector2.MoveTowards(
                        rb.position,
                        playerPos2D,
                        magnetPullSpeed * magnetUpdateInterval
                    );

                    rb.MovePosition(newPos);
                }
                else
                {
                    c.transform.position = Vector3.MoveTowards(
                        c.transform.position,
                        transform.position,
                        magnetPullSpeed * magnetUpdateInterval
                    );
                }

                magnetHits[i] = null;
            }

            magnetTimeRemaining -= magnetUpdateInterval;
            yield return wait;
        }

        magnetTimeRemaining = 0f;
        magnetActive = false;

        for (int i = 0; i < magnetHits.Length; i++)
            magnetHits[i] = null;

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