using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDeathBurst : MonoBehaviour
{
    public GameObject coin;

    void Start()
    {
        int coins = GameObject.Find("Manager").GetComponent<Manager>().coins;
        GameObject.Find("Manager").GetComponent<Manager>().coins = 0;
        for (int i = 0; i < coins; i++)
        {
            Instantiate(coin, transform);
        }
    }
}
