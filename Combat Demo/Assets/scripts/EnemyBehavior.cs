using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public GameObject deathBurst;

    public int healthMax, health;

    private Animator anim;

    enum type {slime}

    void Start()
    {
        anim = GetComponent<Animator>();
        health = healthMax;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetHit()
    {
        anim.SetTrigger("oof");
        health--;
        if(health <= 0)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        if(deathBurst != null)
        {
            Instantiate(deathBurst, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
