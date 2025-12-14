using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AskRateUs : MonoBehaviour
{
    // Start is called before the first frame update
  public  void OpenLink()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
        Application.OpenURL("http://play.google.com/store/apps/details?id=" + Application.identifier);
        PlayerDataController.instance.playerData.rateUS = 3;
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void CloseRateUs()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
        PlayerDataController.instance.playerData.rateUS = 3;
        this.gameObject.SetActive(false);
    }
}
