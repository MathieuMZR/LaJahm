using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterArea : MonoBehaviour
{
    private bool isWindy = false;
    public float WindForce_Y = 45f;

    public InfiniteMovement Character; // A valider quand le perso sera pret a fonctionner
    //public GameObject Character;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = Character.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        isWindy = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isWindy)
        {
            rb.AddForce(new Vector2(0, WindForce_Y));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isWindy = false;
    }
}
