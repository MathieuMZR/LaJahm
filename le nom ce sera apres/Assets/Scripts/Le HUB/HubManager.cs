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

    private bool isHub1 = false;
    private bool isHub2 = false;
    private bool isHub3 = false;
    

    public int index = 0;
    public bool canAcessToHub = false;
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

    IEnumerator TimeWaitForUI()
    {
        InfiniteMovement.instance.anim.SetBool("Plug", true);
        yield return new WaitForSeconds(3.5f);//Play Annimation;

        if (index == 0) //Teleport player into hub 1
        {
            Player.transform.position = respawnPointHub1.transform.position;
            index += 1; 
            //StartCoroutine(ComeBackHub1());
            isHub1 = true;
            InfiniteMovement.instance.anim.SetBool("Idle", true);
            InfiniteMovement.instance.anim.SetFloat("Run", 0);

        }
        else if (index == 1)//Teleport player into hub 2
        {
            Player.transform.position = respawnPointHub2.transform.position;
            index += 1;
            //StartCoroutine(ComeBackHub2());
            isHub2 = true;
        }
        else if (index == 2) //Teleport player into hub 3
        {
            Player.transform.position = respawnPointHub3.transform.position;
            //StartCoroutine(ComeBackHub3());
            isHub3 = true;
        }
        
    }
    
    private void Awake()
    {
        Instantiate(Hub_1, new Vector3(-62.68461f, -15.13549f, 0f), Quaternion.identity); //Hub 1
        Instantiate(Hub_2, new Vector3(-73f, -21f, 0f), Quaternion.identity); //Hub 2
        Instantiate(Hub_3, new Vector3(-48f, -21f, 0f), Quaternion.identity); //Hub 3
    }
    

    IEnumerator ComeBackHub1()
    {
        yield return new WaitForSeconds(1f);
        Player.transform.position = respawnPointModule1.transform.position;
        newMoveSpeed = 20f;
        yield return new WaitForSeconds(0.1f);
        RespawnPointModule1.SetActive(false);
        RespawnPointModule1.SetActive(true);
        newMoveSpeed = 0f;
        
        Debug.Log("va te faire foutre");
    }
    
    IEnumerator ComeBackHub2()
    {
        yield return new WaitForSeconds(1f);
        Player.transform.position = respawnPointModule1.transform.position;
        newMoveSpeed = 20f;
        yield return new WaitForSeconds(0.1f);
        RespawnPointModule1.SetActive(false);
        RespawnPointModule1.SetActive(true);
        newMoveSpeed = 0f;
    }
    
    IEnumerator ComeBackHub3()
    {
        yield return new WaitForSeconds(1f);
        Player.transform.position = respawnPointModule1.transform.position;
        newMoveSpeed = 20f;
        yield return new WaitForSeconds(0.1f);
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
