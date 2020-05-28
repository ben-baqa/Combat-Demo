using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public GameObject playerObject;
    public Image healthRed, healthMaxOutline;
    private Text coinDisplay;
    public Sprite[] healthRedImages, healthMaxImages;
    public Vector2 spawnLocation;

    public float respawnDelay;
    /// <summary>
    /// incrementally keeps track of upgrade levels. Order: Damage, Slam, Parry health, Parry time, health
    /// </summary>
    public int[] upgradeLevels, prices;
    public int displayHealth, displayHealthMax, coins;

    private Health pHealth;
    private Sword sword;

    void Start()
    {
        pHealth = GameObject.Find("Player").GetComponent<Health>();
        sword = GameObject.Find("Player").GetComponentInChildren<Sword>();
        coinDisplay = GameObject.Find("HUD").GetComponentInChildren<Text>();
        DontDestroyOnLoad(coinDisplay.transform.parent.gameObject);
        DontDestroyOnLoad(transform.gameObject);

        Physics2D.IgnoreLayerCollision(8, 9, true);
        Physics2D.IgnoreLayerCollision(9, 11, true);
        Physics2D.IgnoreLayerCollision(9, 12, true);
        Physics2D.IgnoreLayerCollision(11, 11, true);
        Physics2D.IgnoreLayerCollision(11, 12, true);
        Physics2D.IgnoreLayerCollision(12, 12, true);
    }

    private void FixedUpdate()
    {
        if(pHealth == null)
        {
            StartCoroutine(Respawn());
        }
        else
        {
            UpdateHealthBar();
        }
        coinDisplay.text = coins.ToString();
    }
    /// <summary>
    /// Called by shop UI, makes purchase and call function to update upgrades
    /// </summary>
    /// <param name="n">the index of which attribute to upgrade</param>
    public void Upgrade(int n)
    {
        coins -= prices[upgradeLevels[n]];
        upgradeLevels[n]++;
        UpdateUpgrades();
    }
    /// <summary>
    /// Updates the values of upgrades
    /// </summary>
    private void UpdateUpgrades()
    {
        sword.normalDamage = upgradeLevels[0] + 1;
        sword.slamDamageMod = upgradeLevels[1];
        sword.healthRestoredOnParry = upgradeLevels[2];
        sword.parrySlowAmount = 0.3f + upgradeLevels[3] * 0.05f;
        if(upgradeLevels[4] + 3 > pHealth.healthMax)
        {
            pHealth.health++;
        }
        pHealth.healthMax = 3 + upgradeLevels[4];
    }
    /// <summary>
    /// Checks if the cost requirements are met to buy a particular item
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public bool CostMet(int n)
    {
        if(coins >= prices[upgradeLevels[n]])
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// Gets the cost of a perticular upgrade
    /// </summary>
    /// <param name="n">the index of the upgrade to check for</param>
    /// <returns>the cost of the upgrade</returns>
    public int getCost(int n)
    {
        return prices[upgradeLevels[n]];
    }
    /// <summary>
    /// Updates the player health bar
    /// </summary>
    private void UpdateHealthBar()
    {
        if (pHealth.health != displayHealth)
        {
            displayHealth = pHealth.health;
            healthRed.sprite = healthRedImages[displayHealth];
        }
        if (pHealth.healthMax != displayHealthMax)
        {
            displayHealthMax = (int)pHealth.healthMax;
            healthMaxOutline.sprite = healthMaxImages[displayHealthMax - 3];
        }
    }
    /// <summary>
    /// Called upon player death, respawns the player
    /// </summary>
    /// <returns></returns>
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);
        GameObject pInst = Instantiate(playerObject, spawnLocation, Quaternion.identity);
        pHealth = pInst.GetComponent<Health>();
        GameObject[] coins = GameObject.FindGameObjectsWithTag("Collectable");
        foreach(GameObject c in coins)
        {
            Destroy(c);
        }
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject e in enemies)
        {
            Destroy(e);
        }
        StopAllCoroutines();
    }
}
