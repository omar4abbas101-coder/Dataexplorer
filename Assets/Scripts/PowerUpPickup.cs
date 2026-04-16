using UnityEngine;

public class PowerUpPickup : MonoBehaviour
{
    public enum PowerUpType { RapidFire, Magnet }

    [Header("Power Up")]
    public PowerUpType type = PowerUpType.RapidFire;
    public float duration = 5f;

    [Header("Rapid Fire")]
    public float rapidFireCooldownMultiplier = 0.5f;

    [Header("Magnet")]
    public float magnetRadius = 6f;
    public float magnetPullSpeed = 30f;
    public LayerMask pickupLayer;

    private bool collected;
    private Collider2D col;
    private SpriteRenderer sr;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;

        var power = other.GetComponentInParent<PlayerPowerUps>()
                 ?? other.GetComponentInChildren<PlayerPowerUps>();

        if (power == null) return;

        collected = true;

        if (col != null) col.enabled = false;
        if (sr != null) sr.enabled = false;

        if (type == PowerUpType.RapidFire)
            power.ActivateRapidFire(duration, rapidFireCooldownMultiplier);
        else
            power.ActivateMagnet(duration, magnetRadius, magnetPullSpeed, pickupLayer);

        Destroy(transform.root.gameObject);
    }
}