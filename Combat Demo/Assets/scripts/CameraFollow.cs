using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public Vector2 offset;

    public float xMod, yMod, offsetDecayFactor;

    private Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Transform>();
    }

    /// <summary>
    /// sets the camera position a fraction of the distance away from centered on the player, y damping factor is reduced by
    /// sufficiantly large velocities so as to not loose the player when they are dropping to fast.
    ///    Probably don't touch unless you understand it.
    /// </summary>
    void FixedUpdate()
    {
        float offsetMod = 20;
        if (offset.magnitude > 0.01)
        {
            offset *= offsetDecayFactor;
            offsetMod = 1 / (Mathf.Abs(offset.y) + 0.01f);
        }
        Vector3 camPos = cam.position, pPos = player.transform.position;
        cam.position = new Vector3
            (camPos.x - (camPos.x - pPos.x) / xMod + offset.x,
            camPos.y - (camPos.y - pPos.y - (float)0.1) / (yMod / (1 + Mathf.Pow((player.GetComponent<Rigidbody2D>().velocity.y
            + offsetMod) / 4,2))) + offset.y, -10);
    }
}
