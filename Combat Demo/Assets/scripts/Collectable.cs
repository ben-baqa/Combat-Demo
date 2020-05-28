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
        GetComponentInParent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-forcePar[0], forcePar[0]), 
            Random.Range(forcePar[1], forcePar[2])), ForceMode2D.Impulse);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GetComponentInParent<Animator>().SetTrigger("collect");
            GetComponentInParent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            GetCollected();
        }
    }

    private void GetCollected()
    {
        switch (type){
            case CollectableType.coin:
                GameObject.Find("Manager").GetComponent<Manager>().coins++;
                break;
        }
    }
}
