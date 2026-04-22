using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("Shooting")]
    public float fireRate = 1.5f;
    public GameObject bulletPrefab;
    public Transform firePoint;

    private float fireTimer;

    void Update()
    {
        ShootCheck();
    }

    void ShootCheck()
    {
        // if enemy is still appearing it doesn't shoot
        if (GetComponent<Enemy>().appearing) return;

        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            fireTimer = 0f;
            Shoot();
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, -90));
        // I replaced "Quaternion.identity" with "Quaterniom.Euler(0, 0, -90)" to make bullets be spawned at a correct angle
        // Use Quaterniom.Euler(x, y, z) when spawning to set rotation 
    }
}