using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : MonoBehaviour
{
    public GameObject burst;

    private Sword sword;
    // Start is called before the first frame update
    void Start()
    {
        sword = GetComponentInParent<Sword>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [System.Obsolete]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && collision.GetComponent<EnemyBehavior>().parryable && !sword.contact)
        {
            sword.OnParry();
            collision.GetComponent<Animator>().SetTrigger("oof");
            collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            EnemyBehavior eB = collision.GetComponent<EnemyBehavior>();
            eB.attackTimer = -eB.attackDelay;

            GameObject burstInst = Instantiate(burst, transform.position, Quaternion.identity);
            burstInst.transform.localScale = transform.parent.localScale;
            burstInst.GetComponent<ParticleSystem>().scalingMode = ParticleSystemScalingMode.Shape;
        }
    }
}
