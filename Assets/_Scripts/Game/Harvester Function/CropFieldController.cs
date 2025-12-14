using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CropFieldController : MonoBehaviour
{
    public GameObject harvesterCutter;
    public void HarvesterCutterOn(bool harvesterState)
    {
        if (harvesterState)
        {
            harvesterCutter.GetComponent<BoxCollider>().enabled = harvesterState;
            harvesterCutter.GetComponent<DOTweenAnimation>().DOPlay();
        }
        else
        {
            harvesterCutter.GetComponent<BoxCollider>().enabled = harvesterState;
            harvesterCutter.GetComponent<DOTweenAnimation>().DOPause();
        }

    }
}
