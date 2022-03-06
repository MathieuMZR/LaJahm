using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class GenerationModule : MonoBehaviour
{
    public List<GameObject> prefabList;

    public GameObject _nextSpawnPrefab;
    public GameObject nextSpawnOffset;
    public Vector3 offset;

    private int _randomFloat;

    private int _indexList;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _randomFloat = Random.Range(0, prefabList.Count);
            _nextSpawnPrefab = prefabList[_randomFloat];

            Debug.Log(nextSpawnOffset.transform.position);
            GameObject spawnedPrefab = Instantiate(_nextSpawnPrefab, this.transform.position + offset, Quaternion.identity);


            StackGrids.instance.oldSpawnedPrefab.Add(spawnedPrefab);
            _indexList = StackGrids.instance.oldSpawnedPrefab.Count;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (StackGrids.instance.oldSpawnedPrefab.Count > 2)
            {
                if (StackGrids.instance.basePrefab != null)
                {
                    //Destroy(StackGrids.instance.basePrefab);
                }
                
                //Destroy(StackGrids.instance.oldSpawnedPrefab[0]);
                
                StackGrids.instance.oldSpawnedPrefab.RemoveAt(0);
            }
        }
    }

    private void Update()
    {
        //Debug.Log(stackGrids);
    }
}
