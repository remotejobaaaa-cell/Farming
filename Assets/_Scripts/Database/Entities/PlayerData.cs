using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class PlayerData
{
    public float playerCash;
    public int currentPlayerLevel;
    public int rateUS;
    public int noAds;
    public int unlockAllLevels;
    public int unlockAllFarmers;
    public int unlockAllGame;
    public int playerExperience;

    public string playerName;

    public float totalCashEarned;
    public float totalCashSpend;

    public int routeCompleted;
    public int regionsUnlocked;

    public List<PlayerFarmer> farmers;
    public int currentlySelectedFarmer;

    public List<Map> maps;
    public int currentlySelectedMap;

    public int giftCounter;
    public int maxGifts = 10;
    public int luckySpinCounter;
    public int maxSpin = 5;

    public bool isCompanyYoutubeClicked;
    public bool isCompanyFacebookClicked;
    public bool isCompanyInstagaramClicked;
    public bool isCompanyTwitterClicked;
    public bool isCompanyLinkedInClicked;
    public bool isGameInstagaramClicked;
    public int socialReward = 150;

    public bool isBackgroundMusicOn = true;
    public bool isSoundFxOn = true;
    public bool isMirrorOn = true;
    public ControlsType controlsType = ControlsType.Buttons;
    public int buttonControlLayoutIndex;
    public int steeringControlLayoutIndex;
    public int tiltControlLayoutIndex;
    public Quality quality = Quality.Low;
    public float gameVolume = 0.5f;
    public float cameraSensitivity = 0.5f;

    public bool isAdsPurchased;
    public bool isBoosterPurchased;
}
