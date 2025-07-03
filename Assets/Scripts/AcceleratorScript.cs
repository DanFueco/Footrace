using System;
using UnityEngine;

public class AcceleratorScript : MonoBehaviour
{
    public float speedMultiplier = 2f;
    public float boostDuration = 1.5f;


    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("Test");
        BaseMovement movement = other.GetComponent<BaseMovement>();
        if (movement != null)
        {
            movement.StartBoost(speedMultiplier, boostDuration);
        }
    }

   
}
