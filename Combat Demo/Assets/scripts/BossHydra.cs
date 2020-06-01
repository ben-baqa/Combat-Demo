using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHydra : MonoBehaviour
{
    public GameObject thingToSpawn;
    public Vector2[] SpawnPositions;

    public int numberOfSpawns;

    private Animator anim;
    private BossBehavior boss;
    private Health health;

    private float spawnHealthAmount;
    private int spawnNumber;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        boss = GetComponent<BossBehavior>();
        health = GetComponent<Health>();
        spawnHealthAmount = health.healthMax / (numberOfSpawns + 1f);
    }

    private void FixedUpdate()
    {
        if(health.health < spawnHealthAmount * (numberOfSpawns - spawnNumber))
        {
            Vector2 spawn = new Vector2(transform.position.x + (SpawnPositions[spawnNumber].x * transform.localScale.x),
                transform.position.y + SpawnPositions[spawnNumber].y);
            if (health.health > 0)
                Instantiate(thingToSpawn, spawn, Quaternion.identity);
            anim.SetTrigger("bud");
            spawnNumber++;
        }
    }

    public void Reset()
    {
        spawnNumber = 0;
    }
}
