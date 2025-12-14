using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GleyDailyRewards;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;
using DG.Tweening;

public class MainMenuManager : MonoBehaviour
{
    //[HideInInspector]
    public PlayerData playerData;

    [Header("Menu")]
    [Space(2)]
    public TopBarMenuManager topBarMenu;
    public StartMenuManager startMenu;
    public FarmerSelectionMenuManager ownVehicleMenu;
    public LevelSelectionMenuManager levelSelectionMenuManager;
    public GameObject dailyRewardPanel;

    public FarmersStore garageVehicles;
    public GameObject mobileDrag;

    public Text declineText;
    public GameObject giveRewardPanel;


    [Header("Popups")]
    [Space(2)]
    public ExitMenuPopup exitPopup;
    public SettingsPopup settingsPop;
    public DailyRewardPopup dailyRewardPopup;
    public GiftPopup giftPopup;
    public LuckySpinPopup luckSpinPopup;
    public FreeCashSocialPopup freeCashSocialPopup;
    public ShareRewardPopup shareRewardPopup;
    public StorePopup storePopup;
    public RewardCollectorPopup rewardCollectorPopup;
    public RateUsPopup rateUsPopup;

    [Header("Menus Stack")]
    [SerializeField]
    private List<GameObject> menusList;
    [SerializeField]
    private List<GameObject> popupList;
    public MenuNames currentMenuName;
    public MenuNames previousMenuName;
    public PopupNames currentPopup;
    public PopupNames previousPopup;
    public List<MenuNames> menusStack;
    private bool isPopupVisible;
    public PurchasingPanel purchaingDia;
    [Header("Confetti ")]
    [SerializeField]
    public GameObject confettiObject;

    [Header("Loading")]
    [Space(2)]
    public GameObject loadingMenu;
    public Slider loadingSlider;
    private AsyncOperation ao;
    public Text loadingProgressText;
   


    public IEnumerator ConfettiOnOffObject()
    {
        confettiObject.SetActive(true);
        yield return new WaitForSeconds(6);
        confettiObject.SetActive(false);
    }

    private void Awake()
    {
        playerData = PlayerDataController.instance.playerData;
    }

    private void OnEnable()
    {
        Initilization();
        ToggleMenu();
#if HAVE_ADS
        if (AdsManager.instance)
        AdsManager.instance.LogEvent("Main_Menu");
#endif

    }
    private void Start()
    {
#if HAVE_ADS
        if (AdsManager.instance)
        {
            AdsManager.instance.HideAllBanners();
            AdsManager.instance.ShowBannerAdLeft();
            AdsManager.instance.ShowBannerAdRight();
        }
#endif
    }
    private void Initilization()
    {
        topBarMenu.Initilize();
        InitilizeDailyReward();
        InitializeGameConstants();
    }

    private void InitializeGameConstants()
    {
        GameConstants.currentlySelectedFarmer = playerData.currentlySelectedFarmer;
        GameConstants.currentlySelectedMap = playerData.currentlySelectedMap;
    }

    private void InitilizeDailyReward()
    {
        //CalendarManager.Instance.ResetCalendar();
        CalendarManager.Instance.Initialize();
        CalendarManager.Instance.LoadPopup(dailyRewardPopup.calendarPopup.gameObject, CalendarManager.Instance.dailyRewardsSettings.allDays);
        if (dailyRewardPopup.calendarPopup.IsAnyRewardRewardAvailable())
        {
            //Invoke("DelayInDailyReward", 1.5f);
        }
    }
    public void PurchaseNoAdsBtn()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
#if HAVE_IAP
        if (PurchaserManager.instance != null)
        {
              PurchaserManager.instance.PurchaseNoAds();
        }
#endif

        //purchaingDia.InitProduct("Store", "Are you want to remove ads from your game ?", PurchingProduct.WhatProduct.NoAds);
    }

    public void PurchaseUnlockAllGameBtn()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
#if HAVE_IAP
        if (PurchaserManager.instance != null)
        {
             PurchaserManager.instance.PurchasAllGame();
        }
#endif

        //purchaingDia.InitProduct("Store", "Are you want to purchase all game features?", PurchingProduct.WhatProduct.unlockallgame);
    }
    public void PurchaseUnlockAllCharacterBtn()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
#if HAVE_IAP
        if (PurchaserManager.instance != null)
        {
              PurchaserManager.instance.PurchasAllFarmer();
        }
#endif
        //purchaingDia.InitProduct("Store", "Are you want to purchase all characters?", PurchingProduct.WhatProduct.characters);
    }
    public void PurchaseUnlockAllLevelBtn()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
#if HAVE_IAP
        if (PurchaserManager.instance != null)
        {
            PurchaserManager.instance.PurchaseAllLevels();
        }
#endif
        // purchaingDia.InitProduct("Store", "Are you want to purchase all game missions?", PurchingProduct.WhatProduct.Levels);
    }
    private void DelayInDailyReward()
    {
        ShowPopup(PopupNames.DAILY_REWARD);
    }
    public void ShowMediationTestSuite()
    {
        //AdsManager.instance.ShowMediationTestSuite();
    }
    private void ToggleMenu()
    {
        menusStack = new List<MenuNames>();
        ShowMenu(MenuNames.MAIN_MENU);
        if (GameConstants.openLevels)
        {
            ShowMenu(MenuNames.LEVEL_SELECTION);
        }
    }

    void Update()
    {
        if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.Escape))
        {
            OnClick_HandleBackMenu();
        }
    }

    #region Menu Transition Handeling

    public void ShowMenu(MenuNames menuName)
{
    previousMenuName = currentMenuName;
    menusList[currentMenuName.GetHashCode()].SetActive(false);
    menusList[menuName.GetHashCode()].SetActive(true);
    menusStack.Add(menuName);

    currentMenuName = menuName;

    var focusInterface = menusList[currentMenuName.GetHashCode()].GetComponent<IFocus>();
    if (focusInterface != null)
    {
        focusInterface.OnFocusMenu(true);
    }

#if HAVE_ADS
    if (currentMenuName == MenuNames.LEVEL_SELECTION)
    {
        if (AdsManager.instance)
            AdsManager.instance.ShowRectBanner();
        topBarMenu.noAdsBtn.SetActive(false);
    }
    else
    {
        topBarMenu.noAdsBtn.SetActive(true);
        if (AdsManager.instance)
            AdsManager.instance.HideRectBanner();
    }
#else
    topBarMenu.noAdsBtn.SetActive(true); // Default behavior if ads are disabled
#endif
}

    public void ShowPopup(PopupNames popupName)
    {
        previousPopup = currentPopup;
        for (int i = 0; i < popupList.Count; i++)
        {
            popupList[i].SetActive(false);
        }
        isPopupVisible = true;
        currentPopup = popupName;

        popupList[popupName.GetHashCode()].SetActive(true);

        var focusInterface = menusList[currentMenuName.GetHashCode()].GetComponent<IFocus>();
        if (focusInterface != null)
        {
            focusInterface.OnFocusMenu(false);
        }
      
    }

    public void OnClick_HandleBackMenu()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }

        if (isPopupVisible)
        {
            ClosePopup();
            return;
        }

        if (menusStack.Count >= 2)
        {
            MenuNames toShow = menusStack[menusStack.Count - 2];
            MenuNames toRemove = menusStack[menusStack.Count - 1];
            menusStack.Remove(toRemove);
            currentMenuName = toShow;
            previousMenuName = currentMenuName;

            menusList[toRemove.GetHashCode()].SetActive(false);
            menusList[toShow.GetHashCode()].SetActive(true);
        }
        else
        {
            exitPopup.gameObject.SetActive(true);
        }

#if HAVE_ADS
        if (currentMenuName == MenuNames.LEVEL_SELECTION)
        {
            if (AdsManager.instance)
                AdsManager.instance.ShowRectBanner();
            topBarMenu.noAdsBtn.SetActive(false);
        }
        else
        {
            topBarMenu.noAdsBtn.SetActive(true);
            if (AdsManager.instance)
                AdsManager.instance.HideRectBanner();
        }
#else
    topBarMenu.noAdsBtn.SetActive(true); // Ensure button state is handled regardless of ads.
#endif
    }


    public void ClosePopup()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
        isPopupVisible = false;
        popupList[currentPopup.GetHashCode()].SetActive(false);

        var focusInterface = menusList[currentMenuName.GetHashCode()].GetComponent<IFocus>();
        if (focusInterface != null)
        {
            focusInterface.OnFocusMenu(true);
        }
        if(currentPopup == PopupNames.REWARD_COLLECTOR)
        {
            topBarMenu.GiveRewad(500);
          //  StartCoroutine(topBarMenu.CoinAttractionParticleOnOffPanel(500));
        }
    }

    public void CloseAndOpenPopup(PopupNames popup)
    {
        if (isPopupVisible)
        {
            ClosePopup();
            ShowPopup(popup);
        }
    }

    public void CloseAndOpenPreviousPopup()
    {
        if (isPopupVisible)
        {
            ClosePopup();
            ShowPopup(previousPopup);
        }
    }
    public void RateUsLike()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.fgs.modern.farming.simulator");
    }
    #endregion

    public void LoadLevel(string value)
    {
#if HAVE_ADS
        if (AdsManager.instance)
            AdsManager.instance.ShowRectBanner();
#endif

        StartCoroutine(Loading());
    }
    public float totalFakeLoadTime; // 20 seconds fake load time

  public  IEnumerator Loading()
    {
        if (currentMenuName == MenuNames.LEVEL_SELECTION)
        {
            totalFakeLoadTime = 5;
        }

            loadingSlider.value = 0f;
        loadingProgressText.text = "";
        loadingMenu.SetActive(true);

        float fakeProgress = 0f;

        while (fakeProgress < totalFakeLoadTime)
        {
            fakeProgress += Time.deltaTime;
            float progress = Mathf.Clamp01(fakeProgress / totalFakeLoadTime);
            loadingSlider.value = progress;
            loadingProgressText.text = " " + (int)(progress * 100) + "%";
            yield return null;
        }
        if (currentMenuName == MenuNames.MAIN_MENU)
        {
            loadingMenu.SetActive(false);
            ShowMenu(MenuNames.FARMER_SELECTION_MENU);
        }
        else if (currentMenuName == MenuNames.FARMER_SELECTION_MENU)
        {
            loadingMenu.SetActive(false);
            ShowMenu(MenuNames.MAP_SELECTION);
        }
        else if (currentMenuName == MenuNames.MAP_SELECTION)
        {
            loadingMenu.SetActive(false);
            ShowMenu(MenuNames.LEVEL_SELECTION);
        }
        else if (currentMenuName == MenuNames.DAILY_REWARD)
        {
            loadingMenu.SetActive(false);
            OnClick_HandleBackMenu();
            //ShowMenu(MenuNames.MAIN_MENU);
        }
        else if (currentMenuName == MenuNames.LEVEL_SELECTION)
        {
            totalFakeLoadTime = 5;
           SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        // StartCoroutine(LoadTheLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }
    public void NoRewardedAdAvailable()
    {
        declineText.text = "No Video Ad Available, Try Again Later.";
        declineText.gameObject.SetActive(true);
        declineText.GetComponent<DOTweenAnimation>().DORestart();
    }
    public void GetCoins(int cash)
    {
        playerData.playerCash = playerData.playerCash + cash;
        topBarMenu.Initilize();
        PlayerDataController.instance.Save();
    }
    public void Purchased_RemoveAds()
    {
        playerData.noAds = 3;
        topBarMenu.Initilize();
        PlayerDataController.instance.Save();
   }
    
    public void Purchased_Farmers()
    {
        playerData.unlockAllFarmers = 3;
        ownVehicleMenu.InitializeFarmer();
        PlayerDataController.instance.Save();
    } 
    
    public void Purchased_Levels()
    {
        playerData.unlockAllLevels = 3;
        levelSelectionMenuManager.Initialize();
        PlayerDataController.instance.Save();
    } 
    
    public void Purchased_EveryThing()
    {
        playerData.unlockAllGame = 3;
        playerData.noAds = 3;
        topBarMenu.Initilize();

        playerData.unlockAllLevels = 3;
        levelSelectionMenuManager.Initialize();

        playerData.unlockAllFarmers = 3;
        ownVehicleMenu.InitializeFarmer();
        PlayerDataController.instance.Save();
    }
}
