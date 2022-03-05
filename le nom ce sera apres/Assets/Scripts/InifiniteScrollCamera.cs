using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InifiniteScrollCamera : MonoBehaviour
{
    public GameObject self;
    public GameObject player;
    public float Offset;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        self.transform.position = new Vector3(player.transform.position.x - Offset,0f,-10f);
    }
}
