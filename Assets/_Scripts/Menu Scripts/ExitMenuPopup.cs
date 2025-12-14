using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitMenuPopup : MonoBehaviour
{
    public void OnClick_ExitGame()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play_ClickButtonSound();
        }
        Application.Quit();
    }
}
