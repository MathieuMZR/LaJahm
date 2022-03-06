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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Movements());
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
        for (int i = 0; i < 10; i++)
        {
            this.gameObject.transform.position += new Vector3(0f, 0.2f, 0f);
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(0.5f);
        
        for (int j = 0; j > 0; j--)
        {
            this.gameObject.transform.position -= new Vector3(0f, 0.2f, 0f);
            yield return new WaitForSeconds(0.01f);
        }
        
        yield return new WaitForSeconds(0.05f);

        StartCoroutine(Movements());

    }
}
