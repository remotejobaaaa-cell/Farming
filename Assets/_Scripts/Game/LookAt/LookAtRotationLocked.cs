using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtRotationLocked : MonoBehaviour
{
    Transform _transf;
    public Transform lookAtTarget;
    public bool isLockRotation;

    public void OnEnable()
    {
        _transf = transform;
    }

    void Start()
    {
        _transf = transform;
    }

    void LateUpdate()
    {
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

}
