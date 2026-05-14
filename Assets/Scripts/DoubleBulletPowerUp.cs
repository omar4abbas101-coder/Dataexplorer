using UnityEngine;

public class DoubleBulletPowerUp : MonoBehaviour
{
    public float duration = 5f;          // how long it lasts

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerShooter player = other.GetComponent<PlayerShooter>();

            if (player != null)
            {
                StartCoroutine(ApplyBulletBoost(player));

                // hide object instead of destroying immediately
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<Collider2D>().enabled = false;
            }
        }
    }

    private System.Collections.IEnumerator ApplyBulletBoost(PlayerShooter player)
    {
        player.bulletCount++;
        yield return new WaitForSeconds(duration);
        player.bulletCount--;

        Destroy(gameObject);
    }
}