using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float movementSpeed = 0;

    private void FixedUpdate()
    {
        LaserMovement();
    }

    void LaserMovement()
    {
        transform.Translate(0, -movementSpeed * Time.deltaTime, 0, Space.World);
    }
}
