using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Storekeeper : MonoBehaviour
{
    public GameObject canvas;
    public float cameraOffsetGoal, offsetMovementFactor, offsetBaseMovement;

    private CameraFollow cam;
    private Pouch pouch;

    private float cameraOffset;
    private bool active;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Camera").GetComponent<CameraFollow>();
        pouch = GameObject.Find("Player").GetComponent<Pouch>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        EnablePurchases(pouch.coins >= 10);
        CameraShift();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canvas.SetActive(true);
            active = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canvas.SetActive(false);
            active = false;
        }
    }

    private void CameraShift()
    {
        if (active && cameraOffset < cameraOffsetGoal)
        {
            cameraOffset += offsetBaseMovement + offsetMovementFactor * (cameraOffsetGoal - cameraOffset);
        }
        else if (!active && cameraOffset > 0)
        {
            cameraOffset -= offsetBaseMovement + offsetMovementFactor * (cameraOffset);
        }
        cam.noDecayOffset = new Vector2(0, cameraOffset);
    }

    private void EnablePurchases(bool e)
    {
        Button[] buttons = GetComponentsInChildren<Button>();
        foreach(Button b in buttons)
        {
            b.interactable = e;
        }
    }
}
