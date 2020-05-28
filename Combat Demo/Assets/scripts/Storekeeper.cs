using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Storekeeper : MonoBehaviour
{
    public GameObject canvas;

    private Manager manager;
    
    void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<Manager>();
    }

    private void FixedUpdate()
    {
        ManageButtons();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canvas.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canvas.SetActive(false);
        }
    }
    /// <summary>
    /// Sets the shop buttons to be active if the cost requirements are met, also sets cost displays
    /// </summary>
    private void ManageButtons()
    {
        Button[] buttons = GetComponentsInChildren<Button>();
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = manager.CostMet(i);
            int cost = manager.getCost(i);
            Text text = buttons[i].GetComponentInChildren<Text>();
            if (cost < 2147483647)
            {
                text.text = cost.ToString();
            }
            else
            {
                text.text = "";
            }
        }
    }
}
