using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBurst : MonoBehaviour
{
    private ParticleSystem pSystem;
    void Start()
    {
        pSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pSystem.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
