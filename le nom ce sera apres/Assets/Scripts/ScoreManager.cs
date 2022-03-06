using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int collectibleScore = 200;
    private GameObject Player;
    public AnimationCurve animCurve;
    private float graphValue;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player = other.gameObject;
            InfiniteMovement.instance.scorePlayer += collectibleScore;
            Destroy(gameObject);
        }
    }
}
