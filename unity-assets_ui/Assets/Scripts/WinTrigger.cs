using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Directives above^

public class WinTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Attempt to get the Timer component from the object that triggered this event
        Timer otherTimer = other.GetComponent<Timer>();

        if (otherTimer != null) // Check if the object has a Timer component attached
        {
            // If the Timer component exists, disable it
            otherTimer.enabled = false;
        }
    }
}