using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.vCharacterController;

public class DropCropController : MonoBehaviour
{
    public GameObject instantiateCrop;
    public Transform cropTransform;
    public void InstantiateCropObject()
    {
        Instantiate(instantiateCrop, cropTransform.position, cropTransform.rotation);
    }
}
