using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector2 offset, noDecayOffset;

    public float xMod, yMod, offsetDecayFactor, maxTimeSlow;

    private GameObject player;
    private Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = GetComponent<Transform>();
    }

    /// <summary>
    /// sets the camera position a fraction of the distance away from centered on the player, y damping factor is reduced by
    /// sufficiantly large velocities so as to not loose the player when they are dropping to fast.
    ///    Probably don't touch unless you understand it.
    /// </summary>
    void FixedUpdate()
    {
        if(Time.timeScale < 1)
        {
            StartCoroutine(ResetTimeScale());
        }
        else
        {
            StopAllCoroutines();
        }
        if (player != null)
        {
            float offsetMod = 0;
            if (offset.magnitude > 0.01)
            {
                offset *= offsetDecayFactor;
                //offsetMod = 1 / (Mathf.Abs(offset.y) + 0.01f);
                offsetMod = Mathf.Abs(offset.y) * 50;
            }
            Vector3 camPos = cam.position, pPos = player.transform.position;
            cam.position = new Vector3
                (camPos.x - (camPos.x - pPos.x) / xMod + offset.x + noDecayOffset.x,
                camPos.y - (camPos.y - pPos.y - (float)0.1) / (yMod / (1 + Mathf.Pow((0.1f * player.GetComponent<Rigidbody2D>().velocity.y
                + offsetMod) / 2, 2))) + offset.y + noDecayOffset.y, -10);
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    IEnumerator ResetTimeScale()
    {
        yield return new WaitForSeconds(maxTimeSlow);
        Time.timeScale = 1;
    }
}
