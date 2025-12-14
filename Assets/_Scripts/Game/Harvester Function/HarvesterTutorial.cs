using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HarvesterTutorial : MonoBehaviour
{
    public RCC_MobileButtons RCCMobileButtons;
    private void OnEnable()
    {
        HudMenuManager.instance.playerSpanwer.rccCarControllerV3.KillEngine();
        RCCMobileButtons.DisableButtons();
        this.GetComponent<DOTweenAnimation>().DOPlay();
    }
}
