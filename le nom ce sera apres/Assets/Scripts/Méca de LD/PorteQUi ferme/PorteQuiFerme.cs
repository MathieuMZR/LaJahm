using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PorteQuiFerme : MonoBehaviour
{
    public TriggerQuiferme Trigger;
    
    public float time;
    public float hauteur;
    
    private void Update()
    {
        
        if (Trigger.isTriggered == true)
        {
            /*animator.SetTrigger("Ferme");
            animator.SetTrigger("BienFerme");*/
            
            Debug.Log(Trigger.isTriggered);
            gameObject.transform.DOMoveY(hauteur, time);
        }
    }
}

