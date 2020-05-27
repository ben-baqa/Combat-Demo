using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Vector2 jumpForce;

    public int attackDelay, attackTimer;
    public bool contact, parryable;

    private Animator anim;
    private Rigidbody2D rb;
    private Transform pPos;

    enum type {slime}

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        pPos = GameObject.Find("Player").transform;
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
        if (pPos != null && pPos.position.x > transform.position.x)
        {
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            transform.localScale = new Vector2(-1, 1);
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
    }
    ///// <summary>
    ///// Called in attack animation, specifies activates damage hitbox
    ///// </summary>
    ///// <param name="n">the index of the damage box to activate</param>
    //private void ActivateDamageBox(int n)
    //{
    //    DamageBox[n].SetActive(true);
    //}
    ///// <summary>
    ///// Called at the end of an attack animation, deactivates all damaging hitboxes
    ///// </summary>
    //private void DeactivateDamageBoxes()
    //{
    //    foreach(GameObject g in DamageBox)
    //    {
    //        g.SetActive(false);
    //    }
    //}
    /// <summary>
    /// Called at the start of an attack animation, resets contact variable so player is only hit once
    /// </summary>
    private void startAttack()
    {
        contact = false;
    }
    /// <summary>
    /// Called in attack animation, sets wether or not the enemy can currently be parried
    /// </summary>
    /// <param name="i"></param>
    private void setParryable(int i)
    {
        if (i == 0)
        {
            parryable = false;
        }
        else
        {
            parryable = true;
        }
    }
}
