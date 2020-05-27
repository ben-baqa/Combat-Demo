using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pouch : MonoBehaviour
{
    public int coins;

    private GameObject hud;
    private Text coinDisplay;
    // Start is called before the first frame update
    void Start()
    {
        hud = GameObject.Find("HUD");
        coinDisplay = hud.GetComponentInChildren<Text>();
        coins = 0;
    }

    private void FixedUpdate()
    {
        coinDisplay.text = coins.ToString(); ;
    }

    public void addCoins(int n)
    {
        coins += n;
    }
}
