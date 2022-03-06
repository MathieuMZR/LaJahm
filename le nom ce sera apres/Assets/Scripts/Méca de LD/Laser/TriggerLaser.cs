using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLaser : MonoBehaviour
{
    public bool isTriggered;
    public GameObject Laser;
    private GameObject Character;
    
    public Vector3 offSet = new Vector3(0, 0, 0);
    private int indexLaser = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        isTriggered = true;
        if (indexLaser < 1)
        {
            Instantiate(Laser, offSet, Quaternion.identity);
            
            indexLaser += 1;
        }
    }


    private void Start()
    {
        GameObject[] objs ;
        objs = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in objs)
        {
            Character = player;
        }
    }
}
