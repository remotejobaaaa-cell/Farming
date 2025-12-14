using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingGameOverState : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameConstants.isPlayerWin = true;
            HudMenuManager.instance.GameOver(1);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            GameConstants.isPlayerWin = false;
            HudMenuManager.instance.GameOver(1);
        }
    }
}
    