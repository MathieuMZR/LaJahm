using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackGrids : MonoBehaviour
{
    public static StackGrids instance;
    public List<GameObject> oldSpawnedPrefab;
    public GameObject basePrefab;
    public List<GameObject> prefabList;
    public Vector3 offset;

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
}
