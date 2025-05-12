using System.Collections;
using System.Collections.Generic;
using SA;
using UnityEngine;

public class FPSTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        InputHandler inp = other.GetComponentInParent<InputHandler>();
        if (inp != null)
        {
            if (inp.controller.isCrouch)
            { 
                inp.controller.isFPS = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        InputHandler inp = other.GetComponentInParent<InputHandler>();
        if (inp != null)
        {
            inp.controller.isFPS = true;   
        }
    }
}
