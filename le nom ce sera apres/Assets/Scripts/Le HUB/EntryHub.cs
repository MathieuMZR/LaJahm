using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryHub : MonoBehaviour
{
    
    private GameObject Character;

    public float newMoveSpeed;
    public bool isUi = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Character = other.gameObject;
            //movement.moveSpeed = moveSpeed;
            InfiniteMovement.instance.moveSpeed = newMoveSpeed;
        }
        
        if (isUi == true)
        {
            Debug.Log("C'est le début des emmerdes");
            StartCoroutine(TimeWaitForUI());
        }
    }

    IEnumerator TimeWaitForUI()
    {
        yield return new WaitForSeconds(3);//Play Annnimation
        Debug.Log("C'est l'heure des télétubbies");
    }
}
