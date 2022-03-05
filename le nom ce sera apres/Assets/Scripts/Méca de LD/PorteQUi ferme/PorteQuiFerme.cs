using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorteQuiFerme : MonoBehaviour
{
    public Animator animator;
    public TriggerQuiferme Trigger;

    private void Update()
    {
        if (Trigger.isTriggered == true)
        {
            animator.SetTrigger("Ferme");
            animator.SetTrigger("BienFerme");
        }
    }
}
