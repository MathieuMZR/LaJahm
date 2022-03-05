using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorteQuiOuvre : MonoBehaviour
{
    public Animator animator;
    public TriggerQuiOuvre Trigger;

    private void Update()
    {
        if (Trigger.isTriggered == true)
        {
            animator.SetTrigger("Ouvre");
            animator.SetTrigger("BienOuvre");
        }
    }
}
