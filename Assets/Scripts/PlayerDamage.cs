using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
   public float flashSpeed = 12f;          // higher = faster blinking
    public float lowAlpha = 0.25f;          // how transparent at blink low point

    SpriteRenderer sr;
    Color baseColor;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        baseColor = sr.color;
    }

    void Update()
    {
        if (GameManager.Instance == null || sr == null) return;

        if (GameManager.Instance.IsInvincible)
        {
            // Ping-pong alpha while invincible
            float t = Mathf.PingPong(Time.time * flashSpeed, 1f);
            float a = Mathf.Lerp(lowAlpha, 1f, t);

            Color c = baseColor;
            c.a = a;
            sr.color = c;
        }
        else
        {
            // Restore normal color
            sr.color = baseColor;
        }
    }
}
