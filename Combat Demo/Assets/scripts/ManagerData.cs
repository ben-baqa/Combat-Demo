using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerData : MonoBehaviour
{
    /// <summary>
    /// incrementally keeps track of upgrade levels. Order: Damage, Slam, Parry health, Parry time, health
    /// </summary>
    public int[] upgradeLevels, prices;
    public int coins;
    public bool destroyOnLoad;

    void Start()
    {
        if (!destroyOnLoad)
            DontDestroyOnLoad(gameObject);
    }
}
