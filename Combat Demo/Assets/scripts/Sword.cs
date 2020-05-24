using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public Camera cam;
    public Vector2[] cameraShakeOffsets;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void ResetTrigger()
    {
        anim.ResetTrigger("attack");
    }

    private void CameraShake(int n)
    {
        cam.GetComponent<CameraFollow>().offset = cameraShakeOffsets[n];
    }
}
