using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public GameObject[] hurtBox;
    public GameObject roamNose;
    public Vector2 jumpForce, moveForce, maxSpeed;

    public float attackDistance, homingDistance, chaseSpeedMod;
    public int attackDelay, attackTimer;
    public bool contact, parryable;

    private Animator anim;
    private Rigidbody2D rb;
    private Transform pPos;

    private float maxSpeedBase;
    private bool limitXSpeed;

    public enum Enemytype {slime, meelee, ranged}
    public Enemytype type;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        pPos = GameObject.FindGameObjectWithTag("Player").transform;
        maxSpeedBase = maxSpeed.x;
        limitXSpeed = true;
    }

    void Update()
    {
        anim.SetFloat("vY", rb.velocity.y);
        anim.SetFloat("vX", Mathf.Abs(rb.velocity.x) + 0.1f);
    }
    private void FixedUpdate()
    {
        switch (type)
        {
            case Enemytype.slime:
                SlimeAttackLoop();
                break;
            case Enemytype.meelee:
                MeeleeActivityLoop();
                break;
        }
        if(Mathf.Abs(rb.velocity.y) > maxSpeed.y)
            rb.velocity = new Vector2(rb.velocity.x, maxSpeed.y * Mathf.Abs(rb.velocity.y) / rb.velocity.y);
        if (limitXSpeed && Mathf.Abs(rb.velocity.x) > maxSpeed.x)
            rb.velocity = new Vector2(maxSpeed.x * Mathf.Abs(rb.velocity.x) / rb.velocity.x, rb.velocity.y);
    }
    /// <summary>
    /// Faces the player
    /// </summary>
    private void OrientSelf()
    {
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
            if (buffer != null)
            {
                pPos = buffer.transform;
            }
        }
    }
    /// <summary>
    /// Runs the main attack loop for slime type enemies
    /// </summary>
    private void SlimeAttackLoop()
    {
        OrientSelf();
        if (pPos != null && Vector2.Distance(pPos.position, transform.position) < attackDistance)
        {
            if (attackTimer > attackDelay && rb.velocity.y == 0)
            {
                attackTimer = 0;
                anim.SetTrigger("attack");
            }
        }
        attackTimer++;
    }
    /// <summary>
    /// Runs the attack behavior for meelee enemies
    /// </summary>
    private void MeeleeActivityLoop()
    {
        if (pPos != null )
        {
            float dist = Vector2.Distance(pPos.position, transform.position);
            if(dist < attackDistance)
            {
                if (attackTimer > attackDelay)
                {
                    attackTimer = 0;
                    anim.SetTrigger("attack");
                }
            }else if(dist< homingDistance)
            {
                maxSpeed = new Vector2(maxSpeedBase * chaseSpeedMod, maxSpeed.y);
                OrientSelf();
                Move();
            }
            else
            {
                maxSpeed = new Vector2(maxSpeedBase, maxSpeed.y);
                Move();
            }
            roamNose.SetActive(dist >= homingDistance);
        }
        attackTimer++;
    }
    /// <summary>
    /// Applies movement force
    /// </summary>
    private void Move()
    {
        if (Mathf.Abs(rb.velocity.x) > maxSpeed.x / 2)
        {
            rb.AddForce(new Vector2(moveForce.x * transform.localScale.x, moveForce.y), ForceMode2D.Force);
        }
        else
        {
            rb.AddForce(new Vector2(moveForce.x * transform.localScale.x, moveForce.y), ForceMode2D.Impulse);
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
        contact = true;
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
    /// Sets the value of limitXSpeed
    /// </summary>
    /// <param name="n"></param>
    private void SetVXConstraints(int n)
    {
        if (n == 0)
            limitXSpeed = false;
        else
            limitXSpeed = true;
    }
    /// <summary>
    /// Called at the end of an attack animation, disables damage and parrying
    /// </summary>
    private void EndAttack()
    {
        parryable = false;
        ActivateHurtBox(-1);
        limitXSpeed = true;
    }
    /// <summary>
    /// Called in attack animation, specifies activates damage hitbox
    /// </summary>
    /// <param name="n">the index of the damage box to activate</param>
    private void ActivateHurtBox(int n)
    {
        foreach (GameObject h in hurtBox)
        {
            h.SetActive(false);
        }
        if (n < 0)
            return;

        hurtBox[n].SetActive(true);
    }
}
