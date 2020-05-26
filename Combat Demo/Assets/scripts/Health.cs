using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public GameObject deathBurst, bar;

    public float deathDelay, healthMax;
    public int health;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        health = (int)healthMax;
    }
    /// <summary>
    /// Called when this object is hit by an attack
    /// </summary>
    /// <param name="damage">How much damage to take</param>
    public void GetHit(int damage)
    {
        health -= damage;
        UpdateHealthBar();
        anim.SetTrigger("oof");
        if (health <= 0)
        {
            StartCoroutine(OnDeath());
        }
    }
    /// <summary>
    /// Called when health reaches 0
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnDeath()
    {
        yield return new WaitForSeconds(deathDelay);
        if (deathBurst != null)
        {
            Instantiate(deathBurst, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// Updates the look of the health bar
    /// </summary>
    public void UpdateHealthBar()
    {
        if (health > healthMax)
        {
            health = (int)healthMax;
        }
        bar.transform.localScale = new Vector2(health / healthMax, 1);
    }
}
