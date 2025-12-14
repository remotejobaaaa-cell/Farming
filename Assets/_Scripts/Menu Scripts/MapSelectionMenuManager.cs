using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MapSelectionMenuManager : MonoBehaviour
{
    public MainMenuManager mainMenuManager;

    public Text mapNameText;
    public Text mapDescriptionText;
    public GameObject lockedIcon;
    public Text mapPriceText;
    public GameObject unlockWithCashButton;
    public GameObject selectMapButton;
    public Image mapImage;
    public Sprite[] mapSprite;
    public Image[] dots;
    public Color newDotColor;
    private int mapIndex;

    private void OnEnable()
    {
        mapIndex = GameConstants.currentlySelectedMap;
        InitializeMap();
    }

    public void OnClick_NextMap()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
        mapIndex++;
        if (mapIndex >= mapSprite.Length)
        {
            mapIndex = 0;
        }
        InitializeMap();
    }

    public void OnClick_PreviousMap()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
        mapIndex--;
        if (mapIndex < 0)
        {
            mapIndex = mapSprite.Length - 1;
        }
        InitializeMap();
    }

    private void InitializeMap()
    {
        mapNameText.text = mainMenuManager.playerData.maps[mapIndex].mapName;
        mapDescriptionText.text = mainMenuManager.playerData.maps[mapIndex].mapDescription;
        foreach (var item in dots)
        {
            item.color = Color.white;
        }
        //var newColor = new Color(36, 169, 227);
        //dots[mapIndex].color = Color.blue;    
        dots[mapIndex].color = newDotColor;
        mapImage.sprite = mapSprite[mapIndex];
        if (mainMenuManager.playerData.maps[mapIndex].isLocked)
        {
            mapPriceText.text = mainMenuManager.playerData.maps[mapIndex].unlockPrice.ToString();
            lockedIcon.SetActive(true);
            //unlockWithCashButton.SetActive(true);
            selectMapButton.SetActive(false);
        }
        else
        {
            lockedIcon.SetActive(false);
            unlockWithCashButton.SetActive(false);
            selectMapButton.SetActive(true);
        }
    }

    public void OnClick_SelectMap()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
        mainMenuManager.playerData.currentlySelectedMap = mapIndex;
        GameConstants.currentlySelectedMap = mapIndex;
        StartCoroutine(mainMenuManager.Loading());

#if HAVE_ADS
        if (AdsManager.instance)
            AdsManager.instance.LogAnalyticsEvent("mapselection_to_levelselection");
#endif

        //  mainMenuManager.ShowMenu(MenuNames.LEVEL_SELECTION);
    }

    public void OnClick_UnlockMapWithCash()
    {
        if (mainMenuManager.playerData.playerCash >= mainMenuManager.playerData.maps[mapIndex].unlockPrice)
        {
            mainMenuManager.playerData.playerCash -= mainMenuManager.playerData.maps[mapIndex].unlockPrice;
            mainMenuManager.playerData.maps[mapIndex].isLocked = false;
            mainMenuManager.topBarMenu.Initilize();
            InitializeMap();
        }
        else
        {
            mainMenuManager.ShowPopup(PopupNames.STORE);
        }
    }
}