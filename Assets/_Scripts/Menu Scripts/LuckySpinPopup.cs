using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyUI.PickerWheelUI;
using GleyDailyRewards;
using System;

public class LuckySpinPopup : MonoBehaviour
{
    [SerializeField] private MainMenuManager mainMenuManager;

    [SerializeField] private int luckySpinCounter;
    [SerializeField] private TimerButtonIDs buttonID;

    [SerializeField] private Button closeButton;
    [SerializeField] private Button uiSpinButton;
    [SerializeField] private Sprite[] uiSpinButtonSprites;
    [SerializeField] private Text uiSpinButtonText;
    [SerializeField] private Text luckySpinCounterText;
    [SerializeField] private Text nextLuckySpinInfo;
    [SerializeField] private Text nextLuckySpinTime;
    [SerializeField] private PickerWheel pickerWheel;

    private float currentTime;
    private bool initialized;

    const float refreshTime = 0.3f;

    private void OnEnable()
    {
        closeButton.interactable = true;
        uiSpinButton.interactable = true;
        uiSpinButton.image.sprite = uiSpinButtonSprites[1];
        luckySpinCounter = mainMenuManager.playerData.luckySpinCounter;
    }

    private void Start()
    {
        TimerButtonManager.Instance.Initialize(buttonID, InitializationComplete);
    }

    public void OnClick_SpinWheel()
    {
        TimerButtonManager.Instance.ButtonClicked(buttonID, ClickResult);
    }

    private void StartSpin()
    {
        closeButton.interactable = false;
        uiSpinButton.interactable = false;
        uiSpinButton.image.sprite = uiSpinButtonSprites[0];
        uiSpinButtonText.text = "Spinning";
        pickerWheel.OnSpinEnd(OnSpinEnd);
        pickerWheel.Spin();
        uiSpinButtonText.text= GleyLocalization.Manager.GetText(WordIDs.SpinID);

    }

    private void OnSpinEnd(WheelPiece wheelPiece)
    {
        Debug.Log("Index: " + wheelPiece.Index + " Label: " + wheelPiece.Label + " Amount: " + wheelPiece.Amount + " Chance: " + wheelPiece.Chance + "%");
        closeButton.interactable = true;
        uiSpinButton.interactable = true;
        uiSpinButton.image.sprite = uiSpinButtonSprites[1];
        uiSpinButtonText.text = GleyLocalization.Manager.GetText(WordIDs.SpinID);
    }

    /// <summary>
    /// Setup the button
    /// </summary>
    /// <param name="remainingTime">time until ready</param>
    /// <param name="interactable">is button clickable</param>
    /// <param name="completeText">the text that appears after timer is done</param>
    private void InitializationComplete(string remainingTime)
    {
        // TODO: Implement Animation
        nextLuckySpinInfo.gameObject.SetActive(true);
        nextLuckySpinTime.text = remainingTime;
        RefreshButton();
    }

    /// <summary>
    /// refresh button text
    /// </summary>
    void Update()
    {
        if (initialized)
        {
            currentTime += Time.deltaTime;
            if (currentTime > refreshTime)
            {
                currentTime = 0;
                RefreshButton();
            }
        }
    }

    /// <summary>
    /// update button appearance
    /// </summary>
    private void RefreshButton()
    {
        nextLuckySpinInfo.gameObject.SetActive(true);
        nextLuckySpinTime.text = TimerButtonManager.Instance.GetRemainingTime(buttonID);
        RefreshLuckySpinText();

        if (TimerButtonManager.Instance.TimeExpired(buttonID))
        {
            if (luckySpinCounter < 1)
                luckySpinCounter++;
            RefreshLuckySpinText();
            nextLuckySpinInfo.gameObject.SetActive(false);
            nextLuckySpinTime.text = "";
            uiSpinButton.interactable = true;
            uiSpinButton.image.SetNativeSize();
            initialized = false;
        }
        else
        {
            initialized = true;
        }
    }

    private void RefreshLuckySpinText()
    {
        luckySpinCounterText.text = luckySpinCounter + " / " + mainMenuManager.playerData.maxSpin;
    }

    /// <summary>
    /// Reset the button state if clicked and the reward was collected
    /// </summary>
    /// <param name="timeExpired"></param>
    private void ClickResult(bool timeExpired)
    {
        if (luckySpinCounter > 0)
        {
            if (luckySpinCounter == 1)
            {
                if (timeExpired)
                {
                    TimerButtonManager.Instance.Initialize(buttonID, InitializationComplete);
                    luckySpinCounter--;
                    StartSpin();
                }
            }
            else
            {
                luckySpinCounter--;
                StartSpin();
            }
        }
        RefreshLuckySpinText();
    }

    public void OnClick_WatchAdForLuckySpin()
    {
        if (luckySpinCounter == 0)
        {
            TimerButton.RemoveTime(TimerButtonIDs.LuckySpinWheel, new TimeSpan(0, 60, 0));
        }
        else
        {
            luckySpinCounter++;
            if (luckySpinCounter > mainMenuManager.playerData.maxSpin)
            {
                luckySpinCounter = mainMenuManager.playerData.maxSpin;
            }
        }
        RefreshLuckySpinText();
    }

    public void OnClick_Close()
    {
        mainMenuManager.OnClick_HandleBackMenu();
    }

    public void OnClick_RemoveTime()
    {
        TimerButton.RemoveTime(TimerButtonIDs.LuckySpinWheel, new TimeSpan(60, 0, 0));
    }

    private void OnDisable()
    {
        mainMenuManager.playerData.luckySpinCounter = luckySpinCounter;
        PlayerDataController.instance.Save();
    }
}
