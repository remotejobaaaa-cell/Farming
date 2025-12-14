using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TopBarMenuManager : MonoBehaviour
{
    public MainMenuManager mainMenuMenu;
    public GameObject noAdsBtn;
    public GameObject UnlockAllGameBtn;
    public GameObject coinAttractionParticle;

    public Text cashText;
    private void OnEnable()
    {
        Initilize();
    }
    public void Initilize()
    {
        cashText.text = mainMenuMenu.playerData.playerCash.ToString();
        IAPInitialize();  }
    public void IAPInitialize()
    { 
        if(mainMenuMenu.playerData.noAds == 3)
        {
            noAdsBtn.transform.GetChild(0).gameObject.SetActive(false);
#if HAVE_ADS
            if (AdsManager.instance != null)
                AdsManager.instance.HideRectBanner();
#endif
        }
        else
        {
            noAdsBtn.transform.GetChild(0).gameObject.SetActive(true);
        }

        if (mainMenuMenu.playerData.unlockAllGame == 3)
        {
            UnlockAllGameBtn.transform.gameObject.SetActive(false);
        }
        else
        {
            UnlockAllGameBtn.transform.gameObject.SetActive(true);
        }
    }
    public void OnClick_OpenSettings()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
        mainMenuMenu.ShowPopup(PopupNames.SETTINGS);
    }

    public void OnClick_MoreGames()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
        Application.OpenURL(GameConstants.moreGamesURL);
    }

    public void OnClick_Back()
    {
        mainMenuMenu.OnClick_HandleBackMenu();
    }
    public IEnumerator CoinAttractionParticleOnOffPanel(int coins)
    {
        coinAttractionParticle.SetActive(true);
        if (AudioManager.instance)
            AudioManager.instance.Play_ClickCoinsSound();
        yield return new WaitForSeconds(1.5f);

        // Calculate the number of frames for 3 seconds based on the desired frame rate
        int totalFrames = Mathf.CeilToInt(1 / Time.fixedDeltaTime);

        // Calculate the coins to add per frame based on the total number of coins and frames
        int coinsToAddPerFrame = Mathf.CeilToInt((float)coins / totalFrames);

        int remainingCoins = coins;

        while (remainingCoins > 0)
        {
            int coinsToAdd = Mathf.Min(coinsToAddPerFrame, remainingCoins);
            mainMenuMenu.playerData.playerCash += coinsToAdd;
            remainingCoins -= coinsToAdd;

            // Optionally, you can add visual effects or sound effects here to indicate coin addition
            cashText.text = mainMenuMenu.playerData.playerCash.ToString();

            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(4.5f);
        coinAttractionParticle.SetActive(false);
        PlayerDataController.instance.Save();
    } 
    
  public void GiveRewad(int coinsToAdd)
    {
            mainMenuMenu.playerData.playerCash += coinsToAdd;
        cashText.text = mainMenuMenu.playerData.playerCash.ToString();

    }
}
