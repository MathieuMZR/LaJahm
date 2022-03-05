using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class Laser : MonoBehaviour
{
    public float speed;
    
    public GameObject raycastLaser1;
    public GameObject raycastLaser2;
    public LayerMask playerLayerMask;

    public float lenghtRaycast = 10f;
    public bool shootReady = false;

    private int index = 0;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    

    private void Update()
    {
        rb.velocity = new Vector2(transform.localScale.x * speed, rb.velocity.y);
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
                Debug.Log("Ta mere");
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
