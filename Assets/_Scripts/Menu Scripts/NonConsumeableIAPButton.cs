using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NonConsumeableIAPButton : MonoBehaviour
{
    public StorePopup storePopup;
    public Text titleText;
    public Text priceText;
    public Button button;
    public GameObject buyButton;
    public GameObject purchasedButton;

    public void Initialize()
    {
        //if (IAPManager.Instance.IsActive(product))
       
    }

    public void OnClick_Buy()
    {
    }
}