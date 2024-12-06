using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateBarrier : MonoBehaviour
{
    public GameObject targetObject;  // Assign the object to be activated in the inspector

    void OnTriggerEnter(Collider other)
    {
        // Check if the player has entered the trigger
        if (other.CompareTag("Player"))
        {
            // Activate the target object
            if (targetObject != null)
            {
                targetObject.SetActive(true);
                Debug.Log("Target object activated!");
            }
            else
            {
                Debug.LogWarning("Target object is not assigned in the inspector.");
            }
        }
    }
}
