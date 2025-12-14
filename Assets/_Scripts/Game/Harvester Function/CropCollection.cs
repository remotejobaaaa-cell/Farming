using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CropCollection : MonoBehaviour
{
    public List<GameObject> crops;

    private float F1;
    private float F2;
    private float cropPercentageFloat;

    [Header("Crops Calulation")]
    [Space(1)]
    [HideInInspector] public float CropQuantity;
    [HideInInspector] public int cropPercentage = 0;

    void Start()
    {
        GetChildObject(this.transform, "Crops");
        CropQuantity = crops.Count;
    }

    public void GetChildObject(Transform parent, string _tag)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == _tag)
            {
                crops.Add(child.gameObject);
            }
            if (child.childCount > 0)
            {
                GetChildObject(child, _tag);
            }
        }
    }
    public void CropPercentage()
    {
        if (cropPercentage > -1 && cropPercentage <= 100)
        {
            F1 = CropQuantity;
            F2 = crops.Count;
            cropPercentageFloat = 100 - ((F1 / F2) * 100);
            cropPercentage = Convert.ToInt32(cropPercentageFloat);
            if (HudMenuManager.instance)
            {
                HudMenuManager.instance.gameplayMenu.infoPopup.gameObject.SetActive(true);
                HudMenuManager.instance.gameplayMenu.inforPopupText.text = "Crops: " + cropPercentage.ToString() + "%";
            }
            if (cropPercentage == 100)
            {
                if (HudMenuManager.instance.gameplayMenu.inforPopupText)
                {
                    HudMenuManager.instance.gameplayMenu.inforPopupText.text = "Crops: 100% Full";
                }
                foreach (GameObject obj in crops)
                {
                    obj.gameObject.GetComponent<CapsuleCollider>().enabled = false;
                }

                if (HudMenuManager.instance.playerSpanwer.rccCarControllerV3)

                {
                    HudMenuManager.instance.playerSpanwer.rccCarControllerV3.KillEngine();
                }
                GameConstants.isPlayerWin = true;
                HudMenuManager.instance.GameOver(3);

                return;
            }
            else if (cropPercentage > 99)
            {
                HudMenuManager.instance.gameplayMenu.inforPopupText.text = "Crops: 100% Full";
                return;
            }
        }
    }
}
