using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlamCaster : MonoBehaviour
{
    public LayerMask whatIsGround;
    public float swordOffset, bodyOffset;

    public float groundCushion;
    
    void Start()
    {
        Vector2 swordCast = new Vector2 (transform.position.x + swordOffset, transform.position.y - 1.4f),
            bodyCast = new Vector2(transform.position.x + bodyOffset, transform.position.y - 1.4f);
        RaycastHit2D[] hits = Physics2D.RaycastAll(swordCast, Vector2.down,  whatIsGround);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.CompareTag("Ground"))
            {
                swordCast = hit.point;
            }
        }
        hits = Physics2D.RaycastAll(bodyCast, Vector2.down, whatIsGround);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.CompareTag("Ground"))
            {
                bodyCast = hit.point;
            }
        }
        if(swordCast.y < bodyCast.y)
        {
            DoSlam(bodyCast);
        }
        else
        {
            DoSlam(swordCast);
        }
        Destroy(gameObject);
    }
    /// <summary>
    /// Teleports the player to the Contact point
    /// </summary>
    /// <param name="point">Which contact point to use</param>
    private void DoSlam(Vector2 point)
    {
        Transform pPos = GameObject.Find("Player").GetComponent<Transform>();
        Rigidbody2D pRB = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        pPos.Translate(new Vector2(0, point.y - transform.position.y + groundCushion));
        pRB.velocity = Vector2.zero;
    }
}
