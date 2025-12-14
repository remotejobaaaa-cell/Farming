using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FarmerSelectionMenuManager : MonoBehaviour
{
    public MainMenuManager mainMenuManager;

    public GameObject selectButton;
    public GameObject selectedButton;
    public GameObject unlockButton;
    public GameObject unlockButton_Reward;
    public GameObject unlockAllFarmersBtn;
    public Text unlockPriceText;
    public Text currentFarmerName;
    public Text declineText;
    public GameObject adLogo;
    public GameObject confetti;

    private void OnEnable()
    {
        mainMenuManager.garageVehicles.ChangeCameraFocusPoint(CameraFocus.FarmerSelection);
        InitializeFarmer();
#if HAVE_ADS
        if (adLogo)
            adLogo.SetActive(AdsManager.instance && AdsManager.instance.IsIntertitialAvaliable());
#else
    if (adLogo)
        adLogo.SetActive(false);  // Hide adLogo if ads are disabled.
#endif
    }

    public void OnClick_NextFarmer(int index)
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
        mainMenuManager.garageVehicles.ShowFarmer(index);
        declineText.gameObject.SetActive(false);
        InitializeFarmer();
    }
     public void OnClick_NextFarmer()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
        mainMenuManager.garageVehicles.NextFarmer();
        declineText.gameObject.SetActive(false);
        InitializeFarmer();
    }
   
    public void OnClick_PreviousFarmer()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
        mainMenuManager.garageVehicles.PreviousFarmer();
        declineText.gameObject.SetActive(false);
        InitializeFarmer();
    }

    public void OnClick_UnlockWithCash()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
        if (mainMenuManager.playerData.playerCash >= mainMenuManager.playerData.farmers[mainMenuManager.garageVehicles.currentIndex].unlockPrice)
        {
            mainMenuManager.playerData.playerCash -= mainMenuManager.playerData.farmers[mainMenuManager.garageVehicles.currentIndex].unlockPrice;
            mainMenuManager.playerData.farmers[mainMenuManager.garageVehicles.currentIndex].isLocked = false;
            mainMenuManager.topBarMenu.Initilize();
            InitializeFarmer();
            PlayerDataController.instance.Save();
        }
        else
        {
            //mainMenuManager.ShowPopup(PopupNames.STORE);
            declineText.text = "Not enough coins";
            declineText.gameObject.SetActive(true);
            declineText.GetComponent<DOTweenAnimation>().DORestart();
        }
    }
    public void IAPInitialize()
    {
        if (mainMenuManager.playerData.unlockAllFarmers == 3)
        {
            unlockAllFarmersBtn.gameObject.SetActive(false);
            for (int i = 0; i < mainMenuManager.playerData.farmers.Count; i++)
            {
                mainMenuManager.playerData.farmers[i].isLocked = false;
            }
            PlayerDataController.instance.Save();
        }
        else
        {
            unlockAllFarmersBtn.gameObject.SetActive(true);
        }
    }
    public void Method()
    {
    
        StartCoroutine(mainMenuManager.Loading());
#if HAVE_ADS
        if (AdsManager.instance)
            AdsManager.instance.LogAnalyticsEvent("formselection_to_mapselection");
#endif
    }
    public void OnClick_SelectFarmer()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
        GameConstants.currentlySelectedFarmer = mainMenuManager.garageVehicles.currentIndex;
        mainMenuManager.playerData.currentlySelectedFarmer = mainMenuManager.garageVehicles.currentIndex;
            PlayerDataController.instance.Save();
//#if HAVE_ADS
//        if (AdsManager.instance && AdsManager.instance.IsIntertitialAvaliable() && !AdsManager.instance.isRemoveAd())
//            AdsManager.instance.ShowInterstitialAd(Method);
//        else
//#endif
            Method();

        // mainMenuManager.ShowMenu(MenuNames.MAP_SELECTION);
    }

    public void InitializeFarmer()
    {
        IAPInitialize();
        if (mainMenuManager.playerData.farmers[mainMenuManager.garageVehicles.currentIndex].isLocked)
        {
            selectButton.SetActive(false);
            selectedButton.SetActive(false);
            unlockButton.SetActive(true);
            unlockButton_Reward.SetActive(true);
            unlockPriceText.text = mainMenuManager.playerData.farmers[mainMenuManager.garageVehicles.currentIndex].unlockPrice.ToString();
        }
        else
        {
            
            unlockButton.SetActive(false);
            unlockButton_Reward.SetActive(false);
            selectButton.SetActive(true);
            //if (GameConstants.currentlySelectedFarmer == mainMenuManager.garageVehicles.currentIndex)
            //{
            //    selectButton.SetActive(false);
            //    selectedButton.SetActive(true);
            //}
            //else
            //{
            //    selectButton.SetActive(true);
            //    selectedButton.SetActive(false);
            //}
            
        }
        mainMenuManager.garageVehicles.ShowFarmer();
    }
    public void UnlockFarmerByWatchingReward()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
#if HAVE_ADS
        if (AdsManager.instance && AdsManager.instance.isAdmobRewardedVideoAvailable())
        {
            AdsManager.instance.ShowRewardedAD(FarmerUnlockReward);
        }
        else
#endif
        {
            declineText.text = "No Video Ad Available, Try Again Later.";
            declineText.gameObject.SetActive(true);
            declineText.GetComponent<DOTweenAnimation>().DORestart();
        }
    }
    public void FarmerUnlockReward()
    {
        mainMenuManager.playerData.farmers[mainMenuManager.garageVehicles.currentIndex].isLocked = false;
        mainMenuManager.topBarMenu.Initilize();
        InitializeFarmer();
        PlayerDataController.instance.Save();
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickInGameRewardSound();
            AudioManager.instance.Play_ClickConfettiSound();
        }
        StartCoroutine(mainMenuManager.ConfettiOnOffObject());
    }
   
}