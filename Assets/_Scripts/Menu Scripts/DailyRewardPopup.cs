using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GleyDailyRewards;

public class DailyRewardPopup : MonoBehaviour
{
    public MainMenuManager mainMenuManager;

    public CalendarPopup calendarPopup;

    private void Start()
    {
        Calendar.AddClickListener(OnClick_Reward);
    }

    public void OnClick_Close()
    {
        mainMenuManager.OnClick_HandleBackMenu();
    }

    private void OnClick_Reward(int dayNumber, string rewardValue)
    {
        Debug.Log("Click " + dayNumber + " " + rewardValue);
        Debug.Log("Your reward is " + rewardValue + " on Day " + rewardValue);

        Debug.Log(dayNumber);
        switch (dayNumber)
        {
            case 1:
                Debug.Log("Day 1 Reward is collected");
                mainMenuManager.rewardCollectorPopup.CollectGold(200);
                mainMenuManager.ShowPopup(PopupNames.REWARD_COLLECTOR);
                break;
            case 2:
                Debug.Log("Day 2 Reward is collected");
                break;
            case 3:
                Debug.Log("Day 3 Reward is collected");
                break;
            case 4:
                Debug.Log("Day 4 Reward is collected");
                break;
            case 5:
                Debug.Log("Day 5 Reward is collected");
                break;
            case 6:
                Debug.Log("Day 6 Reward is collected");
                break;
            case 7:
                Debug.Log("Day 7 Reward is collected");
                break;
            default:
                break;
        }
    }
}
