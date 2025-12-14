using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraControls : MonoBehaviour
{
    private void Awake()
    {
        HudMenuManager.instance.playerSpanwer.currentCamera = this.gameObject;
    }
}
