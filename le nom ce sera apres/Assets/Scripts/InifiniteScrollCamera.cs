using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InifiniteScrollCamera : MonoBehaviour
{
    public GameObject self;
    public GameObject player;
    public float Offset;
    public float CameraValue;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        self.transform.position = new Vector3(player.transform.position.x - Offset,self.transform.position.y,-10f);
    }
}
