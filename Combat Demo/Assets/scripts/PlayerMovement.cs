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
    private void OnDestroy()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
    /// <summary>
    /// Runs the Logic of the controls using obtained input
    /// </summary>
    private void ExecuteControls()
    {
        if(up & canJump)
        {
            up = false;
            rb.velocity = Vector2.right * rb.velocity.x;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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
        if (parry)
        {
            swordAnim.SetTrigger("parry");
            parry = false;
        }
        swordAnim.SetBool("down", down);
    }
    /// <summary>
    /// Moves the player laterally in wither the positive or negative direction
    /// </summary>
    /// <param name="flip">1 or -1 depending on if inout is left or right</param>
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
                FlipCharacter();
        }
    }
    /// <summary>
    /// Sets various parameters of the player animator
    /// </summary>
    private void RunAnimator()
    {
        anim.SetFloat("vX", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("vY", rb.velocity.y);
        Collider2D col = Physics2D.OverlapBox(jumpChecker.position, jumpChecker.localScale, 0, 1 << 10);
        if(col)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }
        swordAnim.SetBool("air", !canJump);
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
