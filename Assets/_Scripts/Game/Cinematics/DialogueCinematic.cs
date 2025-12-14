using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCinematic : MonoBehaviour
{
    private HudMenuManager hudMenu;

    private void OnEnable()
    {
        if (!hudMenu)
        {
            hudMenu = HudMenuManager.instance;
        }
        if (hudMenu.playerSpanwer.currentCamera)
        {
            hudMenu.playerSpanwer.currentCamera.SetActive(false);
        }

    }
    private void OnDisable()
    {
        if (hudMenu.playerSpanwer.currentCamera)
        {
            hudMenu.playerSpanwer.currentCamera.SetActive(true);
        }
    }
}
