using UnityEngine;

public class MatrixSymbol : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    float fallSpeed;
    Camera mainCam;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCam = Camera.main;
    }

    public void InitSymbol(Sprite sprite, Color color, float scale, float speed)
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        spriteRenderer.sprite = sprite;
        spriteRenderer.color = color;
        transform.localScale = Vector3.one * scale;
        fallSpeed = speed;
    }

    void FixedUpdate()
    {
        transform.position += Vector3.down * fallSpeed;

        if (mainCam == null)
        {
            mainCam = Camera.main;
            return;
        }

        float camDistance = -mainCam.transform.position.z;
        Vector3 bottomLeft = mainCam.ViewportToWorldPoint(new Vector3(0, 0, camDistance));

        if (transform.position.y < bottomLeft.y - 1f)
        {
            Destroy(gameObject);
        }
    }
}