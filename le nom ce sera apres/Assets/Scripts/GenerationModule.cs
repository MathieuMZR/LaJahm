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
    public GameObject _nextSpawnPrefab;
    public GameObject nextSpawnOffset;

    private int _randomFloat;

    private int _indexList;

    private bool canSpawn = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canSpawn = true;
            _randomFloat = Random.Range(0, StackGrids.instance.prefabList.Count);
            _nextSpawnPrefab = StackGrids.instance.prefabList[_randomFloat];

            Debug.Log(nextSpawnOffset.transform.position);
            GameObject spawnedPrefab =
                Instantiate(_nextSpawnPrefab, this.transform.position + StackGrids.instance.offset,
                    Quaternion.identity);


            StackGrids.instance.oldSpawnedPrefab.Add(spawnedPrefab);
            _indexList = StackGrids.instance.oldSpawnedPrefab.Count;
        }

        /*if (StackGrids.instance.oldSpawnedPrefab.Count == 4 || 
            StackGrids.instance.oldSpawnedPrefab.Count == 8 ||
            StackGrids.instance.oldSpawnedPrefab.Count == 12)
        {
            canSpawn = true;
        }
        else
        {
            if (other.CompareTag("Player"))
            {
                canSpawn = true;
                _randomFloat = Random.Range(0, StackGrids.instance.prefabList.Count);
                _nextSpawnPrefab = StackGrids.instance.prefabList[_randomFloat];

                Debug.Log(nextSpawnOffset.transform.position);
                GameObject spawnedPrefab =
                    Instantiate(_nextSpawnPrefab, this.transform.position + StackGrids.instance.offset, 
                        Quaternion.identity);


                StackGrids.instance.oldSpawnedPrefab.Add(spawnedPrefab);
                _indexList = StackGrids.instance.oldSpawnedPrefab.Count;
            }*/
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
                
                //StackGrids.instance.oldSpawnedPrefab.RemoveAt(0);
            }
        }
    }

    private void Update()
    {
        //Debug.Log(stackGrids);
    }
}
