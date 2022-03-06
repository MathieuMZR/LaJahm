using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialSpawn : MonoBehaviour
{
    public static InitialSpawn instance;
    
    private void Awake()
        {
            if (instance == null)instance = this;
        }
}
