using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerAnim : MonoBehaviour
{
    public Transform unlockAllFarmer;
    public float duration;
    public LeanTweenType easeType;

    private void OnEnable()
    {
        LeanArray();
    }

    public void LeanArray()
    {
        unlockAllFarmer.GetComponent<RectTransform>().localPosition = new Vector2(0, -158);
         
        LeanTween.move(unlockAllFarmer.GetComponent<RectTransform>(), new Vector2(0,38), duration);
    }
}
