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
    [SerializeField] float minDodgeSpeed;
    [SerializeField] float maxDodgeSpeed;
    [SerializeField] float auraOffsetX;
    [SerializeField] float auraOffsetY;
    [HideInInspector] public Coroutine currentDodge;
    float currentDodgeSpeed;

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
        float dodgeSpeed = UnityEngine.Random.Range(minDodgeSpeed, maxDodgeSpeed);

        currentDodge = StartCoroutine(Dodge(isBulletAhead, dodgeSpeed));
    }

    /// <summary>
    /// Temporarily changes direction to avoid the bullet;
    /// </summary>
    /// <returns></returns>
    IEnumerator Dodge(bool dodgeBack, float dodgeSpeed)
    {
        Debug.Log("BulletDodge: starting the dodge");

        if (dodgeBack) enemy.ChangeDirection();
        enemy.moveSpeed *= dodgeSpeed;
        currentDodgeSpeed = dodgeSpeed;

        float dodgeT = (dodgeTime + UnityEngine.Random.Range(-0.12f, 0.12f)) / dodgeSpeed;
        yield return new WaitForSeconds(dodgeT);
        
        enemy.moveSpeed /= dodgeSpeed;
        if (dodgeBack) enemy.ChangeDirection();

        currentDodge = null;
    }

    public void StopDodge()
    {
        if (currentDodge == null) return;

        enemy.moveSpeed /= currentDodgeSpeed;
        StopCoroutine(currentDodge);
        currentDodge = null;
    }
}
