using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiggle : MonoBehaviour
{
    public float amplitude;

    private RectTransform tr;
    private Vector2 referencePositon;
    private float timer;
    
    void Start()
    {
        tr = GetComponent<RectTransform>();
        referencePositon = tr.position;
        timer = Random.Range(0, 1f);
    }

    private void FixedUpdate()
    {
        tr.position = new Vector2(referencePositon.x + amplitude * (Mathf.PerlinNoise(timer, timer * timer) - 0.5f),
            referencePositon.y + amplitude * (Mathf.PerlinNoise(timer * timer, timer) - 0.5f));
        if (timer < 1)
        {
            timer += 0.01f;
        }
        else
        {
            timer -= 1;
        }
    }
}
