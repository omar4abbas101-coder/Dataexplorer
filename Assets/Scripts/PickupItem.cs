using UnityEngine;

public class PickupItem : MonoBehaviour
{
    private AudioSource audioSource;
    public int pickupscore;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        // Increase score
        GameManager.Instance.AddScore(pickupscore, transform.position, Color.yellow);
        AudioManager.instance.PlaySFX("CollectSFX");
      
        Destroy(gameObject);
    }
}
