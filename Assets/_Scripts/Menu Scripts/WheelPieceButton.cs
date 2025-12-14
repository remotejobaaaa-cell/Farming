using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelPieceButton : MonoBehaviour
{
    public GameObject selectPiece;

    public void OnSelected(bool value)
    {
        if (value)
        {
            selectPiece.SetActive(true);
        }
        else
        {
            selectPiece.SetActive(false);
        }
    }
}
