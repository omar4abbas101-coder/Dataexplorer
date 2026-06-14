using System.Collections;
using UnityEngine;

public class HealthPowerUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerShooter player = other.GetComponent<PlayerShooter>();

            if (player != null)
            {
                HealPlayer();
            }
        }
    }

    private void HealPlayer()
    {
        if (GameManager.Instance.HP < GameManager.Instance.startHP) GameManager.Instance.PlayerHit(-1);

        Destroy(gameObject);
    }
}
