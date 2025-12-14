using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GleyLocalization;

public class FreeCashSocialPopup : MonoBehaviour
{
    public MainMenuManager mainMenuManager;

    public int rewardCash;

    public GameObject companyYoutubeRewardCashIcon;
    public Text companyYoutubeReward;
    public GameObject companyFacebookRewardCashIcon;
    public Text companyFacebookReward;
    public GameObject companyInstagramRewardCashIcon;
    public Text companyInstagramReward;
    public GameObject gameInstagramRewardCashIcon;
    public Text gameInstagramReward;
    public GameObject companyTwitterRewardCashIcon;
    public Text companyTwitterReward;
    public GameObject companyLinkedInRewardCashIcon;
    public Text companyLinkedInReward;
    public GameObject watchAdRewardCashIcon;
    public Text watchAdReward;
    public ScrollRect scrollRect;

    private bool isRewardPending;

    private void OnEnable()
    {
        Initilize();
        scrollRect.verticalNormalizedPosition = 1f;
    }

    private void Initilize()
    {
        companyYoutubeReward.text = Manager.GetText(WordIDs.EarnID)+ " " + mainMenuManager.playerData.socialReward + " " + Manager.GetText(WordIDs.CashID);
        companyFacebookReward.text = Manager.GetText(WordIDs.EarnID) + " " + mainMenuManager.playerData.socialReward + " " + Manager.GetText(WordIDs.CashID);
        companyInstagramReward.text = Manager.GetText(WordIDs.EarnID) + " " + mainMenuManager.playerData.socialReward + " " + Manager.GetText(WordIDs.CashID);
        gameInstagramReward.text = Manager.GetText(WordIDs.EarnID) + " " + mainMenuManager.playerData.socialReward + " " + Manager.GetText(WordIDs.CashID);
        companyTwitterReward.text = Manager.GetText(WordIDs.EarnID) + " " + mainMenuManager.playerData.socialReward + " " + Manager.GetText(WordIDs.CashID);
        companyLinkedInReward.text = Manager.GetText(WordIDs.EarnID) + " " + mainMenuManager.playerData.socialReward + " " + Manager.GetText(WordIDs.CashID);
        watchAdReward.text = Manager.GetText(WordIDs.EarnID) + " " + mainMenuManager.playerData.socialReward + " " + Manager.GetText(WordIDs.CashID);

        if (mainMenuManager.playerData.isCompanyYoutubeClicked)
        {
            companyYoutubeReward.gameObject.SetActive(false);
            companyYoutubeRewardCashIcon.gameObject.SetActive(false);
        }
        else
        {
            companyYoutubeReward.gameObject.SetActive(true);
            companyYoutubeRewardCashIcon.gameObject.SetActive(true);
        }

        if (mainMenuManager.playerData.isCompanyFacebookClicked)
        {
            companyFacebookReward.gameObject.SetActive(false);
            companyFacebookRewardCashIcon.gameObject.SetActive(false);
        }
        else
        {
            companyFacebookReward.gameObject.SetActive(true);
            companyFacebookRewardCashIcon.gameObject.SetActive(true);
        }

        if (mainMenuManager.playerData.isCompanyTwitterClicked)
        {
            companyTwitterReward.gameObject.SetActive(false);
            companyTwitterRewardCashIcon.gameObject.SetActive(false);
        }
        else
        {
            companyTwitterReward.gameObject.SetActive(true);
            companyTwitterRewardCashIcon.gameObject.SetActive(true);
        }

        if (mainMenuManager.playerData.isCompanyInstagaramClicked)
        {
            companyInstagramReward.gameObject.SetActive(false);
            companyInstagramRewardCashIcon.gameObject.SetActive(false);
        }
        else
        {
            companyInstagramReward.gameObject.SetActive(true);
            companyInstagramRewardCashIcon.gameObject.SetActive(true);
        }

        if (mainMenuManager.playerData.isCompanyLinkedInClicked)
        {
            companyLinkedInReward.gameObject.SetActive(false);
            companyLinkedInRewardCashIcon.gameObject.SetActive(false);
        }
        else
        {
            companyLinkedInReward.gameObject.SetActive(true);
            companyLinkedInRewardCashIcon.gameObject.SetActive(true);
        }

        if (mainMenuManager.playerData.isCompanyTwitterClicked)
        {
            companyTwitterReward.gameObject.SetActive(false);
            companyTwitterRewardCashIcon.gameObject.SetActive(false);
        }
        else
        {
            companyTwitterReward.gameObject.SetActive(true);
            companyTwitterRewardCashIcon.gameObject.SetActive(true);
        }

        if (mainMenuManager.playerData.isGameInstagaramClicked)
        {
            gameInstagramReward.gameObject.SetActive(false);
            gameInstagramRewardCashIcon.gameObject.SetActive(false);
        }
        else
        {
            gameInstagramReward.gameObject.SetActive(true);
            gameInstagramRewardCashIcon.gameObject.SetActive(true);
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            if (isRewardPending)
            {
                isRewardPending = false;
                mainMenuManager.playerData.playerCash += rewardCash;
                mainMenuManager.topBarMenu.Initilize();
            }
        }
    }

    public void OnClick_Close()
    {
        mainMenuManager.OnClick_HandleBackMenu();
    }

    public void OnClick_CompanyYoutube()
    {
        if (!mainMenuManager.playerData.isCompanyYoutubeClicked)
        {
            mainMenuManager.playerData.isCompanyYoutubeClicked = true;
            isRewardPending = true;
            Initilize();
        }
        else
        {
            isRewardPending = false;
        }
        Application.OpenURL(GameConstants.companyYoutube);
    }

    public void OnClick_CompanyFacebook()
    {
        if (!mainMenuManager.playerData.isCompanyFacebookClicked)
        {
            mainMenuManager.playerData.isCompanyFacebookClicked = true;
            isRewardPending = true;
            Initilize();
        }
        else
        {
            isRewardPending = false;
        }
        Application.OpenURL(GameConstants.companyFacebook);
    }

    public void OnClick_CompanyInstagram()
    {
        if (!mainMenuManager.playerData.isCompanyInstagaramClicked)
        {
            mainMenuManager.playerData.isCompanyInstagaramClicked = true;
            isRewardPending = true;
            Initilize();
        }
        else
        {
            isRewardPending = false;
        }
        Application.OpenURL(GameConstants.companyInstagaram);
    }

    public void OnClick_CompanyTwitter()
    {
        if (!mainMenuManager.playerData.isCompanyTwitterClicked)
        {
            mainMenuManager.playerData.isCompanyTwitterClicked = true;
            isRewardPending = true;
            Initilize();
        }
        else
        {
            isRewardPending = false;
        }
        Application.OpenURL(GameConstants.companyTwitter);
    }

    public void OnClick_CompanyLinkedIn()
    {
        if (!mainMenuManager.playerData.isCompanyLinkedInClicked)
        {
            mainMenuManager.playerData.isCompanyLinkedInClicked = true;
            isRewardPending = true;
            Initilize();
        }
        else
        {
            isRewardPending = false;
        }
        Application.OpenURL(GameConstants.companyLinkedIn);
    }

    public void OnClick_GameInstagram()
    {
        if (!mainMenuManager.playerData.isGameInstagaramClicked)
        {
            mainMenuManager.playerData.isGameInstagaramClicked = true;
            isRewardPending = true;
            Initilize();
        }
        else
        {
            isRewardPending = false;
        }
        Application.OpenURL(GameConstants.gameInstagaram);
    }

    public void OnClick_WatchAd()
    {

    }
}
