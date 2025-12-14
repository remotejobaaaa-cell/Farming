using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class LevelSelectionMenuManager : MonoBehaviour
{
    public MainMenuManager mainMenuManager;

    public ScrollRect scrollRect;
    public RectTransform contentPanel;
    public Image levelImage;
    public Text levelDescriptionText;
    public GameObject playButton;
    public GameObject lockedButton;
    public AudioClip typingSound;
    public ScrollRect levelScrollRect;
    public Sprite[] selectedllevelSprite;
    public GameObject unlockAllLevelBtn;

    public LevelObjectives[] levelObjectives;
    public List<LevelUIButton> levelButtons;
    public string[] sceneName;

    [Header("View Advertisement")]
    public GameObject viewAdMenu;
    public Text countdownText; // Reference to the countdown text element
    public int countdownDuration = 3; // Countdown duration in seconds
    public GameObject adLogo;

    private void OnEnable()
    {

        Initialize();
#if HAVE_ADS
        if (adLogo)
        {
            if (AdsManager.instance && AdsManager.instance.IsIntertitialAvaliable() && !AdsManager.instance.isAdlimit)
                adLogo.SetActive(true);
            else
                adLogo.SetActive(false);
        }
#else
    if (adLogo)
    {
        adLogo.SetActive(false); // Hide ad logo if ads are not enabled
    }
#endif
    }
    public void Initialize()
    {
        IAPInitialize();
        GameConstants.currentlySelectedLevel = mainMenuManager.playerData.maps[GameConstants.currentlySelectedMap].levelCompleted;
        LevelSnapping(GameConstants.currentlySelectedLevel);
    }
    public void IAPInitialize()
    {
        if (mainMenuManager.playerData.unlockAllLevels == 3)
        {
            unlockAllLevelBtn.transform.GetChild(0).gameObject.SetActive(false);
            for (int i = 0; i < levelButtons.Count; i++)
            {
                mainMenuManager.playerData.maps[0].routes[i].isLocked = false;
                levelButtons[i].Initialize();
            }
        }
        else
        {
            unlockAllLevelBtn.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    
        private void Start()
    {
    }
    public void SelectLevel(LevelUIButton levelUIButton)
    {
        GameConstants.currentlySelectedLevel = levelUIButton.id;
        GameConstants.currentlySelectedLevelDescription = levelObjectives[GameConstants.currentlySelectedMap].objectives[levelUIButton.id];
        levelImage.sprite = levelUIButton.levelSprite;
        levelDescriptionText.text = "";
        levelDescriptionText.DOText(levelObjectives[GameConstants.currentlySelectedMap].objectives[levelUIButton.id], 1);
        if (AudioManager.instance)
        {
            AudioManager.instance.GetComponent<AudioSource>().PlayOneShot(typingSound);
        }
        foreach (LevelUIButton level in levelButtons)
        {
            level.selectedlevelImage.sprite = selectedllevelSprite[0];
        }
        levelButtons[GameConstants.currentlySelectedLevel].selectedlevelImage.sprite = selectedllevelSprite[1];

        if (mainMenuManager.playerData.maps[GameConstants.currentlySelectedMap].routes[levelUIButton.id].isLocked)
        {
            playButton.SetActive(false);
            lockedButton.SetActive(true);
        }
        else
        {
            playButton.SetActive(true);
            lockedButton.SetActive(false);
        }
    }
    public void Method()
    {
        StartCoroutine(mainMenuManager.Loading());
    }
    public void OnClick_Play()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }

#if HAVE_ADS
        if (AdsManager.instance && AdsManager.instance.IsIntertitialAvaliable() && !AdsManager.instance.isRemoveAd() && !AdsManager.instance.isAdlimit)
        {
            AdsManager.instance.ShowInterstitialAd(Method);
        }
        else
        {
            Method();
        }

        int selectedLevel = GameConstants.currentlySelectedLevel + 1;
        AdsManager.instance.LogAnalyticsEvent("level_select_" + selectedLevel);
#else
    Method(); // Proceed directly if ads are disabled
#endif

        // mainMenuManager.LoadLevel(sceneName[GameConstants.currentlySelectedMap]);

        if (AudioManager.instance)
        {
            //AudioManager.instance.GetComponent<AudioListener>().enabled = false;
        }

        Invoke("DelayAudio", 3f);

        int SelectedLevel = GameConstants.currentlySelectedLevel + 1;
        UnityAnalytics(SelectedLevel);
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
       // if (AdsManager.instance != null)
         //   AdsManager.instance.ShowInterstitialAd(null);
#endif
        //StartCoroutine(ShowInterstitialWithCountdown());

    }

    #endregion

    public void LevelSnapping(int currentLevel)
    {
        SelectLevel(levelButtons[currentLevel]);
       // SnapTo(levelButtons[currentLevel].GetComponent<RectTransform>());
        var criticalPoint = levelButtons.Count / 2;
        float endValue;
        float levelCount = (float)levelButtons.Count;
        if (currentLevel < criticalPoint)
        {
            endValue = (float)(currentLevel) / levelCount;
        }
        else
        {
            endValue = (float)(currentLevel + 1) / levelCount;
        }
        levelScrollRect.DOHorizontalNormalizedPos(endValue, 1.5f);
    }

    private void SnapTo(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();

        contentPanel.anchoredPosition =
            (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position)
            - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
    }

    public void DelayAudio()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_BackgroundMusic();
        }
    }
    void UnityAnalytics(int massage)
    {
      
    }
    public void ObjectivePanelOff()
    {
        LeanTween.move(objectivePanel.GetComponent<RectTransform>(), new Vector2(500, -67), duration);
        StartCoroutine(DelayInOff(levelobjectivePanel));  
    }

    public GameObject levelobjectivePanel;
    public Transform objectivePanel;
    public float duration;
    public LeanTweenType easeType;


    public void LeanArray()
    {
        objectivePanel.GetComponent<RectTransform>().localPosition = new Vector2(2000, -67);

        LeanTween.move(objectivePanel.GetComponent<RectTransform>(), new Vector2(-34, -67), duration);
    }
    IEnumerator DelayInOff(GameObject g)
    {
        yield return new WaitForSeconds(duration);
        g.SetActive(false);
    }
}
