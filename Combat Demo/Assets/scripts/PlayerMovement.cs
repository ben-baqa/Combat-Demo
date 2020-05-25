using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform jumpChecker;
    public Animator swordAnim;

    public float moveForce, jumpForce, maxSpeed;
    public string[] rightKeyCodes, leftKeyCodes, upKeyCodes, downKeyCodes, attackKeyCodes, parryKeyCodes;

    private Animator anim;
    private Rigidbody2D rb;
    private Transform tr;
    //private Sword sword;

    private int lastPressed;
    private bool right, left, up, down, attack, parry, canJump;


    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0.5f, 0.5f, 0.7f);
        Gizmos.DrawCube(jumpChecker.position, jumpChecker.localScale);
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
        //sword = GetComponentInChildren<Sword>();

        Physics2D.IgnoreLayerCollision(8, 9, true);
    }
    private void Update()
    {
        GetControls();
    }
    private void FixedUpdate()
    {
        RunAnimator();
        ExecuteControls();
    }
    /// <summary>
    /// Runs the Logic of the controls using obtained input
    /// </summary>
    private void ExecuteControls()
    {
        if(up & canJump)
        {
            up = false;
            anim.SetTrigger("Jump");
        }
        if (right && rb.velocity.x < maxSpeed)
        {
            lateralMovement(1);
        }
        if (left && rb.velocity.x > -maxSpeed)
        {
            lateralMovement(-1);
        }
        if (attack)
        {
            swordAnim.SetTrigger("attack");
            attack = false;
        }
    }

    private void lateralMovement(int flip)
    {
        lastPressed = flip;
        rb.AddForce(Vector2.right * moveForce * flip, ForceMode2D.Force);
        if (rb.velocity.x * flip < 0)
        {
            rb.AddForce(Vector2.right * moveForce * flip, ForceMode2D.Force);
        }
        if (tr.localScale.x == -flip)
        {
            if (canJump)
            {
                anim.SetTrigger("turn");
            }
            else
            {
                FlipCharacter();
            }
        }
        else
        {
            anim.ResetTrigger("turn");
        }
    }

    /// <summary>
    /// Sets various parameters of the player animator
    /// </summary>
    private void RunAnimator()
    {
        anim.SetFloat("vX", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("vY", rb.velocity.y);
        Collider2D[] col = Physics2D.OverlapBoxAll(jumpChecker.position, jumpChecker.localScale, 0);
        if(col.Length > 1)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }
        swordAnim.SetBool("air", !canJump);
    }

    private void CheckTurn()
    {
        if (rb.velocity.x > 0)
        {
            tr.localScale = new Vector2(1, 1);
        }
        else
        {
            tr.localScale = new Vector2(-1, 1);
        }
    }
    /// <summary>
    /// gets the values of all inputs
    /// </summary>
    private void GetControls()
    {
        right |= GetKeyDowns(rightKeyCodes);
        left |= GetKeyDowns(leftKeyCodes);
        up |= GetKeyDowns(upKeyCodes);
        up &= canJump;
        down |= GetKeyDowns(downKeyCodes);
        attack |= GetKeyDowns(attackKeyCodes) | Input.GetMouseButtonDown(0);
        parry |= GetKeyDowns(parryKeyCodes) | Input.GetMouseButtonDown(1);

        right &= GetKeyUps(rightKeyCodes);
        left &= GetKeyUps(leftKeyCodes);
        up &= GetKeyUps(upKeyCodes);
        down &= GetKeyUps(downKeyCodes);
        attack &= GetKeyUps(attackKeyCodes) & !Input.GetMouseButtonUp(0);
        parry &= GetKeyUps(parryKeyCodes) & !Input.GetMouseButtonUp(1);
    }
    /// <summary>
    /// returns true if any of the passed keycodes have been pressed
    /// </summary>
    /// <param name="ar"> the list of keyCodes to check</param>
    /// <returns></returns>
    private bool GetKeyDowns(string[] ar)
    {
        foreach (string s in ar)
        {
            if (Input.GetKeyDown(s))
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// returns false if any of the passed keycodes have been released
    /// </summary>
    /// <param name="ar"> the list of keyCodes to check</param>
    /// <returns></returns>
    private bool GetKeyUps(string[] ar)
    {
        foreach (string s in ar)
        {
            if (Input.GetKeyUp(s))
            {
                return false;
            }
        }
        return true;
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///     Animation Events           ///
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Called in the jump animation, gives player upward force impulse
    /// </summary>
    private void Jump()
    {
        rb.velocity = Vector2.right * rb.velocity.x;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
    /// <summary>
    /// Called in the turn animation, turns the player in the correct direction
    /// </summary>
    private void FlipCharacter()
    {
        if (right && !left)
        {
            tr.localScale = new Vector3(1, 1, 1);
            return;
        }
        else if (!right && left)
        {
            tr.localScale = new Vector3(-1, 1, 1);
            return;
        }
        tr.localScale = new Vector3(lastPressed, 1, 1);
    }
}
