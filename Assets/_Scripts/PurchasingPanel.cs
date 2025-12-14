using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchasingPanel : MonoBehaviour
{
	public Text topText, descriptionText;
	PurchingProduct.WhatProduct theProduct;

	public void InitProduct (string title, string description, PurchingProduct.WhatProduct product)
	{
		gameObject.SetActive (true);
		topText.text = title;
		descriptionText.text = description;
		theProduct = product;
	}

	public void YesPurchase ()
	{
#if HAVE_IAP
        if (theProduct == PurchingProduct.WhatProduct.NoAds)
        {
            PurchaserManager.instance.PurchaseNoAds();
        }
        else if (theProduct == PurchingProduct.WhatProduct.Levels)
        {
            Debug.Log("Levels");
            PurchaserManager.instance.PurchaseAllLevels();
        }
        else if (theProduct == PurchingProduct.WhatProduct.characters)
        {
            Debug.Log("characters");
            PurchaserManager.instance.PurchasAllFarmer();
        }
        else if (theProduct == PurchingProduct.WhatProduct.unlockallgame)
        {
            Debug.Log("unlockallgame");
            PurchaserManager.instance.PurchasAllGame();
        }
#else
    Debug.LogWarning("IAP functionality is disabled. Enable HAVE_IAP in Scripting Define Symbols.");
#endif
    }
}
