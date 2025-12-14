using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SimpleLookAt : MonoBehaviour 
{
    Transform _transf;
    public Transform lookAtTarget;
    public bool isLockRotation;

    public void OnEnable()
    {
        //getRccCamera();
        _transf = transform;
    }

    void Start()
    {
        _transf = transform;
    }

    void LateUpdate()
    {
        if (lookAtTarget == null)
        {
            if (FindObjectOfType<RCC_Camera>())
            {
                lookAtTarget = FindObjectOfType<RCC_Camera>().transform;
            }
        }
        if (isLockRotation)
        {
            LockRotations();
        }
        else
        {
            _transf.LookAt(lookAtTarget);
        }
    }

    void LockRotations()
    {
        Vector3 targetPostition = new Vector3(lookAtTarget.position.x,
                                       this.transform.position.y,
                                       lookAtTarget.position.z);
        this.transform.LookAt(targetPostition);
    }

    //public void getRccCamera()
    //{
    //    if (FindObjectOfType<RCC_Camera>())
    //    {
    //        lookAtTarget = FindObjectOfType<RCC_Camera>().transform;
    //    }
    //}

    //public void getRccCamera(float delayGetRccCamera)
    //{
    //    Invoke("getRccCamera", delayGetRccCamera);
    //}

}
