using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public GameObject[] hurtBox;
    public Vector2 jumpForce;

    public int attackDelay, attackTimer;
    public bool contact, parryable;

    private Animator anim;
    private Rigidbody2D rb;
    private Transform pPos;

    public enum Enemytype {slime}
    public Enemytype type;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        pPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("vY", rb.velocity.y);
    }
    private void FixedUpdate()
    {
        AttackLoop();
    }
    /// <summary>
    /// Runs the main attack loop
    /// </summary>
    private void AttackLoop()
    {
        if (attackTimer > attackDelay && rb.velocity.y == 0)
        {
            attackTimer = 0;
            anim.SetTrigger("attack");
        }
        else
        {
            attackTimer++;
        }
        if (pPos != null)
        {
            if (pPos.position.x > transform.position.x)
            {
                transform.localScale = new Vector2(1, 1);
            }
            else
            {
                transform.localScale = new Vector2(-1, 1);
            }
        }
        else
        {
            GameObject buffer = GameObject.FindGameObjectWithTag("Player");
            if(buffer != null)
            {
                pPos = buffer.transform;
            }
        }
    }
    /// <summary>
    /// Called when the enemy is parried
    /// </summary>
    public void GetParried()
    {
        ActivateHurtBox(-1);
        anim.SetTrigger("oof");
        rb.velocity = Vector2.zero;
        attackTimer = -attackDelay;
        parryable = false;
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///     Animation Events           ///
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Called in jump animation, applies force
    /// </summary>
    private void Jump()
    {
        Vector2 v = jumpForce;
        v.x *= transform.localScale.x;
        rb.AddForce(v, ForceMode2D.Impulse);
        contact = false;
        parryable = true;
        ActivateHurtBox(0);
    }
    /// <summary>
    /// Called at the end of an attack animation, disables damage and parrying
    /// </summary>
    private void EndAttack()
    {
        parryable = false;
        ActivateHurtBox(-1);
    }
    /// <summary>
    /// Called in attack animation, specifies activates damage hitbox
    /// </summary>
    /// <param name="n">the index of the damage box to activate</param>
    private void ActivateHurtBox(int n)
    {
        if(n < 0)
        {
            foreach (GameObject h in hurtBox)
            {
                h.SetActive(false);
            }
            return;
        }
        hurtBox[n].SetActive(true);
    }
}
