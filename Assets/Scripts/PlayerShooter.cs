using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;

    [Header("shooting type")]
    [HideInInspector] public int bulletCount = 1;
    [SerializeField] float bulletAngle;

    [Header("Fire Rate")]
    public float baseFireCooldown = 0.25f; // normal cooldown
    public float fireCooldown = 0.25f;     // current cooldown (powerups modify this)

    [Header("Audio")]
    public AudioClip shootSfx;
    AudioSource audioSource;

    float fireTimer;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        fireCooldown = baseFireCooldown; // start normal
    }

    void Update()
    {
        fireTimer -= Time.deltaTime;

        if (Input.GetKey(KeyCode.Space) && fireTimer <= 0f)
        {
            Shoot();
            fireTimer = fireCooldown;
        }
    }

    void Shoot()
    {
        // shooting bullet(s)
        for (int i = 0; i < bulletCount; i++)
        {
            // calculating bullet angle based on how many bullets are shooting at once
            float angle = (bulletAngle / -2 * (bulletCount - 1)) + bulletAngle * i;

            GameObject bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            bullet.transform.Rotate(0, 0, angle);
        }

        // playing shooting soundeffect
        if (shootSfx != null && audioSource != null) audioSource.PlayOneShot(shootSfx);
    }
}
