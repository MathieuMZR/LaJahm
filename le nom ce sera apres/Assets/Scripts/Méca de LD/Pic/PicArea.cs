using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicArea : MonoBehaviour
{
    public GameObject Character;
    
    // Aloîs t'es vraiment un mec super

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(Character);
        // A mettre la vrai fonction permettant de butter le chara
    }
}
