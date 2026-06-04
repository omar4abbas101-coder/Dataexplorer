using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float lifeTime;

    private void Start()
    {
        // destroying the projectile after lifeTime runs out
        Destroy(this.gameObject, lifeTime);
    }
    void Update()
    {
        // moving the projectile forward
        transform.Translate(0, speed * Time.deltaTime, 0, Space.Self);
    }
}
