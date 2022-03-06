using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterArea : MonoBehaviour
{
    public float WindForce_Y = 45f;

    private GameObject Character; // A valider quand le perso sera pret a fonctionner
    //public GameObject Character;

    private Rigidbody2D rb;

    public Animator anim;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetBool("isActivate", true);
            Character = other.gameObject;
            rb = Character.GetComponent<Rigidbody2D>();
            rb.AddForce(new Vector2(0, WindForce_Y));
        }
    }
}
