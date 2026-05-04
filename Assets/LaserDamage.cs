using UnityEngine;

public class LaserDamage : MonoBehaviour
{
    public int damage = 1;
    public float damageInterval = 0.5f;

    float timer;

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        timer += Time.deltaTime;

        if (timer >= damageInterval)
        {
            timer = 0f;

            if (GameManager.Instance != null)
                GameManager.Instance.TakeDamage(damage);
        }
    }


    void OnTriggerEnter2D(Collider2D other)
{
   if (!other.CompareTag("Player")) return;
            
   GameManager.Instance.TakeDamage(damage);
    
}


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            timer = 0f;
        }
    }
}