using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightLimit : MonoBehaviour
{
    public float maxHeight;
    public float minHeight;
    public float dist;
    public Transform obj;
    void Start()
    {
        
    }

    void Update()
    {
        dist = Vector3.Distance(transform.position,obj.position);
        if (dist >= maxHeight)
        {
         HudMenuManager.instance.thirdPersonControl.droneJoystickToggle.SetActive(false);
        }
        if (dist <= minHeight)
        {

        }
    }
}
