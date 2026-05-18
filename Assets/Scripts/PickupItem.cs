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
        GameManager.Instance.AddScore( pickupscore);

        // Play sound safely
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
            Destroy(gameObject, audioSource.clip.length);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
