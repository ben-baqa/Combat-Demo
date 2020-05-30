﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class NextLevelTrigger : MonoBehaviour
{
    public string nextFloor;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene(nextFloor, LoadSceneMode.Single);
    }
}
