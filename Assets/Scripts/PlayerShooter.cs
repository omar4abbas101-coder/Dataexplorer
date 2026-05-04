using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;

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
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        if (shootSfx != null && audioSource != null)
            audioSource.PlayOneShot(shootSfx);
    }
}
