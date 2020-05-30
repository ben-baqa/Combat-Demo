using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public GameObject playerObject;
    public Health bossHealth;
    public Image healthRed, healthMaxOutline;
    private Text coinDisplay;
    public Sprite[] healthRedImages, healthMaxImages;
    public Vector2 spawnLocation;

    public float respawnDelay;
    public int floor;
    public int displayHealth, displayHealthMax, coins;
    public bool destoryHUDonLoad;

    private Health pHealth;
    private Sword sword;
    private ManagerData data;

    void Start()
    {
        pHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        sword = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Sword>();
        coinDisplay = GameObject.Find("HUD").GetComponentInChildren<Text>();
        healthRed = GameObject.Find("Player Health").GetComponent<Image>();
        healthMaxOutline = GameObject.Find("Player Max Health").GetComponent<Image>();
        data = GameObject.FindGameObjectWithTag("Data").GetComponent<ManagerData>();
        coins = data.coins;
        UpdateUpgrades();
        if (!destoryHUDonLoad)
            DontDestroyOnLoad(coinDisplay.transform.parent.gameObject);

        //Physics2D.IgnoreLayerCollision(8, 9, true);
        //Physics2D.IgnoreLayerCollision(9, 11, true);
        //Physics2D.IgnoreLayerCollision(9, 12, true);
        //Physics2D.IgnoreLayerCollision(11, 11, true);
        //Physics2D.IgnoreLayerCollision(11, 12, true);
        //Physics2D.IgnoreLayerCollision(12, 12, true);
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
        data.coins = coins;
    }
    /// <summary>
    /// Called by shop UI, makes purchase and call function to update upgrades
    /// </summary>
    /// <param name="n">the index of which attribute to upgrade</param>
    public void Upgrade(int n)
    {
        coins -= data.prices[data.upgradeLevels[n]];
        data.upgradeLevels[n]++;
        UpdateUpgrades();
    }
    /// <summary>
    /// Updates the values of upgrades
    /// </summary>
    private void UpdateUpgrades()
    {
        sword.normalDamage = data.upgradeLevels[0] + 1;
        sword.UpdateSwordColour();
        sword.slamDamageMod = data.upgradeLevels[1];
        sword.healthRestoredOnParry = data.upgradeLevels[2];
        sword.parrySlowAmount = 0.3f + data.upgradeLevels[3] * 0.05f;
        if(data.upgradeLevels[4] + 3 > pHealth.healthMax)
        {
            pHealth.health++;
        }
        pHealth.healthMax = 3 + data.upgradeLevels[4];
    }
    /// <summary>
    /// Checks if the cost requirements are met to buy a particular item
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public bool CostMet(int n)
    {
        if(floor < 4 && data.upgradeLevels[n] >= floor * 3)
        {
            return false;
        }
        if(coins >= data.prices[data.upgradeLevels[n]])
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
        return data.prices[data.upgradeLevels[n]];
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
        sword = pInst.GetComponentInChildren<Sword>();
        UpdateUpgrades();
        GameObject[] coinObjects = GameObject.FindGameObjectsWithTag("Collectable");
        foreach(GameObject c in coinObjects)
        {
            Destroy(c);
        }
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject e in enemies)
        {
            Destroy(e);
        }
        bossHealth.ResetHealth();
        StopAllCoroutines();
    }
}
