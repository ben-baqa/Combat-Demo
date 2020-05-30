using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public Vector2 pushForce;

    public float timeSlowAmount;
    public int damage;
    public bool isBoss;

    private EnemyBehavior enemy;
    private BossBehavior boss;

    void Start()
    {
        if (!isBoss)
        {
            enemy = GetComponentInParent<EnemyBehavior>();
        }
        else
        {
            boss = GetComponentInParent<BossBehavior>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!isBoss)
            {
                if (enemy.contact)
                    return;
                enemy.contact = true;
            }
            else
            {
                if (boss.contact)
                    return;
                boss.contact = true;
            }
            collision.GetComponent<Health>().GetHit(damage);
            Vector2 v = pushForce;
            v.x *= transform.parent.localScale.x;
            collision.GetComponent<Rigidbody2D>().AddForce(v, ForceMode2D.Impulse);
            if (gameObject.activeSelf)
            {
                Time.timeScale = 0.2f;
                StartCoroutine(CancelBulletTime());
            }
        }
        if (collision.CompareTag("Parry"))
        {
            
        }
    }
    /// <summary>
    /// Cancels slowed time after given delay
    /// </summary>
    /// <returns></returns>
    IEnumerator CancelBulletTime()
    {
        yield return new WaitForSeconds(timeSlowAmount);
        Time.timeScale = 1;
    }
}
