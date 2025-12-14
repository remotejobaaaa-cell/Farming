using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropToCollect : MonoBehaviour
{
    public CropCollection cropCollection;

    private void OnDisable()
    {
        if (cropCollection)
        {
            cropCollection.CropQuantity = cropCollection.CropQuantity - 1;
            cropCollection.CropPercentage();
        }
    }

}
