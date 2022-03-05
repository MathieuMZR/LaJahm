using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class GenerationModule : MonoBehaviour
{
    public List<GameObject> prefabList;

    private GameObject _nextSpawnPrefab;
    public GameObject nextSpawnOffset;

    private int _randomFloat;

    public StackGrids stackGrids;

    private int _indexList;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _randomFloat = Random.Range(0, stackGrids.oldSpawnedPrefab.Count);
            Debug.Log(_randomFloat);
            _nextSpawnPrefab = prefabList[_randomFloat];

            GameObject spawnedPrefab = Instantiate(_nextSpawnPrefab);
            spawnedPrefab.transform.position = nextSpawnOffset.transform.position;
            
            stackGrids.oldSpawnedPrefab.Add(spawnedPrefab);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (stackGrids.oldSpawnedPrefab.Any())
            {
                if (_indexList > 2)
                {
                    Destroy(stackGrids.basePrefab);
                    Destroy(stackGrids.oldSpawnedPrefab[0]);
                    stackGrids.oldSpawnedPrefab.RemoveAt(0);
                }
            }
        }
    }

    private void Update()
    {
        _indexList = stackGrids.oldSpawnedPrefab.Count;
    }
}
