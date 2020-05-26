using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject thingToSpawn;

    private Animator anim;
    
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Collider2D[] collisions = Physics2D.OverlapBoxAll(new Vector2(44-0.09f, 0.5f-0.08f), new Vector2(0.6f, 0.85f), 0);
        foreach(Collider2D col in collisions)
        {
            if (col.CompareTag("Player Attack") || col.CompareTag("Parry"))
            {
                anim.SetTrigger("oof");
            }
        }
    }
    /// <summary>
    /// Resets the trigger to prevent unintended looping
    /// </summary>
    private void ResetTrigger()
    {
        anim.ResetTrigger("oof");
    }
    /// <summary>
    /// Spawns the thing, called in trigger animation
    /// </summary>
    private void SpawnThing()
    {
        Vector2 spawnPosition = new Vector2(Random.Range(25, 35), Random.Range(0, -3));
        Instantiate(thingToSpawn, spawnPosition, Quaternion.identity);
    }
}
