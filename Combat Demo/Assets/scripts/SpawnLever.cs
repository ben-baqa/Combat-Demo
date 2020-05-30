using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLever : MonoBehaviour
{

    public GameObject[] possibleSpawns;
    public Animator door;

    private Animator anim;

    private bool checkForEnemies;
    
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!checkForEnemies)
        {
            Collider2D[] collisions = Physics2D.OverlapBoxAll(new Vector2(transform.position.x - 0.09f,
                transform.position.y + 0.5f - 0.08f), new Vector2(0.6f, 0.85f), 0);
            foreach (Collider2D col in collisions)
            {
                if (col.CompareTag("Player Attack") || col.CompareTag("Parry"))
                {
                    anim.SetTrigger("oof");
                    door.SetTrigger("close");
                }
            }
        }
        CheckForEnemies();
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
    private void TriggerSpawn()
    {
        int index = Random.Range(0, possibleSpawns.Length);
        //Vector2 spawnPosition = new Vector2(Random.Range(25, 35), Random.Range(0, -3));
        Instantiate(possibleSpawns[index]);
        checkForEnemies = true;
    }
    /// <summary>
    /// Checks if there are still enemies alive, if not, opens the door and resets lever
    /// </summary>
    private void CheckForEnemies()
    {
        if (checkForEnemies)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length == 0)
            {
                anim.SetTrigger("return");
                anim.ResetTrigger("oof");
                door.SetTrigger("open");
                door.ResetTrigger("close");
                checkForEnemies = false;
            }
        }
    }
}
