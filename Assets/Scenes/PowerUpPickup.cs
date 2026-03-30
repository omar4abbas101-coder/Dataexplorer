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
    public LayerMask pickupLayer; // set this to the layer your collectibles are on

    private bool collected;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;

        // Works even if the collider is on a child object
        var power = other.GetComponentInParent<PlayerPowerUps>()
                 ?? other.GetComponentInChildren<PlayerPowerUps>();

        if (power == null) return;

        collected = true;

        if (type == PowerUpType.RapidFire)
            power.ActivateRapidFire(duration, rapidFireCooldownMultiplier);
        else
            power.ActivateMagnet(duration, magnetRadius, magnetPullSpeed, pickupLayer);

        // Destroy whole prefab instance (parent/child safe)
        Destroy(transform.root.gameObject);
    }
}
