using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public GameObject raycastLaser1;
    public GameObject raycastLaser2;
    public LayerMask playerLayerMask;

    public float lenghtRaycast = 10f;
    public bool shootReady = false;

    private int index = 0;
    
    
    private void Update()
    {
        StartCoroutine(TimeToChargeLaser());

        if (shootReady == true && index < 1)
        {
            index += 1;
            StartCoroutine(TimeToStopLaser());
            Debug.DrawRay(raycastLaser1.transform.position, transform.TransformDirection(Vector2.left) 
                                                            * lenghtRaycast, Color.red, 0.2f);
            RaycastHit2D hit1 = Physics2D.Raycast(raycastLaser1.transform.position, 
                transform.TransformDirection(Vector2.left), lenghtRaycast, playerLayerMask);
            
            
            Debug.DrawRay(raycastLaser2.transform.position, transform.TransformDirection(Vector2.left) 
                                                            * lenghtRaycast, Color.red, 0.2f);
            RaycastHit2D hit2 = Physics2D.Raycast(raycastLaser2.transform.position, 
                transform.TransformDirection(Vector2.left), lenghtRaycast, playerLayerMask);
            
            if (hit1 || hit2)
            {
                // Kill the player
            }
        }
    }

    IEnumerator TimeToChargeLaser()
    {
        if (shootReady == false)
        {
            yield return new WaitForSeconds(3);
            shootReady = true;
        }
    }
    
    IEnumerator TimeToStopLaser()
    {
        if (shootReady == true)
        {
            yield return new WaitForSeconds(1);
            shootReady = false;
        }
    }
    
}
