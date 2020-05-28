using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiggle : MonoBehaviour
{
    public float amplitude;
    public bool notUI;

    private RectTransform tr;
    private Vector2 referencePositon;
    private float timer, timerY;
    
    void Start()
    {
        if (!notUI)
        {
            tr = GetComponent<RectTransform>();
            referencePositon = tr.position;
        }
        else
        {
            referencePositon = transform.position;
        }
        timer = Random.Range(0, 1f);
        timerY = Random.Range(0, 1f);
    }

    private void FixedUpdate()
    {
        if (!notUI)
        {
            tr.position = new Vector2(referencePositon.x + amplitude * (Mathf.PerlinNoise(timer, timer * timerY) - 0.5f),
                referencePositon.y + amplitude * (Mathf.PerlinNoise(timer * timer, timerY) - 0.5f));
        }
        else
        {
            transform.position = new Vector2(referencePositon.x + amplitude * (Mathf.PerlinNoise(timer, timer * timerY) - 0.5f),
                referencePositon.y + amplitude * (Mathf.PerlinNoise(timer * timer, timerY) - 0.5f));
        }
        if (timer < 1)
        {
            timer += 0.01f;
        }
        else
        {
            timer -= 1;
        }
        if(timerY < 1)
        {
            timerY += 0.002f;
        }
        else
        {
            timerY -= 1;
        }
    }
}
