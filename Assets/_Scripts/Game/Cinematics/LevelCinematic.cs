using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCinematic : MonoBehaviour
{
    public LevelManager levelManager;

    public float completeTime = 5;
    private GameObject currentPlayerCamera;

    private void OnEnable()
    {
        if (HudMenuManager.instance.playerSpanwer.currentCamera)
        {
            HudMenuManager.instance.playerSpanwer.currentCamera.SetActive(false);
        }

        if (HudMenuManager.instance)
        {
            HudMenuManager.instance.gameplayMenu.gameObject.SetActive(false);
        }
        Invoke("OnFinishCinematic", completeTime);
    }

    public void OnFinishCinematic()
    {
        levelManager.CutSceneCompleted();
    }
    
    private void OnDisable()
    {
        if (HudMenuManager.instance.playerSpanwer.currentCamera)
        {
            HudMenuManager.instance.playerSpanwer.currentCamera.SetActive(true);
        }
    }
}
