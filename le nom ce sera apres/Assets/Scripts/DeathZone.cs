using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class DeathZone : MonoBehaviour
{
    private bool notNull;
    public GameObject initialSpawn;
    private StackGrids stackGrids;
    
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
                    player.transform.position = InitialSpawn.instance.transform.position;

                    /*foreach (GameObject x in StackGrids.instance.oldSpawnedPrefab)
                    {
                        Destroy(x);
                    }
                    StackGrids.instance.oldSpawnedPrefab.Clear();*/
                }
            }
        }
    }
}
