using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    public GameObject[] hurtBox;
    public Vector2 moveForce;

    public float animSpeed, attackDistance;
    public int attackDelay, attackTimer;
    public bool contact, parryable;

    private Animator anim;
    private Rigidbody2D rb;
    private Transform pPos;

    private bool roam;

    public enum Bosstype { kingSlime, Ent, Medusa }
    public Bosstype type;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        pPos = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void FixedUpdate()
    {
        switch (type)
        {
            case Bosstype.kingSlime:
                SlimeAttackLoop();
                break;
            case Bosstype.Ent:
                EntActivity();
                break;
            case Bosstype.Medusa:
                MedusaActivity();
                break;
        }
    }
    /// <summary>
    /// Runs the main attack loop for the king slime
    /// </summary>
    private void SlimeAttackLoop()
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
            if (buffer != null)
            {
                pPos = buffer.transform;
            }
        }
    }
    /// <summary>
    /// Runs the main attack loop for the ent
    /// </summary>
    private void EntActivity()
    {
        if (Mathf.Abs(rb.velocity.x) > 1)
        {
            rb.AddForce(new Vector2(moveForce.x * transform.localScale.x, moveForce.y), ForceMode2D.Force);
        }
        else
        {
            rb.AddForce(new Vector2(moveForce.x * transform.localScale.x, moveForce.y), ForceMode2D.Impulse);
        }
        animSpeed = Mathf.Abs(rb.velocity.x);
        anim.SetFloat("speed", animSpeed);
    }
    /// <summary>
    /// Runs the main attack loop for the medusa
    /// </summary>
    private void MedusaActivity()
    {
        if (pPos != null)
        {
            if (!roam)
            {
                if (pPos.position.x > transform.position.x)
                    transform.localScale = new Vector2(1, 1);
                else
                    transform.localScale = new Vector2(-1, 1);
            }
            if (Vector2.Distance(pPos.position, transform.position) < attackDistance)
            {
                roam = false;
                if (attackTimer > attackDelay)
                {
                    attackTimer = 0;
                    anim.SetTrigger("attack");
                }
                attackTimer++;
            }
            else
            {
                roam = true;
                if (Mathf.Abs(rb.velocity.x) > 1)
                    rb.AddForce(new Vector2(moveForce.x * transform.localScale.x, moveForce.y), ForceMode2D.Force);
                else
                    rb.AddForce(new Vector2(moveForce.x * transform.localScale.x, moveForce.y), ForceMode2D.Impulse);
                animSpeed = Mathf.Abs(rb.velocity.x);
                anim.SetFloat("speed", animSpeed);
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
    /// Removes time slow after a given time delay
    /// </summary>
    /// <param name="time">how long the time slow should last</param>
    /// <returns></returns>
    IEnumerator CancelBulletTime(float time)
    {
        yield return new WaitForSeconds(time);
        Time.timeScale = 1;
    }
    /// <summary>
    /// Called when the enemy hits the player
    /// </summary>
    /// <param name="time"></param>
    public void OnHit(float time)
    {
        StartCoroutine(CancelBulletTime(time));
    }
    /// <summary>
    /// Called when the enemy is parried
    /// </summary>
    public void GetParried()
    {
        ActivateHurtBox(-1);
        anim.SetTrigger("oof");
        rb.velocity = Vector2.zero;
        if(type == Bosstype.Medusa)
            attackTimer = - 2 * attackDelay;
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
        Vector2 v = moveForce;
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
    /// Resets the animator atrtack trigger
    /// </summary>
    private void ResetAttackTrigger()
    {
        anim.ResetTrigger("attack");
    }
    /// <summary>
    /// Sets the damaging hitbox to be active or not
    /// </summary>
    /// <param name="n"></param>
    private void ActivateHurtBox(int n)
    {
        if (n < 0)
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
