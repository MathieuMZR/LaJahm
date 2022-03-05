using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class GenerationModule : MonoBehaviour
{
    public List<GameObject> prefabList;

    private GameObject nextSpawnPrefab;
    public GameObject nextSpawnOffset;
    private List<GameObject> oldSpawnedPrefab;
    
    private int randomFloat;
    
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
            Debug.Log("YO");
            randomFloat = Random.Range(0, prefabList.Count);
            nextSpawnPrefab = prefabList[randomFloat];

            GameObject spawnedPrefab = Instantiate(nextSpawnPrefab);
            spawnedPrefab.transform.position = nextSpawnOffset.transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
        }
    }
}
