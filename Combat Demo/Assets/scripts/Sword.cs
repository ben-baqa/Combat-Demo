using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public Camera cam;
    public GameObject slamCast;
    public Vector2[] cameraShakeOffsets;

    public float timeSlowAmount, parrySlowAmount;
    public int damage, healthRestoredOnParry, slamDamageMod;
    public bool contact;

    private List<GameObject> enemiesAlreadyDamagedInSwing, enemiesAlreadyParriedInSwing;
    private GameObject player;
    private Animator anim;
    private SpriteRenderer sprite;
    private PlayerMovement pMov;
    private Health playerHealth;
    private Vector2 pScale;

    private int normalDamage;

    void Start()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player");
        pMov = player.GetComponent<PlayerMovement>();
        playerHealth = player.GetComponentInChildren<Health>();
        enemiesAlreadyDamagedInSwing = new List<GameObject>();
        enemiesAlreadyParriedInSwing = new List<GameObject>();
        normalDamage = damage;
    }

    private void FixedUpdate()
    {
        if(pMov == null)
        {
            Destroy(gameObject);
            return;
        }
        if(transform.parent == null)
        {
            transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 0.8f);
        }
    }
    /// <summary>
    /// Applies an offset to the camera position to make screen shake
    /// </summary>
    /// <param name="n">which inspector defined vector to apply as offset</param>
    private void CameraShake(int n)
    {
        cam.GetComponent<CameraFollow>().offset = cameraShakeOffsets[n] * Time.timeScale;
    }
    /// <summary>
    /// Called in DamageEnemies when a swing hits something hittable, slows time and changes sword colour
    /// </summary>
    public void OnHit()
    {
        contact = true;
        sprite.color = new Color(1, 0.1f, 0);
        Time.timeScale = 0.3f;
        StartCoroutine(CancelBulletTime(timeSlowAmount));
    }
    /// <summary>
    /// Removes time slow after a given time delay
    /// </summary>
    /// <param name="time">how long the time slow should last</param>
    /// <returns></returns>
    IEnumerator CancelBulletTime(float time)
    {
        yield return new WaitForSeconds(time);
        Time.timeScale = 1;
    }
    /// <summary>
    /// Called on a successfull parry, ensures each enmy can only be parried once per swing
    /// </summary>
    public void OnParry(GameObject enemy)
    {
        bool parryEnemy = true;
        foreach (GameObject e in enemiesAlreadyParriedInSwing)
        {
            if (e.Equals(enemy))
            {
                parryEnemy = false;
            }
        }
        if (parryEnemy)
        {
            sprite.color = new Color(1, 1, 0);
            Time.timeScale = 0.3f;
            StartCoroutine(CancelParryTime());
            anim.speed = 5;
            playerHealth.health += healthRestoredOnParry;
            playerHealth.UpdateHealthBar();
            enemy.GetComponent<EnemyBehavior>().GetParried();
            enemiesAlreadyParriedInSwing.Add(enemy);
        }
    }
    /// <summary>
    /// Cancels slow time cuased by parrying
    /// </summary>
    /// <returns></returns>
    IEnumerator CancelParryTime()
    {
        yield return new WaitForSeconds(parrySlowAmount);
        anim.speed = 1;
        Time.timeScale = 1;
    }
    /// <summary>
    /// Called when a damaging stroke contacts an enemy, ensures each enemy is only hit once
    /// </summary>
    /// <param name="enemy">the enemy to hit</param>
    public void HitEnemy(GameObject enemy)
    {
        bool hitEnemy = true;
        foreach (GameObject e in enemiesAlreadyDamagedInSwing)
        {
            if (e.Equals(enemy))
            {
                hitEnemy = false;
            }
        }
        if (hitEnemy)
        {
            enemy.GetComponent<Health>().GetHit(damage);
            enemiesAlreadyDamagedInSwing.Add(enemy);
        }
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///     Store Buttons           ///
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Called by store UI, upgrades attack
    /// </summary>
    public void UpgradeAttack()
    {
        normalDamage++;
        damage = normalDamage;
    }
    /// <summary>
    /// Called in store UI, upgrades slam attack
    /// </summary>
    public void UpgradeSlam()
    {
        slamDamageMod++;
    }
    /// <summary>
    /// Called in store UI, upgrades parry slow time
    /// </summary>
    public void UpgradeParryTime()
    {
        parrySlowAmount += 0.05f;
    }
    /// <summary>
    /// Called in store UI, upgrades health restored on parry
    /// </summary>
    public void UpgradeParryHeal()
    {
        healthRestoredOnParry++;
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///     Animation Events           ///
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Called at the start of a sword swing, detaches sword from parent transform
    /// </summary>
    private void startSwing()
    {
        damage = normalDamage;
        transform.parent = null;
        contact = false;
        enemiesAlreadyDamagedInSwing.Clear();
        enemiesAlreadyParriedInSwing.Clear();
    }
    /// <summary>
    /// Called at the end of a sword swing, resets parent connection
    /// </summary>
    private void endSwing()
    {
        sprite.color = Color.white;
        if (player != null)
        {
            transform.parent = player.transform;
        }
        transform.localPosition = new Vector2(0, 0.8f);
        transform.localScale = new Vector2(1, 1);
        GetComponentInParent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    /// <summary>
    /// Activates a numbered gameobject that detects hits on a certain frame
    /// </summary>
    ///// <param name="n">the index of the hitbox to trigger</param>
    //private void ActivateDamageBox(int n)
    //{
    //    foreach(GameObject g in swingChecker)
    //    {
    //        g.SetActive(false);
    //    }
    //    swingChecker[n].SetActive(true);
    //}
    ///// <summary>
    ///// deactivates all sword hitboxes;
    ///// </summary>
    //private void DeactivateDamageBoxes()
    //{
    //    foreach (GameObject g in swingChecker)
    //    {
    //        g.SetActive(false);
    //    }
    //}
    /// <summary>
    /// Called in attack animations to prevent one input press from triggering multiple attacks
    /// </summary>
    private void ResetTrigger()
    {
        anim.ResetTrigger("attack");
    }
    /// <summary>
    /// Prevents overlayins parry swings
    /// </summary>
    /// <param name="i"></param>
    private void EnableParry(int i)
    {
        if(i == 0)
        {
            anim.SetBool("canParry", false);
        }
        else
        {
            anim.SetBool("canParry", true);
        }
    }
    /// <summary>
    /// Called in slam animation, triggers downward slam
    /// </summary>
    private void OnSlamDown()
    {
        damage *= slamDamageMod;
        Instantiate(slamCast, transform.position, Quaternion.identity);
    }
}
