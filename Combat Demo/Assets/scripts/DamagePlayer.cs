using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public Vector2 pushForce;

    public float timeSlowAmount;
    public int damage;

    private EnemyBehavior enemy;

    void Start()
    {
        enemy = GetComponentInParent<EnemyBehavior>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !enemy.contact)
        {
            enemy.contact = true;
            collision.GetComponent<Health>().GetHit(damage);
            Vector2 v = pushForce;
            v.x *= transform.parent.localScale.x;
            collision.GetComponent<Rigidbody2D>().AddForce(v, ForceMode2D.Impulse);
            Time.timeScale = 0.2f;
            enemy.OnHit(timeSlowAmount);
        }
        if (collision.CompareTag("Parry"))
        {
            
        }
    }
}
