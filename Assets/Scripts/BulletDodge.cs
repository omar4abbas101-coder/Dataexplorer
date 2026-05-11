using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class BulletDodge : MonoBehaviour
{
    [Header("refs")]
    [HideInInspector] public Enemy enemy;

    [Header("dodging")]
    [SerializeField] float dodgeTime;
    [SerializeField] float auraOffsetX;
    [SerializeField] float auraOffsetY;
    [HideInInspector] public Coroutine currentDodge;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Projectile")
        {
            Debug.Log("BulletDodge: bullet detected");
            StartCoroutine(DodgeCheck(collision.gameObject));
        }
    }

    private void Update()
    {
        FollowEnemy();
    }

    void FollowEnemy()
    {
        // follows the enemy if it still exists
        if (enemy != null)
        {
            Vector3 auraPos = new Vector3(enemy.transform.position.x + auraOffsetX * enemy.moveDirection, enemy.transform.position.y + auraOffsetY, enemy.transform.position.z);
            transform.position = auraPos;
        }
        else Destroy(this.gameObject);
    }

    IEnumerator DodgeCheck(GameObject bullet)
    {
        if (bullet == null || currentDodge != null) yield break;

        bool isBulletAhead = (enemy.moveDirection == 1) ? enemy.transform.position.x < bullet.transform.position.x : enemy.transform.position.x > bullet.transform.position.x;

        currentDodge = StartCoroutine(Dodge(isBulletAhead));
    }

    /// <summary>
    /// Temporarily changes direction to avoid the bullet;
    /// </summary>
    /// <returns></returns>
    IEnumerator Dodge(bool isBulletAhead)
    {
        Debug.Log("BulletDodge: starting the dodge");

        if (isBulletAhead) enemy.ChangeDirection();
        enemy.moveSpeed *= 2;
        yield return new WaitForSeconds(dodgeTime);
        enemy.moveSpeed /= 2;
        if (isBulletAhead) enemy.ChangeDirection();

        currentDodge = null;
    }

    public void StopDodge()
    {
        if (currentDodge == null) return;

        enemy.moveSpeed /= 2;
        StopCoroutine(currentDodge);
        currentDodge = null;
    }
}
