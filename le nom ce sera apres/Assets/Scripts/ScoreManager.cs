using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int collectibleScore = 200;
    private GameObject Character;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /*private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Character = other.gameObject;
            Character.score += collectibleScore;
            Debug.Log(sH.score);
            Destroy(gameObject);
        }
    }*/
}
