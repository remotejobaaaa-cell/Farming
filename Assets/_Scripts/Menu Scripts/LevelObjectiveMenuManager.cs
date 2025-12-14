using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelObjectiveMenuManager : MonoBehaviour
{
    public LevelLoaderManager levelLoaderManager;
    public lb_BirdController birdControl;
    public HudMenuManager hudMenu;

    public Sprite[] levelImagesprites;
    public Image levelImage;
    public Text levelDescription;
    public AudioClip typingSound;
    public GameObject adLogo;

    private void OnEnable()
    {
        levelDescription.text = "";
        levelDescription.DOText(GameConstants.currentlySelectedLevelDescription, 1);
        hudMenu.GetComponent<AudioSource>().PlayOneShot(typingSound);

        levelImage.sprite = levelImagesprites[GameConstants.currentlySelectedLevel];

        // Handle ad logo visibility only if ads are enabled
#if HAVE_ADS
        UpdateAdLogoVisibility();
#else
    if (adLogo)
    {
        adLogo.SetActive(false); // Ensure adLogo is hidden if ads are disabled
    }
#endif
    }

#if HAVE_ADS
    private void UpdateAdLogoVisibility()
    {
        bool isAdAvailable = AdsManager.instance && AdsManager.instance.IsIntertitialAvaliable() && !AdsManager.instance.isAdlimit;

        if (GameConstants.currentlySelectedLevel == 0)
        {
            if (logoCounter == 0)
            {
                logoCounter = 1;
                if (adLogo) adLogo.SetActive(false);
            }
            else if (logoCounter == 1 && adLogo)
            {
                adLogo.SetActive(isAdAvailable);
            }
        }
        else
        {
            if (adLogo) adLogo.SetActive(isAdAvailable);
        }
    }
#endif
    public int logoCounter=0;
    public int adCounter=0;

    public void OnClick_Play()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
        if (levelLoaderManager.currentLevel.cinematicObject)
        {
            levelLoaderManager.currentLevel.cinematicObject.SetActive(false);
        }
        hudMenu.StartLevel();
        gameObject.SetActive(false);
        int levelstart = GameConstants.currentlySelectedLevel + 1;
        Debug.Log("Start_Level_" + levelstart);
        AdsManager.instance.LogEvent("level_start_" + levelstart);
        //birdControl.mYStart();
        if (levelstart == 1)
        {
            if(adCounter==0)
            {
                adCounter = 1;
            }
            else if(adCounter==1)
            {
                DisplayAd();
            }
        }
        else
        {
            DisplayAd();
        }
    }

    public void DisplayAd()
    {
#if HAVE_ADS
       // if (AdsManager.instance && AdsManager.instance.IsIntertitialAvaliable() && !AdsManager.instance.isRemoveAd() && !AdsManager.instance.isAdlimit)
         //   AdsManager.instance.ShowInterstitialAd();
#endif
    }
}
