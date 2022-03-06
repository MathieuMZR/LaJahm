using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class DeathZone : MonoBehaviour
{
    private bool notNull;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other != null)
            {
                GameObject[] objs ;
                objs = GameObject.FindGameObjectsWithTag("Player");
                foreach(GameObject player in objs)
                {
                    Destroy(player);
                }
            }
        }
    }
}
