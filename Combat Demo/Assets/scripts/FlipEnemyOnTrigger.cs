using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipEnemyOnTrigger : MonoBehaviour
{
    public bool feet;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!feet && collision.CompareTag("Ground"))
            transform.parent.localScale = new Vector2(transform.parent.localScale.x * -1, 1);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (feet && collision.CompareTag("Ground"))
            transform.parent.localScale = new Vector2(transform.parent.localScale.x * -1, 1);
    }
}
