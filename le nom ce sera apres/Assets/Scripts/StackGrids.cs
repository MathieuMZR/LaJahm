using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackGrids : MonoBehaviour
{
    public static StackGrids instance;
    public List<GameObject> oldSpawnedPrefab;
    public GameObject basePrefab;

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
