using System.Collections;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [Header("Death Settings")]
    [SerializeField] GameObject deathEffectPrefab;
    [SerializeField] AudioClip deathSound;
    [SerializeField] float deathDelay = 1f;
    [SerializeField] float transitionStartDelay = 0.5f;

    bool isDead = false;

    void Update()
    {
        if (isDead) return;
        if (GameManager.Instance == null) return;

        if (GameManager.Instance.HP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        // Play death sound
        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.simulated = false;
        }

        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.enabled = false;
        }

        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            if (script != this)
                script.enabled = false;
        }

        StartCoroutine(DeathRoutine());
    }

    IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(deathDelay + transitionStartDelay);

        if (SceneLoader.instance != null)
        {
            SceneLoader.instance.LoadScene("GameOver");
        }
    }
}