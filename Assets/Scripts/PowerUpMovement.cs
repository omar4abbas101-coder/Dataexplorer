using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpMovement : MonoBehaviour
{
    public float fallingSpeed = 0.02f;

    private void FixedUpdate()
    {
        transform.Translate(0f, -fallingSpeed, 0f);
    }
}
