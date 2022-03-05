using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    public ScoreHandler sH;
    public TextMeshProUGUI tMP;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tMP.text = "Score : " + sH.score;
    }
}
