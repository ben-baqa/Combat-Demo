using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax : MonoBehaviour
{
    private Transform tr;
    private Transform ct;
    private float xRef, xTar;
    private float yRef, yTar;

    /// <summary>
    /// Amount by which to move by, larger for farther away objects
    /// </summary>
    public float amount;
    public bool parallaxY;
    public float yAmount;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        ct = GameObject.Find("Camera").transform;
        xRef = ct.position.x;
        xTar = 0;
        yRef = ct.position.y;
        yTar = 0;
    }

    void FixedUpdate()
    {
        xTar = (ct.position.x - xRef) / amount;
        yTar = tr.position.y;
        if (parallaxY)
        {
            yTar = (ct.position.y - yRef) / yAmount;
        }
        tr.position = new Vector2(xTar, yTar);
    }
}
