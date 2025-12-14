using UnityEngine;
using GoogleMobileAds.Api;
using Firebase.Analytics;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using System;
using System.Collections;
using GoogleMobileAds.Common;
using UnityEditor;


public enum MenuType
{
    Avtar,
    Country,
    Chrachter,
    Weapon,
    Garage

}

public class AdsManager : MonoBehaviour
{

    public static AdsManager instance;
    public PlayerDataController playerDataController;
    public AdsType adsType;
    public bool withoutAds;
    [Header("UnCheck This Before Taking Build In IOS")]
    public bool showAllAds = false; // Managed by Remote Config
    private bool showAppOpen = false; // Managed by Remote Config
    public bool showAdaptiveBanner = false; // Managed by Remote Config

    [Header("Check This Before Taking Build In IOS")]
    [Space(5f)]
    [SerializeField]
    private bool UseRemoteBoolAds;
    [ShowIfTrue("UseRemoteBoolAds")]
    public string remoteStringAppOpen;
    public string remoteStringAllAdsName;
    public string remoteStringAdaptiveBanner;

    [Space(5f)]
    [SerializeField]
    private bool UseAppOpenAd;
    [ShowIfTrue("UseAppOpenAd")]
    public string AppOpenAdID;

    [Space(5f)]
    public string InterstitialAdID;
    [Space(5f)]

    //  [ShowIfTrue("Use2ndInstAD")]
    //  public string InterstitialAdID2;
    public string BannerAdIDLeft;
    [Space(5f)]
    public string BannerAdIDRight;
    [Space(5f)]
    public string RectBannerAdID;
    [Space(5f)]
    public string RewardedVideoAdID;

    //   public string RewardedInsterstitialAdID;

    private InterstitialAd interstitialAd1;

    // private InterstitialAd interstitialAd2;
    private BannerView smartBannerTopLeft;
    private BannerView smartBannerTopRight;
    private BannerView rectBannerBottomLeft;
    private RewardedAd rewardedVideoAd;
    private BannerView AdaptiveBannerTop;

    [Space(5f)]
    public string AdaptiveBannerAdID;
    // private RewardedInterstitialAd rewardedInterstitialAd;
    private AppOpenAd appOpenAd;
    [Space(5f)]
    public AdPosition AdaptiveBannerPos;
    bool isLowEndDevice = false;
    public event Action OnAdCompleted;
    private bool showRewardedVideoNext = true; // Toggle between rewarded video and interstitial

    [Space(5f)]
    public GameObject adloadingImage;
    bool isFirebaseEnable;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        if (!PlayerPrefs.HasKey("firstopen"))
        {
            PlayerPrefs.SetInt("firstopen", 0);
        }

        if (adsType == AdsType.TestAds)
        {
            AppOpenAdID = "ca-app-pub-3940256099942544/9257395921";
            BannerAdIDRight = "ca-app-pub-3940256099942544/6300978111";
            BannerAdIDLeft = "ca-app-pub-3940256099942544/6300978111";
            RectBannerAdID = "ca-app-pub-3940256099942544/6300978111";
            InterstitialAdID = "ca-app-pub-3940256099942544/1033173712";
            //    InterstitialAdID2 = "ca-app-pub-3940256099942544/1033173712";
            RewardedVideoAdID = "ca-app-pub-3940256099942544/5224354917";
            //  RewardedInsterstitialAdID = "ca-app-pub-3940256099942544/5354046379";
            //showAllAds = true;
        }
        InitializeFirebase();

#if UNITY_ANDROID

        // InitializeMobileAds();


#endif


        if (SystemInfo.systemMemorySize < 1100)
        {
            isLowEndDevice = true;
        }
    }


    private void InitializeFirebase()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            try
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    if (task.IsCompleted && !task.IsFaulted)
                    {
                        Debug.Log("Firebase Initialized Successfully.");
                        Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;
                        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                        isFirebaseEnable = true;
                        /*  if (Application.platform == RuntimePlatform.Android)
                          {*/
                        //#if UNITY_IOS
                        FetchRemoteConfig();
                        Debug.Log("RemoteConfig.");
                        //#endif

                        // }
                    }
                }
                else
                {
                    throw new Exception("Firebase initialization failed: " + task.Exception);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        });
    }
    #region RemoteConfig
    //#if UNITY_IOS
    public bool isAdlimit=false;
    private void FetchRemoteConfig()
    {

        FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero).ContinueWithOnMainThread(task =>
        {
            try
            {
                if (task.IsCompleted)
                {
                    FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWithOnMainThread(_ =>
                    {

                        Debug.Log("Remote Config showAppOpen: " + showAppOpen);
                        if (adsType != AdsType.TestAds)
                        {
                            showAppOpen = FirebaseRemoteConfig.DefaultInstance.GetValue(remoteStringAppOpen).BooleanValue;
                            showAllAds = FirebaseRemoteConfig.DefaultInstance.GetValue(remoteStringAllAdsName).BooleanValue;
                            showAdaptiveBanner = FirebaseRemoteConfig.DefaultInstance.GetValue(remoteStringAdaptiveBanner).BooleanValue;
                        }
                        InitializeMobileAds();


                    });
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Remote Config Fetch failed: " + e.Message);
            }
        });

    }
    //#endif
    #endregion
    public void InitializeMobileAds()
    {
        MobileAds.Initialize(initStatus => { });
        RequestAllAds();
    }
    private void RequestAllAds()
    {
        playerDataController = PlayerDataController.instance;
     
        if (showAllAds) // Request ads only if showAllAds is true
        {
            Invoke(nameof(RequestAppOpenAd), 0.5f);
            if (showAdaptiveBanner)
            {
                Invoke(nameof(RequestAdaptiveBannerAd), 2f);
            }
            else
            {
                Invoke(nameof(RequestBannerAds), 2f);
            }

            Invoke(nameof(RequestInterstitialAds), 3f);
        }
        RequestRewardedAds(); // Rewarded ads should always be requested
    }
    public void _SendEvent(string _event)
    {
        if (isFirebaseEnable)
        {
            FirebaseAnalytics.LogEvent(_event);
        }
    }
    #region Interstitial Ads
    private void RequestInterstitialAds()
    {
        if (isRemoveAd())
        {
            return;
        }
        // Interstitial 1
        InterstitialAd.Load(InterstitialAdID, new AdRequest(),
        (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                Debug.LogError("Interstitial 1 failed to load: " + error.GetMessage());
                //RequestSecondInterstitialAd(); // Try to load Interstitial 2
                return;
            }
            interstitialAd1 = ad;
            //   interstitialAd1.OnAdFullScreenContentClosed += HandleInterstitialClosed;
            Debug.Log("Interstitial 1 Loaded.");
            //Register to ad events to extend functionality.
            RegisterEventHandlers();
        });

        // Interstitial 2 (Only loaded if the first fails)
        //  RequestSecondInterstitialAd();
    }


    public bool IsIntertitialAvaliable()
    {
        if (interstitialAd1 != null && interstitialAd1.CanShowAd())
        {
            return true;
        }
        else
        {
            RequestInterstitialAds();
            return false;
        }
    }
    bool showInt = true;
    //IEnumerator IntTimer()
    //{
    //    //yield return new WaitForSeconds(interstitialTimer);
    //    //showInt = true;
    //}
    private Action interCallBack;
    public void ShowInterstitialAd(Action callBack = null)
    {
        interCallBack = callBack;
        //if (showIntTimer)
        //{
        //    if (!showInt)
        //    {
        //        interCallBack?.Invoke();
        //        StartCoroutine(IntTimer());
        //        Debug.Log("NoShowIntHere");
        //        return;
        //    }
        //}
        if (isRemoveAd())
        {
            return;
        }
        if (showAllAds == false)
        {
            if (interCallBack != null)
            {
                interCallBack.Invoke();
            }
            return;
        }
        if (adloadingImage)
        {
            adloadingImage.SetActive(true);
        }
        if (IsIntertitialAvaliable())
        {
            StartCoroutine(showInstAd());
        }
        else
        {
            Debug.LogWarning("No interstitial ads available.");
            if (adloadingImage)
            {
                interCallBack?.Invoke();
                adloadingImage.SetActive(false);
            }
        }
        Debug.Log("ShowIntHere");
        //if (showIntTimer)
        //{
        //    showInt = false;
        //}
    }

    private void RegisterEventHandlers()
    {

        // Raised when an impression is recorded for an ad.
        interstitialAd1.OnAdImpressionRecorded += () =>
        {
            LogEvent("Interstitial_1_Impression_Recorded");
            //Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        interstitialAd1.OnAdClicked += () =>
        {
            LogEvent("Interstitial_1_Ad_Clicked");
            //showLog("Interstitial ad was clicked.");
        };

        // Raised when the ad closed full screen content.
        interstitialAd1.OnAdFullScreenContentClosed += () =>
        {
            ShowAppOpenRemotely();

            interCallBack?.Invoke();

            adloadingImage.SetActive(false);

            MobileAdsEventExecutor.ExecuteInUpdate(() => { RequestInterstitialAds(); });
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd1.OnAdFullScreenContentFailed += (AdError error) =>
        {

            adloadingImage.SetActive(false);
            interCallBack?.Invoke();

            MobileAdsEventExecutor.ExecuteInUpdate(() => { RequestInterstitialAds(); });

            // showLog("Interstitial ad failed to open full screen content with error : "
            // + error);
        };
    }


    IEnumerator showInstAd()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        interstitialAd1.Show();
    }

    #endregion
    public bool isRemoveAd()
    {
        if (playerDataController == null)
            return false;

        if (playerDataController!=null && playerDataController.playerData.noAds == 3)
            return true;
        else
            return false;

    }
    #region Banner Ads
    private void RequestBannerAds()
    {
        if (isRemoveAd())
        {
            return;
        }
        if (showAllAds)
        {
            // Smart banner top left
            smartBannerTopLeft = new BannerView(BannerAdIDLeft, AdSize.Banner, AdPosition.TopLeft);
            smartBannerTopLeft.LoadAd(new AdRequest());

            // Smart banner top right
            smartBannerTopRight = new BannerView(BannerAdIDRight, AdSize.Banner, AdPosition.TopRight);
            smartBannerTopRight.LoadAd(new AdRequest());

            // If we already have a banner, destroy the old one.
            if (rectBannerBottomLeft != null)
            {
                rectBannerBottomLeft.Destroy();
                rectBannerBottomLeft = null;
            }
            // Rect banner bottom left
            rectBannerBottomLeft = new BannerView(RectBannerAdID, AdSize.MediumRectangle, AdPosition.BottomLeft);
            rectBannerBottomLeft.LoadAd(new AdRequest());
            rectBannerBottomLeft.Hide();
        }
    }
    public void HideAllBanners()
    {

        smartBannerTopLeft?.Hide();
        smartBannerTopRight?.Hide();
        rectBannerBottomLeft?.Hide();
    }
    public void HideSmartBanners()
    {
        smartBannerTopLeft?.Hide();
        smartBannerTopRight?.Hide();
    }
    public void ShowSmartBanners()
    {
        smartBannerTopLeft?.Show();
        smartBannerTopRight?.Show();
    }

    #region  TopBannerRighttWorking
    public bool isBannerAdRight()
    {
        if (smartBannerTopRight != null)
        {
            return true;
        }
        else
        {
            RequestForTopRightBanner();
            return false;
        }

    }

    public void RequestForTopRightBanner()
    {
        // Smart banner top right
        smartBannerTopRight = new BannerView(BannerAdIDRight, AdSize.Banner, AdPosition.TopRight);
        smartBannerTopRight.LoadAd(new AdRequest());
    }

    public void ShowBannerAdRight()
    {
        if (isRemoveAd())
        {
            return;
        }
        if (showAllAds == false)
        {
            return;
        }
        if (isBannerAdRight())
        {
            smartBannerTopRight?.Show();
        }
    }
    #endregion

    #region TopBannerLeftWorking
    public bool isBannerAdLeft()
    {
        if (smartBannerTopLeft != null)
        {
            return true;
        }
        else
        {
            RequestForTopLeftBanner();
            return false;
        }
    }
    public void RequestForTopLeftBanner()
    {
        // Smart banner top left
        smartBannerTopLeft = new BannerView(BannerAdIDLeft, AdSize.Banner, AdPosition.TopLeft);
        smartBannerTopLeft.LoadAd(new AdRequest());
    }

    public void ShowBannerAdLeft()
    {
        if (isRemoveAd())
        {
            return;
        }
        if (showAllAds == false)
        {
            return;
        }
        if (isBannerAdLeft())
        {
            smartBannerTopLeft?.Show();
        }
    }
    #endregion

    #region RectBannerWorking
    public bool isRectbanner()
    {

        if (rectBannerBottomLeft != null)
        {
            return true;
        }
        else
        {
            RequestForRectBanner();
            return false;
        }
    }
    public void RequestForRectBanner()
    {
        if (isRemoveAd())
            return;
            // If we already have a banner, destroy the old one.
            if (rectBannerBottomLeft != null)
        {
            rectBannerBottomLeft.Destroy();
            rectBannerBottomLeft = null;
        }
        // Rect banner bottom left
        rectBannerBottomLeft = new BannerView(RectBannerAdID, AdSize.MediumRectangle, AdPosition.BottomLeft);
        rectBannerBottomLeft.LoadAd(new AdRequest());
    }
    public void ShowRectBanner()
    {
        if (isRemoveAd())
        {
            return;
        }
        if (showAllAds == false)
        {
            return;
        }
        if (isRectbanner())
        {
            if (rectBannerBottomLeft != null) rectBannerBottomLeft.Hide();
            rectBannerBottomLeft?.Show();
        }
    }
    public void HideRectBanner()
    {
        if (showAllAds == false)
        {
            return;
        }
        if (rectBannerBottomLeft != null)
        {
            rectBannerBottomLeft?.Hide();
        }
        if (rectBannerBottomLeft != null)
        {
            rectBannerBottomLeft.Hide();
        }
    }
    #endregion

    #endregion

    #region AdaptiveBanner
    private bool isHighEcpmLoadedAdaptiveBanner = false;
    private void RequestAdaptiveBannerAd()
    {

        #region StandardBannerSizeCommented
        /*  // Smart banner top right
          smartBannerTopRight = new BannerView(BannerAdIDRight, AdSize.Banner, AdPosition.Top);
          smartBannerTopRight.LoadAd(new AdRequest());*/
        #endregion
        // DestroyAdaptiveBanner();

        // Get screen width in pixels
        int screenWidth = Screen.width;

        // Get adaptive banner size based on screen width
        AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(screenWidth);

        // Initialize banner view with adaptive size
        AdaptiveBannerTop = new BannerView(AdaptiveBannerAdID, adaptiveSize, AdPosition.Top);
        AdaptiveBannerTop.OnBannerAdLoaded += AdaptiveBannerLoaded;
        AdaptiveBannerTop.OnBannerAdLoadFailed += AdaptiveBannerFailedToLoad;
        // Load the ad
        //AdRequest request = new AdRequest.Builder().Build();
        AdaptiveBannerTop.LoadAd(new AdRequest());
        //   AdaptiveBannerTop.Hide();


    }
    void AdaptiveBannerLoaded()
    {
    }
    void AdaptiveBannerFailedToLoad(LoadAdError error)
    {
    }
    void DestroyAdaptiveBanner()
    {
        if (AdaptiveBannerTop != null)
        {
            AdaptiveBannerTop.OnBannerAdLoaded -= AdaptiveBannerLoaded;
            AdaptiveBannerTop.OnBannerAdLoadFailed -= AdaptiveBannerFailedToLoad;
            AdaptiveBannerTop.Destroy();
            AdaptiveBannerTop = null;
        }
    }

    public bool isAdaptiveBanner()
    {
        if (AdaptiveBannerTop != null)
        {
            return true;
        }
        RequestAdaptiveBannerAd();
        return AdaptiveBannerTop != null;

    }


    public void ShowAdaptiveBanner()
    {
        if (isRemoveAd())
        {
            return;
        }
        if (isAdaptiveBanner())
        {
            AdaptiveBannerTop?.Show();
            CheckAdaptiveBannerLog();
        }
    }
    void CheckAdaptiveBannerLog()
    {

        LogEvent("adaptivebanner_show");


    }
    #endregion
    #region Rewarded Ads
    private void RequestRewardedAds()
    {
        AdRequest adRequest = new AdRequest();

        // Request Rewarded Video
        RewardedAd.Load(RewardedVideoAdID, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                Debug.LogError("Failed to load Rewarded Video: " + error.GetMessage());
                return;
            }
            rewardedVideoAd = ad;
            rewardedVideoAd.OnAdFullScreenContentClosed += HandleRewardedAdClosed;
        });

        // Request Rewarded Interstitial
        /*     RewardedInterstitialAd.Load(RewardedInsterstitialAdID, adRequest, (RewardedInterstitialAd ad, LoadAdError error) =>
             {
                 if (error != null)
                 {
                     Debug.LogError("Failed to load Rewarded Interstitial: " + error.GetMessage());
                     return;
                 }
                 rewardedInterstitialAd = ad;
                 rewardedInterstitialAd.OnAdFullScreenContentClosed += HandleRewardedAdClosed;
             });*/
    }



    public void ShowRewardedAD(Action adCallback)
    {
        OnAdCompleted = adCallback;
        if (isAdmobRewardedVideoAvailable())
        {
            rewardedVideoAd.Show((Reward reward) => {
                Debug.Log("User rewarded from Rewarded Video Ad.");
                sentCallforReward();
            });
            showRewardedVideoNext = false; // Next time show Rewarded Interstitial
        }
        else
        {
#if UNITY_EDITOR
            OnAdCompleted?.Invoke();
#endif
            Debug.LogWarning("No rewarded ad available.");
        }
    }
    public bool isAdmobRewardedVideoAvailable()
    {
        if (withoutAds)
        {
            return false;
        }

        if (rewardedVideoAd != null && rewardedVideoAd.CanShowAd())
        {
            return true;
        }
        else
        {
            RequestRewardedAds();
            return false;

        }
    }
    private void HandleRewardedAdClosed()
    {
        RequestRewardedAds(); // Reload rewarded ads after they close
    }

    void sentCallforReward()
    {
        OnAdCompleted?.Invoke();
        Invoke(nameof(nullifyEvent), 1.0f);
    }
    void nullifyEvent()
    {
        OnAdCompleted = null;
    }

    #endregion

    #region App Open Ads
    private void RequestAppOpenAd()
    {
        if (isRemoveAd())
        {
            return;
        }
        if (showAllAds)
        {
            if (isLowEndDevice)
            {
                return;
            }
            // Create an AdRequest
            AdRequest adRequest = new AdRequest();

            // Load the App Open Ad
            AppOpenAd.Load(AppOpenAdID, adRequest, (AppOpenAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Failed to load App Open Ad: " + error.GetMessage());
                    return;
                }
                appOpenAd = ad;

                // Subscribe to event for when the ad is closed
                appOpenAd.OnAdFullScreenContentClosed += HandleAppOpenAdClosed;

                Debug.Log("App Open Ad Loaded Successfully.");
            });
        }
    }
    public void ShowAppOpenAd()
    {
        if (isRemoveAd())
        {
            return;
        }
        if (showAllAds == false)
        {
            return;
        }

        if (isLowEndDevice)
        {
            return;
        }
        if (IsAdmobAvaliable())
        {
            appOpenAd.Show();

            if (showAdaptiveBanner)
            {
                AdaptiveBannerTop.Hide();
            }
            else
            {
                HideSmartBanners();
            }

        }
    }
    public bool IsAdmobAvaliable()
    {
        if (appOpenAd != null && appOpenAd.CanShowAd())
        {
            return true;
        }
        else
        {
            RequestAppOpenAd();
            return false;
        }
    }
    public void ShowAppOpenRemotely()
    {
        if (showAppOpen)
        {
            ShowAppOpenAd();
        }
    }

    private void HandleAppOpenAdClosed()
    {
        RequestAppOpenAd(); // Reload app open ad after closing
        if (showAdaptiveBanner)
        {
            AdaptiveBannerTop.Show();
        }
        else
        {
            ShowSmartBanners();
        }

    }
    #endregion

    #region Firebase Analytics
    public void LogAnalyticsEvent(string eventName, string parameterName = null, string parameterValue = null)
    {
        FirebaseAnalytics.LogEvent(eventName, new Firebase.Analytics.Parameter(parameterName, parameterValue));
    } 
    
    public void LogAnalyticsEvent(string eventName)
    {
        FirebaseAnalytics.LogEvent(eventName);
    }

    public void LogEvent(string name)
    {
        if (adsType == AdsType.TestAds)
        {
            return; // Skip logging if test ads are enabled
        }

        if (!isFirebaseEnable)
        {
            return; // Skip logging if Firebase is disabled
        }

        try
        {
            FirebaseAnalytics.LogEvent(ReplaceSpacesWithUnderscores(name));
        }
        catch (Exception ex)
        {
            Debug.LogError("Firebase LogEvent Exception: " + ex.Message);
        }

    }
    string ReplaceSpacesWithUnderscores(string inputString)
    {
        if (!inputString.Contains(" "))
        {
            // If there are no spaces, return the original string
            return inputString;
        }
        // Replace spaces with underscores
        string result = inputString.Replace(" ", "_");

        return result;
    }
    #endregion

    #region FirstOpenchecker
    public bool isFirstOpen()
    {
        if (PlayerPrefs.GetInt("firstopen") == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void setFirstOpenFalse()
    {
        PlayerPrefs.SetInt("firstopen", 1);
    }

    #endregion

    #region RemoveAdsHere
    public void RemoveAds()
    {
        PlayerPrefs.SetInt("removeAds", 1);
        HideAllBanners();
        _SendEvent("UserBuyInAppToRemoveAllAds");
    }
    #endregion
}

[Serializable]
public enum AdsType
{
    TestAds,
    LiveAds
}
#region ShowIfTrueDrawer
/// <summary>
/// Below is Editor scripting for show if true code
/// </summary>

public class ShowIfTrueAttribute : PropertyAttribute
{
    public string ConditionalPropertyName { get; private set; }
    public bool InvertCondition { get; private set; }

    public ShowIfTrueAttribute(string conditionalPropertyName, bool invertCondition = false)
    {
        ConditionalPropertyName = conditionalPropertyName;
        InvertCondition = invertCondition;
    }
}// Add this class
public class ShowIfFalseAttribute : PropertyAttribute
{
    public string ConditionalPropertyName { get; private set; }
    public bool InvertCondition { get; private set; }

    public ShowIfFalseAttribute(string conditionalPropertyName, bool invertCondition = false)
    {
        ConditionalPropertyName = conditionalPropertyName;
        InvertCondition = invertCondition;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ShowIfTrueAttribute))]
[CustomPropertyDrawer(typeof(ShowIfFalseAttribute))] // Add this line
public class ShowIfTrueDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ShowIfTrueAttribute showIfTrueAttribute = attribute as ShowIfTrueAttribute;
        ShowIfFalseAttribute showIfFalseAttribute = attribute as ShowIfFalseAttribute; // Add this line

        SerializedProperty conditionalProperty = property.serializedObject.FindProperty(showIfTrueAttribute.ConditionalPropertyName);

        if (conditionalProperty != null && conditionalProperty.propertyType == SerializedPropertyType.Boolean)
        {
            bool show = showIfTrueAttribute.InvertCondition ? !conditionalProperty.boolValue : conditionalProperty.boolValue;

            if (show)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }
        else
        {
            EditorGUI.LabelField(position, label.text, "Error: Conditional property not found or not a boolean.");
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ShowIfTrueAttribute showIfTrueAttribute = attribute as ShowIfTrueAttribute;
        ShowIfFalseAttribute showIfFalseAttribute = attribute as ShowIfFalseAttribute; // Add this line

        SerializedProperty conditionalProperty = property.serializedObject.FindProperty(showIfTrueAttribute != null ? showIfTrueAttribute.ConditionalPropertyName : showIfFalseAttribute.ConditionalPropertyName); // Modify this line

        if (conditionalProperty != null && conditionalProperty.propertyType == SerializedPropertyType.Boolean)
        {
            bool show = showIfTrueAttribute.InvertCondition ? !conditionalProperty.boolValue : conditionalProperty.boolValue;

            if (show)
            {
                return EditorGUI.GetPropertyHeight(property, label);
            }
            else
            {
                return 0f;
            }
        }
        else
        {
            return EditorGUI.GetPropertyHeight(property, label) + EditorGUIUtility.singleLineHeight;
        }
    }
}
#endif
#endregion