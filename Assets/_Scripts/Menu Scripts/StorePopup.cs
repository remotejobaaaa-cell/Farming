using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorePopup : MonoBehaviour
{
    public MainMenuManager mainMenuManager;

    public Image goldStoreButton;
    public Image dealsStoreButton;
    public Sprite[] storeButtonSprite;

    public GameObject goldStore;
    public GameObject dealsStore;

    [Header("IAP Buttons")]
    public NonConsumeableIAPButton[] iapButtons;

    private void OnEnable()
    {
        OnClick_ShowGoldStore();
        InitializeIapButtons();
    }

    private void InitializeIapButtons()
    {
        foreach (NonConsumeableIAPButton item in iapButtons)
        {
            item.Initialize();
        }
    }



    public void OnClick_ShowGoldStore()
    {
        goldStoreButton.sprite = storeButtonSprite[1];
        dealsStoreButton.sprite = storeButtonSprite[0];
        goldStore.SetActive(true);
        dealsStore.SetActive(false);
    }

    public void OnClick_ShowDealsStore()
    {
        goldStoreButton.sprite = storeButtonSprite[0];
        dealsStoreButton.sprite = storeButtonSprite[1];
        goldStore.SetActive(false);
        dealsStore.SetActive(true);
    }

    public void OnClick_Close()
    {
        mainMenuManager.ClosePopup();
    }
}
