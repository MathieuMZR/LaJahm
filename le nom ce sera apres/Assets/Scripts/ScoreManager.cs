using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int collectibleScore = 200;
    private GameObject Player;
    private float graphValue;
    public ParticleSystem PS;
    private ParticleSystem PSInstantate;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player = other.gameObject;
            InfiniteMovement.instance.scorePlayer += collectibleScore;
            PSInstantate = Instantiate(PS, gameObject.transform.position, Quaternion.identity);
            PSInstantate.Play();
            Destroy(gameObject);
        }
    }
}
