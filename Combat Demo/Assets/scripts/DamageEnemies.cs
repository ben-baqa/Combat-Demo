using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnemies : MonoBehaviour
{
    public Vector2 hitForce;

    private Transform pPos;

    void Start()
    {
        pPos = transform.parent.parent;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Yeet");
            GameObject enemy = collision.gameObject;
            enemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(pPos.localScale.x * hitForce.x, hitForce.y), ForceMode2D.Impulse);
        }
    }
}
