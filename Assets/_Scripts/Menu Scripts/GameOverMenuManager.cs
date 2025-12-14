using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenuManager : MonoBehaviour
{
    public HudMenuManager hudMenu;
    public GameObject levelCompletePopup;
    public GameObject levelCollectRewardPopup;
    public GameObject levelFailPopup;
    public static int rateScreenCounter = 1;
    public static int adsCounter;
    public GameObject gameRateScreen;
    public GameObject noThanksBtn;

    public Text declineText;
    public GameObject giveRewardPanel;

    [Header("Level Complete Components")]
    public Text rewardCash;
    public Text rewardCash_2x;
    public Text collectRewardCash;

    [Header("View Advertisement")]
    public GameObject viewAdMenu;
    public Text countdownText; // Reference to the countdown text element
    public int countdownDuration = 3; // Countdown duration in seconds
  
    private void OnEnable()
    {
        Time.timeScale = 1;
        if (GameConstants.isPlayerWin)
        {
            LevelComplete();
        }
        else
        {
            LevelFailed();
        }
#if HAVE_ADS
        if (AdsManager.instance != null)
            AdsManager.instance.ShowRectBanner();
#endif
    }
    float levelReward;
    public void LevelComplete()
    {
        levelCollectRewardPopup.SetActive(true);
        levelCompletePopup.SetActive(true);
        levelFailPopup.SetActive(false);
        if (PlayerDataController.instance.playerData.rateUS != 3)
        {
            if (rateScreenCounter % 2 == 0)
            {
                gameRateScreen.SetActive(true);
            }
        }
        rateScreenCounter++;
         levelReward = (float)PlayerDataController.instance.playerData.maps[GameConstants.currentlySelectedMap].routes[GameConstants.currentlySelectedLevel].reward;

        rewardCash.text = levelReward.ToString();
        // collectRewardCash.text = "Congratulations! You've just gained "+ levelReward + " coins.";
        collectRewardCash.text = "Congratulations! You've just gained <color=yellow>" + levelReward.ToString() + "</color> coins."; 
        if (GameConstants.currentlySelectedLevel >= PlayerDataController.instance.playerData.routeCompleted)
        {
            PlayerDataController.instance.playerData.playerCash += levelReward;
            if (GameConstants.currentlySelectedLevel < (PlayerDataController.instance.playerData.maps[GameConstants.currentlySelectedMap].routes.Count - 1))
            {
                PlayerDataController.instance.playerData.maps[GameConstants.currentlySelectedMap].routes[GameConstants.currentlySelectedLevel + 1].isLocked = false;
                PlayerDataController.instance.playerData.routeCompleted += 1;
                PlayerDataController.instance.playerData.maps[GameConstants.currentlySelectedMap].levelCompleted += 1;
            }
            PlayerDataController.instance.Save();
        }
        int levelComplete = GameConstants.currentlySelectedLevel + 1;
        Debug.Log("Complete_Level_" + levelComplete);
#if HAVE_ADS
        if (AdsManager.instance)
        AdsManager.instance.LogEvent("level_complete_" + levelComplete);
#endif
        noThanksBtn.SetActive(true);

    }
    public void WatchAd_GetReward()
    {
        if (AudioManager.instance)
            AudioManager.instance.Play_ClickButtonSound();
#if HAVE_ADS
        if (AdsManager.instance && AdsManager.instance.isAdmobRewardedVideoAvailable())
            AdsManager.instance.ShowRewardedAD(GiveCashReward);
        else
            NoRewardedAdAvailable();
#else
    NoRewardedAdAvailable();  // Call this even if ads are disabled
#endif
    }
    public void GiveCashReward()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickInGameRewardSound();
            AudioManager.instance.Play_ClickConfettiSound();
        }
          
        StartCoroutine(hudMenu.ConfettiOnOff());
        giveRewardPanel.SetActive(true);
        levelReward += levelReward ;
        rewardCash.text = levelReward.ToString();
        rewardCash_2x.text = "Congratulations! You've just Double Your Reward Now Your Reward is ." + levelReward.ToString()+ " coins";

        PlayerDataController.instance.playerData.playerCash = PlayerDataController.instance.playerData.playerCash + levelReward;
        PlayerDataController.instance.Save();
    }
    public void NoRewardedAdAvailable()
    {
        declineText.text = "No Video Ad Available, Try Again Later.";
        declineText.gameObject.SetActive(true);
        declineText.GetComponent<DOTweenAnimation>().DORestart();
    }
    public void LevelFailed()
    {
        levelFailPopup.SetActive(true);
        levelCompletePopup.SetActive(false);
        int levelFailed = GameConstants.currentlySelectedLevel + 1;
       // UnityAnalytics(levelFailed);
    }

    public void OnClick_Retry()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
        hudMenu.RetryLevel();
    }

    public void OnClick_MainMenu()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
        GameConstants.openLevels = false;
        hudMenu.LoadMainMenu();
    }

    public void OnClick_Next()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
        if (GameConstants.currentlySelectedLevel < (PlayerDataController.instance.playerData.maps[GameConstants.currentlySelectedMap].routes.Count - 1))
        {
            GameConstants.currentlySelectedLevel += 1;
            hudMenu.RetryLevel();
        }
        else
        {
            GameConstants.openLevels = true;
            hudMenu.LoadMainMenu();
        }
    }
    void UnityAnalytics(int massage)
    {
       
    }
    #region Ads Method
    private IEnumerator ShowInterstitialWithCountdown()
    {
        viewAdMenu.SetActive(true);
        int countdown = countdownDuration;

        // Update countdown text before starting the countdown
        countdownText.text = "Viewing Advertisement.... " + countdown.ToString();

        while (countdown > 0)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            countdown--;
            countdownText.text = "Viewing Advertisement.... " + countdown.ToString();
        }
        viewAdMenu.SetActive(false);
#if HAVE_ADS
        AdsManager.instance.ShowInterstitialAd();
#endif
    }
    public void OnclickCollect_Reward()
    {
        levelCollectRewardPopup.SetActive(false);
#if HAVE_ADS
        if (AdsManager.instance)
        AdsManager.instance.ShowInterstitialAd(null);
#endif
        // StartCoroutine(ShowInterstitialWithCountdown());
    }
    public void OnclickCollect_WithWatchRewarded_AD()
    {
        levelCollectRewardPopup.SetActive(false);
        giveRewardPanel.SetActive(false);
        // StartCoroutine(ShowInterstitialWithCountdown());
    }

    #endregion
}
