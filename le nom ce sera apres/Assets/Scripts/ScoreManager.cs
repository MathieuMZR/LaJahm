using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int collectibleScore = 200;
    private GameObject Player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player = other.gameObject;
            InfiniteMovement.instance.scorePlayer += collectibleScore;
            Destroy(gameObject);
        }
    }

    IEnumerator Movements()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(.1f);
        }
    }
}
