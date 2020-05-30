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

    [System.Obsolete]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && collision.GetComponent<EnemyBehavior>().parryable)
        {
            sword.OnParry(collision.gameObject);
            if (!sword.contact)
            {
                GameObject burstInst = Instantiate(burst, transform.position, Quaternion.identity);
                burstInst.transform.localScale = transform.parent.localScale;
                burstInst.GetComponent<ParticleSystem>().scalingMode = ParticleSystemScalingMode.Shape;
                sword.contact = true;
            }
        }
        else if(collision.CompareTag("Boss") && collision.GetComponent<BossBehavior>().parryable)
        {
            sword.OnBossParry(collision.gameObject);
            if (!sword.contact)
            {
                GameObject burstInst = Instantiate(burst, transform.position, Quaternion.identity);
                burstInst.transform.localScale = transform.parent.localScale;
                burstInst.GetComponent<ParticleSystem>().scalingMode = ParticleSystemScalingMode.Shape;
                sword.contact = true;
            }
        }
    }
}
