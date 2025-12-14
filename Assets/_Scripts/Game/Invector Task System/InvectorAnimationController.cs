using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.vCharacterController;

public class InvectorAnimationController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<vThirdPersonController>())
        {
            HudMenuManager.instance.ShowThirdPersonControl(false);
        }   
    }
}
