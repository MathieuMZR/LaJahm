using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubManager : MonoBehaviour
{
    public float newMoveSpeed;
    private GameObject Character;
    
    public Transform Player;
    public Transform respawnPointHub1;
    public Transform respawnPointHub2;
    public Transform respawnPointHub3;

    public Transform respawnPointModule1;
    public GameObject RespawnPointModule1;
    
    public GameObject Hub_1;
    public GameObject Hub_2;
    public GameObject Hub_3;

    public Vector3 hub1_position;
    public Vector3 hub2_position;
    public Vector3 hub3_position;

    public int index = 0;
    public bool canAcessToHub = false;

    private bool isHub1 = false;
    private bool isHub2 = false;
    private bool isHub3 = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Character = other.gameObject;
            Player = other.transform;
            InfiniteMovement.instance.moveSpeed = newMoveSpeed;
        }
        
        if (canAcessToHub == true)
        {
            StartCoroutine(TimeWaitForUI());
        }
    }

    IEnumerator TimeWaitForUI()
    {
        yield return new WaitForSeconds(3.5f);//Play Annnimation
        Debug.Log("C'est l'heure des télétubbies");
        
        if (index == 0) //Teleport player into hub 1
        {
            Player.transform.position = respawnPointHub1.transform.position;
            index += 1;
            isHub1 = true;
        }
        else if (index == 1)//Teleport player into hub 2
        {
            Player.transform.position = respawnPointHub2.transform.position;
            index += 1;
            isHub2 = true;
        }
        else if (index == 2) //Teleport player into hub 3
        {
            Player.transform.position = respawnPointHub3.transform.position;
            isHub3 = true;
        }
    }

    private void Update()
    {
        if (isHub1 == true)
        {
            Hub1();
        }
        else if (isHub2 == true)
        {
            Hub2();
        }
        else if (isHub3 == true)
        {
            Hub3();
        }
    }

    private void Awake()
    {
        Instantiate(Hub_1, hub1_position, Quaternion.identity); //Hub 1
        Instantiate(Hub_2, hub2_position, Quaternion.identity); //Hub 2
        Instantiate(Hub_3, hub3_position, Quaternion.identity); //Hub 3
    }

    IEnumerator ComeBackHub1()
    {
        yield return new WaitForSeconds(1f);
        Player.transform.position = respawnPointModule1.transform.position;
        newMoveSpeed = 20f;
        yield return new WaitForSeconds(0.3f);
        RespawnPointModule1.SetActive(false);
        Debug.Log("Hello");
        RespawnPointModule1.SetActive(true);
        newMoveSpeed = 0f;
    }
    
    IEnumerator ComeBackHub2()
    {
        yield return new WaitForSeconds(1f);
        Player.transform.position = respawnPointModule1.transform.position;
        newMoveSpeed = 20f;
        yield return new WaitForSeconds(0.3f);
        RespawnPointModule1.SetActive(false);
        RespawnPointModule1.SetActive(true);
        newMoveSpeed = 0f;
    }
    
    IEnumerator ComeBackHub3()
    {
        yield return new WaitForSeconds(1f);
        Player.transform.position = respawnPointModule1.transform.position;
        newMoveSpeed = 20f;
        yield return new WaitForSeconds(0.3f);
        RespawnPointModule1.SetActive(false);
        RespawnPointModule1.SetActive(true);
        newMoveSpeed = 0f;
    }

    void Hub1()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(ComeBackHub1());
        }
    }
    
    void Hub2()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(ComeBackHub2());
        }
    }
    
    void Hub3()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(ComeBackHub3());
        }
    }
}
