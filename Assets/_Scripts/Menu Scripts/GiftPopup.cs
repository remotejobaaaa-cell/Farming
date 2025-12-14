using UnityEngine;
using GleyDailyRewards;
using UnityEngine.UI;
using System;

public class GiftPopup : MonoBehaviour
{
    public MainMenuManager mainMenuManager;

    public int giftCounter;

    public TimerButtonIDs buttonID;
    public Button giftButton;
    public Text giftCounterText;
    public Text nextGiftTime;
    public Sprite[] giftButtonSprite;

    private float currentTime;
    private bool initialized;

    const float refreshTime = 0.3f;

    private void OnEnable()
    {
        giftCounter = mainMenuManager.playerData.giftCounter;
    }

    void Start()
    {
        //Initialize the current button
        TimerButtonManager.Instance.Initialize(buttonID, InitializationComplete);
    }

    private void OnDisable()
    {
        mainMenuManager.playerData.giftCounter = giftCounter;
        PlayerDataController.instance.Save();
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
        nextGiftTime.text = "Next Gift in " + remainingTime;
        //giftButton.interactable = interactable;
        //if (interactable)
        //{
        //    giftButton.image.sprite = giftButtonSprite[0];
        //}
        //else
        //{
        //    giftButton.image.sprite = giftButtonSprite[1];
        //}
        //giftButton.image.SetNativeSize();
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
        nextGiftTime.text = "Next Gift in " + TimerButtonManager.Instance.GetRemainingTime(buttonID);
        RefreshGiftText();

        if (TimerButtonManager.Instance.TimeExpired(buttonID))
        {
            if (giftCounter < 1)
                giftCounter++;
            RefreshGiftText();
            nextGiftTime.text = "";
            giftButton.interactable = true;
            giftButton.image.SetNativeSize();
            giftButton.image.sprite = giftButtonSprite[0];
            initialized = false;
        }
        else
        {
            initialized = true;
        }
    }

    private void RefreshGiftText()
    {
        giftCounterText.text = "Gifts Remaining : " + giftCounter;
    }

    /// <summary>
    /// Listener triggered when button is clicked
    /// </summary>
    public void OnClick_ClaimGift()
    {
        TimerButtonManager.Instance.ButtonClicked(buttonID, ClickResult);
    }


    /// <summary>
    /// Reset the button state if clicked and the reward was collected
    /// </summary>
    /// <param name="timeExpired"></param>
    private void ClickResult(bool timeExpired)
    {
        if (giftCounter > 0)
        {
            if (giftCounter == 1)
            {
                if (timeExpired)
                {
                    TimerButtonManager.Instance.Initialize(buttonID, InitializationComplete);
                    giftCounter--;
                }
            }
            else
            {
                giftCounter--;
            }
        }
        RefreshGiftText();
    }

    public void OnClick_WatchAdForGift()
    {
        if (giftCounter == 0)
        {
            TimerButton.RemoveTime(TimerButtonIDs.HourlyGift, new TimeSpan(0, 60, 0));
        }
        else
        {
            giftCounter++;
            if (giftCounter > mainMenuManager.playerData.maxGifts)
            {
                giftCounter = mainMenuManager.playerData.maxGifts;
            }
        }
        RefreshGiftText();
    }

    public void OnClick_Close()
    {
        mainMenuManager.OnClick_HandleBackMenu();
    }

    public void OnClick_RemoveTime()
    {
        GleyDailyRewards.TimerButton.RemoveTime(TimerButtonIDs.HourlyGift, new TimeSpan(60, 0, 0));
    }
}
