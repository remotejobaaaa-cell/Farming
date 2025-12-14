using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MTAssets.EasyMinimapSystem;

public class GameplayMenuManager : MonoBehaviour
{
    public HudMenuManager hudMenu;
    [Header("Minimap")]
    public MinimapRenderer minimap;
    public MinimapRoutes minimapRoute;

    [Header("Field System")]
    public Animator fieldInfoAnim;
    public Text fieldOwnerNameText;
    public Text fieldTypeNameText;

    [Header("Info Text")]
    public Slider progressBar;
    public GameObject infoPopup;
    public Text inforPopupText;

    [Header("Dialogue System")]
    public DialogueSystem dialogueSystem;

    [Space()]
    public GameObject dimmer;

    [HideInInspector]
    public HarvesterState harvesterState;

    private void Start()
    {
#if HAVE_ADS
        if (AdsManager.instance)
        {
            AdsManager.instance.HideAllBanners();
            AdsManager.instance.ShowBannerAdRight();
            AdsManager.instance.ShowBannerAdLeft();
        }
#endif
    }
    public void OnClick_Pause()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.backgroundMusicAudioSource.Stop();
        }
        hudMenu.PauseGame();

       
    }

    public void On_ClickHarvester()
    {
        if (harvesterState == HarvesterState.CutterOff)
        {
            harvesterState = HarvesterState.CutterOn;
            
            hudMenu.rccMobileButtons.EnableButtons();
            hudMenu.rccMobileButtons.HarvesterControl.transform.GetChild(0).gameObject.SetActive(false);
            hudMenu.playerSpanwer.rccCarControllerV3.StartEngine();
            hudMenu.playerSpanwer.cropFieldController.HarvesterCutterOn(true);
        }
        else
        {
            harvesterState = HarvesterState.CutterOff;
            hudMenu.playerSpanwer.cropFieldController.HarvesterCutterOn(false);
        }

    }
    public void GameOverComplete(int time)
    {
        GameConstants.isPlayerWin = true;
        hudMenu.GameOver(time);
    }
}
