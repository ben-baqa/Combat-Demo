using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public Camera cam;
    public Vector2[] cameraShakeOffsets;
    public GameObject player;
    public GameObject[] swingChecker;

    public float timeSlowAmount;
    public bool contact;

    private Animator anim;
    private SpriteRenderer sprite;
    private Vector2 pScale;

    void Start()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if(transform.parent == null)
        {
            transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 0.8f);
        }
    }

    private void ResetTrigger()
    {
        anim.ResetTrigger("attack");
    }

    private void CameraShake(int n)
    {
        cam.GetComponent<CameraFollow>().offset = cameraShakeOffsets[n];
    }

    private void startSwing()
    {
        transform.parent = null;
        contact = false;
    }

    private void endSwing()
    {
        sprite.color = Color.white;
        transform.parent = player.transform;
        transform.localPosition = new Vector2(0, 0.8f);
        transform.localScale = new Vector2(1, 1);
    }

    private void ActivateDamageBox(int n)
    {
        foreach(GameObject g in swingChecker)
        {
            g.SetActive(false);
        }
        swingChecker[n].SetActive(true);
    }

    private void DeactivateDamageBoxes()
    {
        foreach (GameObject g in swingChecker)
        {
            g.SetActive(false);
        }
    }

    public void OnHit()
    {
        Debug.Log("Time Slowed");
        sprite.color = new Color(1, 0.1f, 0);
        Time.timeScale = 0.3f;
        StartCoroutine(CancelBulletTime(timeSlowAmount));
    }
    IEnumerator CancelBulletTime(float time)
    {
        yield return new WaitForSeconds(time);
        Time.timeScale = 1;
    }
}
