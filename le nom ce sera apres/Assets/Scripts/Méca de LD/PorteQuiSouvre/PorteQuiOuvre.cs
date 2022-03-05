using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorteQuiOuvre : MonoBehaviour
{
    public Animator animator;
    public TriggerQuiferme Trigger;

    private void Update()
    {
        if (Trigger.isTriggered == true)
        {
            
        }
    }
}
