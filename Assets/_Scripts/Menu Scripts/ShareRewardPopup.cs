using GleyDailyRewards;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ShareRewardPopup : MonoBehaviour
{
    public MainMenuManager mainMenuManager;

    public int rewardCash;
    public Image shareFillBar;
    public Text shareAndEarnText;
    public Image shareAvailableImg;
    public Sprite[] shareAvailableSprite;
    public TimerButtonIDs shareRewardID;

    public Image adFillBar;
    public Image adAvailableImg;
    public Sprite[] adAvailableSprite;

    private bool isAdAvailable;
    private bool isShareAvailable;

    private void OnEnable()
    {
        InitializeTime();
        InitializeAdFill();
    }

    private void InitializeAdFill()
    {
        if (isAdAvailable)
        {
            adFillBar.gameObject.SetActive(true);
            adAvailableImg.sprite = adAvailableSprite[1];
        }
        else
        {
            adFillBar.gameObject.SetActive(false);
            adAvailableImg.sprite = adAvailableSprite[0];
        }
    }

    private void InitializeShareFill()
    {
        if (isShareAvailable)
        {
            shareFillBar.gameObject.SetActive(true);
            shareAndEarnText.text = GleyLocalization.Manager.GetText(WordIDs.InviteandEarnFreeCashID);
            shareAvailableImg.sprite = shareAvailableSprite[1];
        }
        else
        {
            shareFillBar.gameObject.SetActive(false);
            shareAndEarnText.text = GleyLocalization.Manager.GetText(WordIDs.InviteYourFriendsID);
            shareAvailableImg.sprite = shareAvailableSprite[0];
        }
    }

    private void InitializeTime()
    {
        TimerButtonManager.Instance.Initialize(shareRewardID, InitializationComplete);
    }

    private void InitializationComplete(string remainingTime)
    {

        if (TimerButtonManager.Instance.TimeExpired(shareRewardID))
        {
            isShareAvailable = true;
        }
        else
        {
            isShareAvailable = false;
        }
        InitializeShareFill();
    }

    private void ClickResult(bool timeExpired)
    {
        if (timeExpired)
        {
            TimerButtonManager.Instance.Initialize(shareRewardID, InitializationComplete);
        }
    }

    public void OnClick_Close()
    {
        mainMenuManager.OnClick_HandleBackMenu();
    }

    public void OnClick_WatchAd()
    {

    }

    public void OnClick_ShareAndInvite()
    {
        StartCoroutine(TakeScreenshotAndShare());
    }

    private IEnumerator TakeScreenshotAndShare()
    {
        yield return new WaitForEndOfFrame();

        TimerButtonManager.Instance.ButtonClicked(shareRewardID, ClickResult);

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        string filePath = Path.Combine(Application.temporaryCachePath, "HotWHeels.png");
        File.WriteAllBytes(filePath, ss.EncodeToPNG());

        // To avoid memory leaks
        Destroy(ss);

        //new NativeShare().AddFile(filePath)
        //    .SetSubject("Hot Wheels").SetText("Can you Beat my Score?\n").SetUrl("http://play.google.com/store/apps/details?id=" + Application.identifier)
        //    .SetCallback(OnSuccess_ShareCallBack)
        //    //.SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
        //    .Share();

        // Share on WhatsApp only, if installed (Android only)
        //if( NativeShare.TargetExists( "com.whatsapp" ) )
        //	new NativeShare().AddFile( filePath ).AddTarget( "com.whatsapp" ).Share();
    }

    //private void OnSuccess_ShareCallBack(NativeShare.ShareResult shareResult, string shareTarget)
    //{
    //    if (shareResult == NativeShare.ShareResult.NotShared)
    //    {

    //    }
    //    else if (shareResult == NativeShare.ShareResult.Shared)
    //    {
    //        mainMenuManager.playerData.playerCash += rewardCash;
    //        mainMenuManager.topBarMenu.Initilize();
    //        mainMenuManager.OnClick_HandleBackMenu();
    //    }
    //    else if (shareResult == NativeShare.ShareResult.Unknown)
    //    {

    //    }
    //}
}
