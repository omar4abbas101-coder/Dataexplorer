using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float movementSpeed = 0;
    public int damageAmount = 1;

    private void FixedUpdate()
    {
        LaserMovement();
    }

    void LaserMovement()
    {
        transform.Translate(0, -movementSpeed * Time.deltaTime, 0, Space.World);
    }

private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.TakeDamage(1);
        }
    }
}
}