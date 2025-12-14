using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using DG.Tweening;
using System;

public class StartMenuManager : MonoBehaviour
{
    [SerializeField]
    private MainMenuManager mainMenuMenu;
    [SerializeField]
    private GameObject luckySpinAvaiableAnimation;

    private void OnEnable()
    {
        mainMenuMenu.garageVehicles.ChangeCameraFocusPoint(CameraFocus.StartMenu);
        InitilizeLuckySpin();
    }

    private void InitilizeGift()
    {
        if (mainMenuMenu.playerData.giftCounter > 0)
        {
            Debug.Log("Play Gift Animation Here");
        }
    }

    private void InitilizeLuckySpin()
    {
        if (mainMenuMenu.playerData.luckySpinCounter > 0)
        {
            luckySpinAvaiableAnimation.transform.DOLocalRotate(Vector3.forward * 200, 1.0f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        }
        else
        {
            DOTween.Pause(luckySpinAvaiableAnimation);
        }
    }

    private void InitializeSelectedVehicle()
    {
        mainMenuMenu.garageVehicles.ShowFarmer();
    }

    public void OnClick_FarmerSelection()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
        StartCoroutine(mainMenuMenu.Loading());
#if HAVE_ADS
        if (AdsManager.instance)
            AdsManager.instance.LogAnalyticsEvent("mainmenu_to_formselection");
#endif
        //  mainMenuMenu.ShowMenu(MenuNames.FARMER_SELECTION_MENU);
    }

    public void OnClick_DailyRewards()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
       // StartCoroutine(mainMenuMenu.Loading());
      
        mainMenuMenu.ShowMenu(MenuNames.DAILY_REWARD);
    }

    public void OnClick_MapSelection()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
        mainMenuMenu.ShowMenu(MenuNames.MAP_SELECTION);
    }

    public void OnClick_CrossPromotion()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
        CrossPromo.Instance.ShowCrossPromoPopup(CrossPromotionCallBack);
    }

    private void CrossPromotionCallBack(bool clicked, string error)
    {
        if (clicked)
        {
            Debug.Log("Custom Ads Clicked");
        }
        else
        {
            Debug.Log("Not Clicked " + error);
        }
    }

    public void OnClick_StorePopup()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
        mainMenuMenu.ShowPopup(PopupNames.STORE);
    }
    public void WatchAd_GetReward()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }

#if HAVE_ADS
        if (AdsManager.instance && AdsManager.instance.isAdmobRewardedVideoAvailable())
        {
            AdsManager.instance.ShowRewardedAD(GiveCashReward);
        }
        else
        {
            mainMenuMenu.NoRewardedAdAvailable();
        }
#else
    mainMenuMenu.NoRewardedAdAvailable();
#endif
    }

    public void GiveCashReward()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickConfettiSound();
            AudioManager.instance.Play_ClickInGameRewardSound();
        }
        mainMenuMenu.ShowPopup(PopupNames.REWARD_COLLECTOR);
        if(mainMenuMenu)
        StartCoroutine(mainMenuMenu.ConfettiOnOffObject()); 
       // mainMenuMenu. playerData.playerCash = mainMenuMenu. playerData.playerCash + 500;
      // mainMenuMenu. topBarMenu.Initilize();
     //  PlayerDataController.instance.Save();
    }
    public void OnClick_DailyReward()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
        mainMenuMenu.ShowPopup(PopupNames.DAILY_REWARD);
    }

    public void OnClick_Gift()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
        mainMenuMenu.ShowPopup(PopupNames.GIFT);
    }

    public void OnClick_Share()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
        mainMenuMenu.ShowPopup(PopupNames.SHARE_REWARD);
    }

    public void OnClick_LuckySpin()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
        mainMenuMenu.ShowPopup(PopupNames.LUCKY_SPIN_WHEEL);
    }

    public void OnClick_FreeCash_Social()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
        mainMenuMenu.ShowPopup(PopupNames.FREECASH_SOCIAL);
    }
    public void OnClick_RateUS()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
        mainMenuMenu.ShowPopup(PopupNames.RATE_US);
    }
}
