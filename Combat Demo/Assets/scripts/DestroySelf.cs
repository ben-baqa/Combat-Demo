using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    private void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
