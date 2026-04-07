using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;
    public Transform leftPoint;
    public Transform rightPoint;

    [Header("Shooting")]
    public float fireRate = 1.5f;
    public GameObject bulletPrefab;
    public Transform firePoint;

    private float fireTimer;
    private int moveDirection = 1;
    private float fixedY;

    private float leftOffset;
    private float rightOffset;
    private float startX;

    void Start()
    {
        fixedY = transform.position.y;
        startX = transform.position.x;

        if (leftPoint != null && rightPoint != null)
        {
            leftOffset = Mathf.Min(leftPoint.localPosition.x, rightPoint.localPosition.x);
            rightOffset = Mathf.Max(leftPoint.localPosition.x, rightPoint.localPosition.x);
        }
    }

    void Update()
    {
        MoveLeftRight();

        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            fireTimer = 0f;
            Shoot();
        }

        transform.position = new Vector3(transform.position.x, fixedY, transform.position.z);
    }

    void MoveLeftRight()
    {
        float leftX = startX + leftOffset;
        float rightX = startX + rightOffset;

        float newX = transform.position.x + moveDirection * moveSpeed * Time.deltaTime;
        newX = Mathf.Clamp(newX, leftX, rightX);

        transform.position = new Vector3(newX, fixedY, transform.position.z);

        if (newX >= rightX)
            moveDirection = -1;
        else if (newX <= leftX)
            moveDirection = 1;
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
    }

    void OnDrawGizmos()
    {
        if (leftPoint == null || rightPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(leftPoint.position, 0.12f);
        Gizmos.DrawSphere(rightPoint.position, 0.12f);
        Gizmos.DrawLine(leftPoint.position, rightPoint.position);
    }
}