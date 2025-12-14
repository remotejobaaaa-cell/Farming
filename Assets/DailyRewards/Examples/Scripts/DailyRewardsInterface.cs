/***************************************************************************\
Project:      Daily Rewards
Copyright (c) Niobium Studios.
Author:       Guilherme Nunes Barbosa (gnunesb@gmail.com)
\***************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using DG.Tweening;
using System.Security.Cryptography;

namespace NiobiumStudios
{
    /**
     * The UI Logic Representation of the Daily Rewards
     **/
    public class DailyRewardsInterface : MonoBehaviour
    {
        public MainMenuManager mainMenuManager;
        public GameObject canvas;
        public GameObject dailyRewardPrefab;        // Prefab containing each daily reward

        [Header("Panel Debug")]
		public bool isDebug;
        public GameObject panelDebug;
		public Button buttonAdvanceDay;
		public Button buttonAdvanceHour;
		public Button buttonReset;
		public Button buttonReloadScene;

        [Header("Panel Reward Message")]
        public GameObject panelReward;              // Rewards panel
        public Text textReward;                     // Reward Text to show an explanatory message to the player
        public Button buttonCloseReward;            // The Button to close the Rewards Panel
        public Image imageReward;                   // The image of the reward

        [Header("Panel Reward")]
        public Button buttonClaim;                  // Claim Button
        public Button buttonClose;                  // Close Button
        public Button buttonCloseWindow;            // Close Button on the upper right corner
        public Text textTimeDue;                    // Text showing how long until the next claim
        public GridLayoutGroup dailyRewardsGroup;   // The Grid that contains the rewards
        public ScrollRect scrollRect;               // The Scroll Rect

        private bool readyToClaim;                  // Update flag
        private List<DailyRewardUI> dailyRewardsUI = new List<DailyRewardUI>();

		private DailyRewards dailyRewards;          // DailyReward Instance      
        public Text declineText;
        public GameObject adLogo;        // Prefab containing each daily reward
        void Awake()
        {
            if (mainMenuManager)
            {
                mainMenuManager.ShowMenu(MenuNames.DAILY_REWARD);
             
            }
            else
            {
                mainMenuManager = FindObjectOfType<MainMenuManager>();
                mainMenuManager.ShowMenu(MenuNames.DAILY_REWARD);
            }
            dailyRewards = GetComponent<DailyRewards>();
        }
        
        void Start()
        {
            InitializeDailyRewardsUI();

            if (panelDebug)
                panelDebug.SetActive(isDebug);

            buttonClose.gameObject.SetActive(false);

            buttonClaim.onClick.AddListener(() =>
            {
                if (AudioManager.instance)
                    AudioManager.instance.Play_ClickButtonSound();
				dailyRewards.ClaimPrize();
                readyToClaim = false;
                UpdateUI();
            });

            buttonCloseReward.onClick.AddListener(() =>
            {
                if (AudioManager.instance)
                    AudioManager.instance.Play_ClickButtonSound(); 
                var keepOpen = dailyRewards.keepOpen;
                panelReward.SetActive(false);
                // canvas.gameObject.SetActive(keepOpen);
                if(rewardIsCoin)
               mainMenuManager.topBarMenu.GiveRewad(rewardCoin);
              mainMenuManager.OnClick_HandleBackMenu();
#if HAVE_ADS
                if (AdsManager.instance)
                {
                    AdsManager.instance.LogAnalyticsEvent("daily_reward_collect");
                    AdsManager.instance.LogAnalyticsEvent("daily_reward_close");
                }
#endif

            });

            buttonClose.onClick.AddListener(() =>
            {
#if HAVE_ADS
                //if (AdsManager.instance && AdsManager.instance.IsIntertitialAvaliable() && !AdsManager.instance.isRemoveAd())
                //    AdsManager.instance.ShowInterstitialAd(Method);
                //else
                    Method();
                Debug.Log("IsRemoveAd " + AdsManager.instance.isRemoveAd());
#else
                Method();
#endif

            });

            buttonCloseWindow.onClick.AddListener(() =>
            {
                if (AudioManager.instance)
                    AudioManager.instance.Play_ClickButtonSound();
                canvas.gameObject.SetActive(false);
            });

            // Simulates the next Day
            if (buttonAdvanceDay)
				buttonAdvanceDay.onClick.AddListener(() =>
				{
                    if (AudioManager.instance)
                        AudioManager.instance.Play_ClickButtonSound(); ShowRewardedAdForSkipTheDay();
                    //dailyRewards.debugTime = dailyRewards.debugTime.Add(new TimeSpan(1, 0, 0, 0));
                    //UpdateUI();
				});

			// Simulates the next hour
			if(buttonAdvanceHour)
				buttonAdvanceHour.onClick.AddListener(() =>
              	{
                      dailyRewards.debugTime = dailyRewards.debugTime.Add(new TimeSpan(1, 0, 0));
                      UpdateUI();
				});

			if(buttonReset)
				// Resets Daily Rewards from Player Preferences
				buttonReset.onClick.AddListener(() =>
				{
					dailyRewards.Reset();
                    dailyRewards.debugTime = new TimeSpan();
                    dailyRewards.lastRewardTime = System.DateTime.MinValue;
					readyToClaim = false;
				});
			if(buttonReloadScene)
				// Reloads the same scene
				buttonReloadScene.onClick.AddListener(() =>
				{
					Application.LoadLevel(Application.loadedLevelName);
				});

			UpdateUI();
        }
        public void Method()
        {
            if (mainMenuManager)
            StartCoroutine(mainMenuManager.Loading());
            else
            {
                MainMenuManager mainMenuManager = FindObjectOfType<MainMenuManager>();
                StartCoroutine(mainMenuManager.Loading());
            }
#if HAVE_ADS
            if (AdsManager.instance)
                AdsManager.instance.LogAnalyticsEvent("daily_reward_close");
#endif
        }
        public void ShowRewardedAdForSkipTheDay()
        {
#if HAVE_ADS
            if (AdsManager.instance && AdsManager.instance.isAdmobRewardedVideoAvailable())
            {
                AdsManager.instance.ShowRewardedAD(SkipTheDay);
            }
            else
#endif
            {
                declineText.gameObject.SetActive(true);
                declineText.GetComponent<DOTweenAnimation>().DORestart();
            }
               

        }
        public void SkipTheDay()
        {
            dailyRewards.debugTime = dailyRewards.debugTime.Add(new TimeSpan(1, 0, 0, 0));
            UpdateUI();
        }
        void OnEnable()
        {
            dailyRewards.onClaimPrize += OnClaimPrize;
            dailyRewards.onInitialize += OnInitialize;
#if HAVE_ADS
            if (adLogo)
            {
                if (AdsManager.instance && AdsManager.instance.IsIntertitialAvaliable())
                    adLogo.SetActive(true);
                else
                    adLogo.SetActive(false);
            }
            if (AdsManager.instance)
                AdsManager.instance.LogAnalyticsEvent("daily_reward_open");
#else
             if (adLogo)
    {
        adLogo.SetActive(false);  // Ensure adLogo is hidden if ads are disabled.
    }
#endif

        }

        void OnDisable()
        {
            if (dailyRewards != null)
            {
                dailyRewards.onClaimPrize -= OnClaimPrize;
                dailyRewards.onInitialize -= OnInitialize;
            }
        }

        // Initializes the UI List based on the rewards size
        private void InitializeDailyRewardsUI()
        {
            for (int i = 0; i < dailyRewards.rewards.Count; i++)
            {
                int day = i + 1;
                var reward = dailyRewards.GetReward(day);

                GameObject dailyRewardGo = GameObject.Instantiate(dailyRewardPrefab) as GameObject;

                DailyRewardUI dailyRewardUI = dailyRewardGo.GetComponent<DailyRewardUI>();
                dailyRewardUI.transform.SetParent(dailyRewardsGroup.transform);
                dailyRewardGo.transform.localScale = Vector2.one;

                dailyRewardUI.day = day;
                dailyRewardUI.reward = reward;
                dailyRewardUI.Initialize();

                dailyRewardsUI.Add(dailyRewardUI);
            }
        }

        public void UpdateUI()
        {
            dailyRewards.CheckRewards();

            bool isRewardAvailableNow = false;

            var lastReward = dailyRewards.lastReward;
            var availableReward = dailyRewards.availableReward;

            foreach (var dailyRewardUI in dailyRewardsUI)
            {
                var day = dailyRewardUI.day;

                if (day == availableReward)
                {
                    dailyRewardUI.state = DailyRewardUI.DailyRewardState.UNCLAIMED_AVAILABLE;

                    isRewardAvailableNow = true;
                }
                else if (day <= lastReward)
                {
                    dailyRewardUI.state = DailyRewardUI.DailyRewardState.CLAIMED;
                }
                else
                {
                    dailyRewardUI.state = DailyRewardUI.DailyRewardState.UNCLAIMED_UNAVAILABLE;
                }

                dailyRewardUI.Refresh();
            }

            buttonClaim.gameObject.SetActive(isRewardAvailableNow);
            buttonClose.gameObject.SetActive(!isRewardAvailableNow);
            if (isRewardAvailableNow)
            {
                SnapToReward();
                textTimeDue.text = "You can claim your reward!";
            }
            readyToClaim = isRewardAvailableNow;
        }

        // Snap to the next reward
        public void SnapToReward()
        {
            Canvas.ForceUpdateCanvases();

            var lastRewardIdx = dailyRewards.lastReward;

            // Scrolls to the last reward element
            if (dailyRewardsUI.Count - 1 < lastRewardIdx)
                lastRewardIdx++;

			if(lastRewardIdx > dailyRewardsUI.Count - 1)
				lastRewardIdx = dailyRewardsUI.Count - 1;

            var target = dailyRewardsUI[lastRewardIdx].GetComponent<RectTransform>();

            var content = scrollRect.content;

            //content.anchoredPosition = (Vector2)scrollRect.transform.InverseTransformPoint(content.position) - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);

            float normalizePosition = (float)target.GetSiblingIndex() / (float)content.transform.childCount;
            scrollRect.verticalNormalizedPosition = normalizePosition;
        }

        void Update()
        {
            dailyRewards.TickTime();
            // Updates the time due
            CheckTimeDifference();
        }

        private void CheckTimeDifference ()
        {
            if (!readyToClaim)
            {
                TimeSpan difference = dailyRewards.GetTimeDifference();

                // If the counter below 0 it means there is a new reward to claim
                if (difference.TotalSeconds <= 0)
                {
                    readyToClaim = true;
                    UpdateUI();
                    SnapToReward();
                    return;
                }

                string formattedTs = dailyRewards.GetFormattedTime(difference);

                textTimeDue.text = string.Format("Come back in {0} for your next reward", formattedTs);
                    buttonAdvanceDay.gameObject.SetActive(true);
            }else
                buttonAdvanceDay.gameObject.SetActive(false);

        }
        // Delegate
        bool rewardIsCoin;
        int rewardCoin;
        private void OnClaimPrize(int day)
        {
            panelReward.SetActive(true);

            var reward = dailyRewards.GetReward(day);
            var unit = reward.unit;
            var rewardQt = reward.reward;
            imageReward.sprite = reward.sprite;

            if (rewardQt > 0)
            {
                if (reward.unit == "Coins")
                {
                    rewardIsCoin = true;
                    if (reward.reward == 500)
                    {
                        rewardCoin = 500;
                       // mainMenuManager.GetCoins(500);
                    } 
                   else if(reward.reward == 1000)
                    {
                        rewardCoin = 1000;
                       // mainMenuManager.GetCoins(1000);
                    } 
                    else if(reward.reward == 3000)
                    {
                        rewardCoin = 3000;
                      //  mainMenuManager.GetCoins(3000);
                    }
                    else if(reward.reward == 5000)
                    {
                        rewardCoin =5000;
                        //mainMenuManager.GetCoins(5000);
                    }
                textReward.text = string.Format("You got {0} {1}!", reward.reward, unit);
                }
            }
            else
            {
                    rewardIsCoin = false;
                if (reward.unit == "Models")
                {
                textReward.text = string.Format("You got New Models");
                    mainMenuManager.Purchased_Farmers();
                }
                else if (reward.unit == "Remove Ads")
                {
                textReward.text = string.Format("You got Remove Ads From Game");
                    mainMenuManager.Purchased_RemoveAds();
                }
                else if (reward.unit == "Levels")
                {
                textReward.text = string.Format("You got All Levels Unlocked");
                    mainMenuManager.Purchased_Levels();
                }
            }

            if (AudioManager.instance)
            {
                AudioManager.instance.Play_ClickConfettiSound();
                AudioManager.instance.Play_ClickInGameRewardSound();
            }
            if(mainMenuManager)
            StartCoroutine(mainMenuManager. ConfettiOnOffObject());

          








            //if (rewardQt > 0)
            //{
            //    textReward.text = string.Format("You got {0} {1}!", reward.reward, unit);
            //}
            //else
            //{
            //    textReward.text = string.Format("You got {0}!", unit);
            //}
        }

        private void OnInitialize(bool error, string errorMessage)
        {
            if (!error)
            {
                var showWhenNotAvailable = dailyRewards.keepOpen;
                var isRewardAvailable = dailyRewards.availableReward > 0;

                UpdateUI();
               // canvas.gameObject.SetActive(showWhenNotAvailable || (!showWhenNotAvailable && isRewardAvailable));

                SnapToReward();
                CheckTimeDifference();
            }
        }
    }
}