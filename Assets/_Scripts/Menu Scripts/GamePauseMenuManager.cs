using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseMenuManager : MonoBehaviour
{
    public HudMenuManager hudMenu;

    [Header("Toggles")]
    public Toggle bgmToggle;
    public Toggle soundFxToggle;

    [Header("Game Volume")]
    public Slider gameVolumeSlider;
    [Header("Camera Sensitivity")]
    public Slider cameraSensitivitySlider;

    [Header("View Advertisement")]
    public GameObject viewAdMenu;
    public Text countdownText; // Reference to the countdown text element
    public int countdownDuration = 3; // Countdown duration in seconds

    private void OnEnable()
    {
        Time.timeScale = 0;
        Initialization();
        DisplayAds();
#if HAVE_ADS
        if (AdsManager.instance)
            AdsManager.instance.ShowRectBanner();
#endif
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
    public void DisplayAds()
    {
#if HAVE_ADS
        if (AdsManager.instance)
        AdsManager.instance.ShowInterstitialAd(null);
#endif
        //StartCoroutine(ShowInterstitialWithCountdown());

    }

    #endregion
    public void OnClick_Resume()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
        SetSound();
        PlayerDataController.instance.Save();

#if HAVE_ADS
        if (AdsManager.instance)
            AdsManager.instance.HideRectBanner();
#endif
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
    private void Initialization()
    {
        bgmToggle.isOn = PlayerDataController.instance.playerData.isBackgroundMusicOn;
        soundFxToggle.isOn = PlayerDataController.instance.playerData.isSoundFxOn;
        gameVolumeSlider.value = PlayerDataController.instance.playerData.gameVolume;
        cameraSensitivitySlider.value = PlayerDataController.instance.playerData.cameraSensitivity;
    }
    public void ChangeBGM(bool value, Toggle toggle)
    {
        if (toggle == bgmToggle)
        {
            PlayerDataController.instance.playerData.isBackgroundMusicOn = value;
            if (value == true)
            {
                if (AudioManager.instance)
                {
                    AudioManager.instance.backgroundMusicAudioSource.volume = PlayerDataController.instance.playerData.gameVolume;
                }
                if (hudMenu.currentlyActiveLevel.levelType == LevelType.Vehicle)
                {
                    if (hudMenu.currentlyActiveLevel.vehicleType == VehicleType.Harvestor)
                    {
                        hudMenu.playerSpanwer.harvesterAudioSource.allAudioSource.SetActive(true);
                    }
                }
            }
            else
            {
                if (AudioManager.instance)
                {
                    AudioManager.instance.backgroundMusicAudioSource.Stop();
                }

                if (hudMenu.currentlyActiveLevel.levelType == LevelType.Vehicle)
                {
                    if (hudMenu.currentlyActiveLevel.vehicleType == VehicleType.Harvestor)
                    {
                        hudMenu.playerSpanwer.harvesterAudioSource.allAudioSource.SetActive(false);
                    }
                }
            }
        }
        else if (toggle == soundFxToggle)
        {
            PlayerDataController.instance.playerData.isSoundFxOn = value;
            if (value == true)
            {
                //if (hudMenu.currentlyActspotifyiveLevel.levelType == LevelType.Vehicle)
                //{
                //    hudMenu.playerSpanwer.vehicleCamera.GetComponent<RCC_Camera>().pivot.transform.GetChild(0).GetComponent<AudioListener>().enabled = true;
                //}
                //else if (hudMenu.currentlyActiveLevel.levelType == LevelType.ThirdPerson)
                //{
                //    hudMenu.playerSpanwer.farmerCamera.transform.GetChild(0).GetComponent<AudioListener>().enabled = true;
                //}
            }
            else
            {
                //if (hudMenu.currentlyActiveLevel.levelType == LevelType.Vehicle)
                //{
                //    hudMenu.playerSpanwer.vehicleCamera.GetComponent<RCC_Camera>().pivot.transform.GetChild(0).GetComponent<AudioListener>().enabled = false;
                //}
                //else if (hudMenu.currentlyActiveLevel.levelType == LevelType.ThirdPerson)
                //{
                //    hudMenu.playerSpanwer.farmerCamera.transform.GetChild(0).GetComponent<AudioListener>().enabled = false;
                //}
            }

        }
    }

    public void SetSound()
    {
        PlayerDataController.instance.playerData.isBackgroundMusicOn = bgmToggle.isOn;
        PlayerDataController.instance.playerData.isSoundFxOn = soundFxToggle.isOn;
        PlayerDataController.instance.playerData.gameVolume = gameVolumeSlider.value;
        if (AudioManager.instance)
        {
            AudioManager.instance.backgroundMusicAudioSource.volume = PlayerDataController.instance.playerData.gameVolume;
        }
        PlayerDataController.instance.playerData.cameraSensitivity = cameraSensitivitySlider.value;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
        SetSound();
        PlayerDataController.instance.Save();

    }


}
