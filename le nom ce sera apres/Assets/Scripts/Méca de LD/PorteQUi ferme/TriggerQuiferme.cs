using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerQuiferme : MonoBehaviour
{
    public bool isTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        isTriggered = true;
    }
}
