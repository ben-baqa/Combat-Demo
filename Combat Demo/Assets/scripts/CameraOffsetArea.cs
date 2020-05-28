using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOffsetArea : MonoBehaviour
{
    public float cameraOffsetGoal, offsetMovementFactor, offsetBaseMovement;

    private CameraFollow cam;

    private float cameraOffset;
    private bool active;
    
    void Start()
    {
        cam = GameObject.Find("Camera").GetComponent<CameraFollow>();
    }

    private void FixedUpdate()
    {
        CameraShift();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            active = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            active = false;
        }
    }

    private void CameraShift()
    {
        cameraOffset = 0;
        if (active && cameraOffset < cameraOffsetGoal)
        {
            cameraOffset = offsetBaseMovement + offsetMovementFactor * (cameraOffsetGoal - cam.noDecayOffset.y);
        }
        else if (!active && cam.noDecayOffset.y > 0)
        {
            cameraOffset = - offsetBaseMovement - offsetMovementFactor * (cam.noDecayOffset.y);
        }
        cam.noDecayOffset = new Vector2(0, cam.noDecayOffset.y + cameraOffset);
    }
}
