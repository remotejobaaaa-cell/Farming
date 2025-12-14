using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MTAssets.EasyMinimapSystem;
using UnityEngine.Serialization;

public class HudMenuManager : MonoBehaviour
{
    public static HudMenuManager instance;

    public PlayerSpanwer playerSpanwer;

    [Header("Menus")]
    public GameObject environment; 
    public GameObject TPSControls;
    public GameObject RCCControls;
    public GameplayMenuManager gameplayMenu;
    public LevelObjectiveMenuManager levelObjectiveMenu;
    public GamePauseMenuManager pauseMenu;
    public GameOverMenuManager gameOverMenu;
    public LevelManager currentlyActiveLevel;

    //[HideInInspector]
    public ThirdPersonControl thirdPersonControl;
    public RCC_MobileButtons rccMobileButtons;
    public RCC_DashboardInputs rccDashboardInputs;

    [Header("Loading")]
    [Space(2)]
    public string mainMenuName = "";
    public GameObject loadingMenu;
    public Slider loadingSlider;
    private AsyncOperation ao;
    public Text loadingProgressText;

    public PlayerData playerData;

    public GameObject confettiParticles;

    private void Awake()
    {

        instance = this;
        GetReffences();
        environment.SetActive(true);
        playerData = PlayerDataController.instance.playerData;
    }
   public IEnumerator ConfettiOnOff()
    {
        confettiParticles.SetActive(true);
        yield return new WaitForSecondsRealtime(5);
        gameOverMenu.noThanksBtn.SetActive(true);
        confettiParticles.SetActive(false);
    }
    private void OnEnable()
    {
        Time.timeScale = 1;
#if HAVE_ADS
        if (AdsManager.instance)
            AdsManager.instance.HideRectBanner();
#endif
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            GameConstants.isPlayerWin = true;
          GameOver(2);
        }
    }
    public void GetReffences()
    {
        thirdPersonControl = TPSControls.GetComponent<ThirdPersonControl>();
        rccMobileButtons = RCCControls.GetComponent<RCC_MobileButtons>();
        rccDashboardInputs = RCCControls.GetComponent<RCC_DashboardInputs>();
    }
    private void OnDisable()
    {
        if (PlayerDataController.instance)
        {
            if (PlayerDataController.instance.playerData.isBackgroundMusicOn)
            {
                if (AudioManager.instance)
                {
                    AudioManager.instance.Play_MenuBackgroundMusic();
                }
            }
            else
            {
                if (AudioManager.instance)
                {
                    AudioManager.instance.backgroundMusicAudioSource.Stop();
                }
            }
        }
    }

    public void ShowThirdPersonControl(bool value)
    {
        if (TPSControls)
        {
            TPSControls.SetActive(value);
        }
    }

    public void ShowVehicleControl(bool value)
    {
        if (RCCControls)
        {
            RCCControls.SetActive(value);
        }
    }

    public void SetCurrentLevel(LevelManager currentLevel)
    {
        currentlyActiveLevel = currentLevel;
        if (currentlyActiveLevel.cinematics == Cinematics.Yes)
        {
            gameplayMenu.gameObject.SetActive(false);
            currentlyActiveLevel.ShowCinametics();
        }
        else
        {
            levelObjectiveMenu.gameObject.SetActive(true);
            playerSpanwer.SpawnPlayer(currentlyActiveLevel, currentlyActiveLevel.playerInsPos);
            gameplayMenu.minimapRoute.startingPoint = playerSpanwer.currentPlayer.transform;
        }
        gameplayMenu.minimapRoute.StartCalculatingAndShowRotesToDestination();
    }

    public void CinameticsCompleted()
    {
        gameplayMenu.gameObject.SetActive(true);
        levelObjectiveMenu.gameObject.SetActive(true);
    }

    public void StartLevel()
    {
        if (currentlyActiveLevel.cinematics == Cinematics.Yes)
        {
            playerSpanwer.SpawnPlayer(currentlyActiveLevel, currentlyActiveLevel.playerInsPos);
            currentlyActiveLevel.HideCinametics();
        }
        if (currentlyActiveLevel.levelType == LevelType.ThirdPerson)
        {
            ShowThirdPersonControl(true);
            ShowVehicleControl(false);
        }
        else
        if (currentlyActiveLevel.vehicleType == VehicleType.Drone)
        {
            ShowThirdPersonControl(true);
            ShowVehicleControl(false);
        }
        else
        {
            ShowThirdPersonControl(false);
            ShowVehicleControl(true);
        }
        GameplayInitialize();
        HudMenuManager.instance.gameplayMenu.minimapRoute.StartCalculatingAndShowRotesToDestination();

    }

    public void GameplayInitialize()
    {
        gameplayMenu.gameObject.SetActive(true);
        gameplayMenu.minimap.minimapCameraToShow = playerSpanwer.minimapCamera;
        gameplayMenu.minimapRoute.startingPoint = playerSpanwer.currentPlayer.transform;
        gameplayMenu.minimap.gameObject.SetActive(true);
        gameplayMenu.minimapRoute.gameObject.SetActive(true);
    }

    public void PauseGame()
    {
        pauseMenu.gameObject.SetActive(true);
    }

    public void GameOver()
    {
        gameOverMenu.gameObject.SetActive(true);
    }

    public void GameOver(float delay)
    {
        //StartCoroutine(ConfettiOnOff());
        if (AudioManager.instance)
            AudioManager.instance.Play_Final_CheckPointSound();
        Invoke("GameOver", delay);
    }

    public void RetryLevel()
    {
        Time.timeScale = 1;
        // StartCoroutine(LoadScene(SceneManager.GetActiveScene().name));
#if HAVE_ADS
        if (AdsManager.instance)
            AdsManager.instance.ShowRectBanner();
#endif

        StartCoroutine(Method(SceneManager.GetActiveScene().name));
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        //StartCoroutine(LoadScene(mainMenuName));
#if HAVE_ADS
        if (AdsManager.instance)
            AdsManager.instance.ShowRectBanner();
#endif
        StartCoroutine(Method(mainMenuName));
    }
   public float totalFakeLoadTime; // 20 seconds fake load time

    IEnumerator Method(string a)
    {
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

        SceneManager.LoadScene(a);
        // StartCoroutine(LoadTheLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }






    private IEnumerator LoadScene(string a)
    {
        loadingMenu.SetActive(true);
        loadingSlider.value = 0f;
        loadingProgressText.text = "0%";

        ao = SceneManager.LoadSceneAsync(a);
        while (!ao.isDone)
        {
            loadingSlider.value = ao.progress;
            loadingProgressText.text = (int)(ao.progress * 100) + "%";

            if (ao.progress == 0.9f)
            {
                loadingSlider.value = 1f;
                loadingProgressText.text = 100 + "%";
                ao.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
