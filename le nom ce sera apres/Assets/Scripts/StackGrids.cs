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

    private void Update()
    {
        if (oldSpawnedPrefab.Count == 4 ||
            oldSpawnedPrefab.Count == 8 ||
            oldSpawnedPrefab.Count == 12)
        {
            Instantiate(StackGrids.instance.modulePrehub, transform.position + StackGrids.instance.offset,
                Quaternion.identity);
            Instantiate(StackGrids.instance.moduleHub, transform.position + StackGrids.instance.offset * 2,
                Quaternion.identity);
            
            Debug.Log("AHAH");
        }
    }
}
