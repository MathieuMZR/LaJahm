using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class StackGrids : MonoBehaviour
{
    public static StackGrids instance;
    public List<GameObject> oldSpawnedPrefab;
    public GameObject basePrefab;
    public List<GameObject> prefabList;
    public Vector3 offset;
    public GameObject modulePrehub;
    public GameObject moduleHub;
    public bool spawnHub = false;

    // Start is called before the first frame update
    void Start()
    {
        oldSpawnedPrefab.Add(basePrefab);
    }

    // Update is called once per frame
    private void Awake()
    {
        if (instance == null)instance = this;
    }

    /*private void Update()
    {
        if (oldSpawnedPrefab.Count == 6 ||
            oldSpawnedPrefab.Count == 14 ||
            oldSpawnedPrefab.Count == 24)
        {
            spawnHub = true;
            
            if (spawnHub)
            {
                Instantiate(modulePrehub, transform.position + new Vector3(offset.x * 3, -3f, 0),
                    Quaternion.identity);
                /*Instantiate(moduleHub, transform.position + new Vector3(offset.x * 4, -3f, 0),
                    Quaternion.identity);
                spawnHub = false;
            }
        }
    }*/
}
