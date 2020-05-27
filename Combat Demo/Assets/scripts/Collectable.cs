using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public enum CollectableType { coin, potion}
    public CollectableType type;

    public float[] forcePar;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-forcePar[0], forcePar[0]), 
            Random.Range(forcePar[1], forcePar[2])), ForceMode2D.Impulse);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GetComponent<Animator>().SetTrigger("collect");
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            GetComponent<CapsuleCollider2D>().enabled = false;
            GetCollected();
        }
    }

    private void GetCollected()
    {
        switch (type){
            case CollectableType.coin:
                GameObject.Find("Player").GetComponent<Pouch>().coins++;
                break;
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
