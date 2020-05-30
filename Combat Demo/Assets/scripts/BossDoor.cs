using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    public float openDelay;
    
    private void FixedUpdate()
    {
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        if(boss == null)
        {
            StartCoroutine(OpenWithDelay());
        }
    }

    private IEnumerator OpenWithDelay()
    {
        yield return new WaitForSeconds(openDelay);
        GetComponent<Animator>().SetTrigger("open");
    }
}
