using UnityEngine;

public class SpeedBoostPickup : MonoBehaviour
{
    public float speedIncrease = 3f;     // how much speed to add
    public float duration = 5f;          // how long it lasts

   private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        PlayerController2D player = other.GetComponent<PlayerController2D>();

        if (player != null)
        {
            StartCoroutine(ApplySpeedBoost(player));

            // hide object instead of destroying immediately
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
    }
}

    private System.Collections.IEnumerator ApplySpeedBoost(PlayerController2D player)
{
    float originalSpeed = player.moveSpeed;

    player.moveSpeed += speedIncrease;

    yield return new WaitForSeconds(duration);

    player.moveSpeed = originalSpeed;

    Destroy(gameObject);
}
}