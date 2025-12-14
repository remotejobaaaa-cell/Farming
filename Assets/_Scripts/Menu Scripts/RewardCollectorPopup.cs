using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardCollectorPopup : MonoBehaviour
{
    public MainMenuManager mainMenuManager;

    public void OnClick_Close()
    {
        mainMenuManager.ClosePopup();
    }

    public void CollectGold(int value)
    {

    }

    public void CollectNoAds()
    {

    }
}
