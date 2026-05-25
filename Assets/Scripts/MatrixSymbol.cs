using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MatrixSymbol : MonoBehaviour
{
    public SpriteRenderer sprite;
    [HideInInspector] public float speed = 1f;

    public void InitSymbol(Sprite spriteToUse, Color colorToUse, float scaleToUse, float speedToUse)
    {
        sprite.sprite = spriteToUse;
        transform.localScale = new Vector3(scaleToUse, scaleToUse);
        sprite.color = colorToUse;
        speed = speedToUse;
    }

    private void FixedUpdate()
    {
        // moving the symbol
        transform.Translate(0, -speed, 0);

        // destroying the symbol once it reaches off screen
        if (sprite.bounds.max.y < Level.instance.GetScreenBottom())
        {
            Destroy(this.gameObject);
        }
    }
}
