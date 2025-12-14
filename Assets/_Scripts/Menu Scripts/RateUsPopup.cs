using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RateUsPopup : MonoBehaviour
{
    public MainMenuManager mainMenuManager;
    public void OnClick_Close()
    {
        mainMenuManager.OnClick_HandleBackMenu();
    }
}
