using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Invector.vCharacterController;


public class TriggerAction : MonoBehaviour
{
    public UnityEvent OnTriggerEnterAction;
    public UnityEvent DroneOnTriggerEnterAction;
    public UnityEvent OnTriggerExitAction;
    public UnityEvent DroneOnTriggerExitAction;

   public void NavmeshPathDraw()
    {
        if (HudMenuManager.instance != null)
        {
            HudMenuManager.instance.playerSpanwer.NavmeshPathDraw(this.gameObject.transform,true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<vThirdPersonController>())
        {
            HudMenuManager.instance.ShowThirdPersonControl(false);
            OnTriggerEnterAction.Invoke();
            this.gameObject.SetActive(false);
        }
        if (other.GetComponent<RCC_CarControllerV3>())
        {
            OnTriggerEnterAction.Invoke();
            this.gameObject.SetActive(false);
        }

        if (other.GetComponent<DroneMovement>())
       {
            Debug.Log("Drone trigger");
           OnTriggerEnterAction.Invoke();
            Debug.Log("Drone trigger2");
            this.gameObject.SetActive(false);
       }
    }

    private void OnTriggerExit(Collider other)
   {
       if (other.GetComponent<vThirdPersonController>())
       {
           DroneOnTriggerExitAction.Invoke();
       }
       if (other.GetComponent<RCC_CarControllerV3>())
       {
           OnTriggerExitAction.Invoke();
           this.gameObject.SetActive(false);
       }
       /*if (!other.GetComponent<RCC_CarControllerV3>() && !other.GetComponent<vThirdPersonController>())
       {
           OnTriggerExitAction.Invoke();
           this.gameObject.SetActive(false);
       }*/
    }

    public void GameOverTaskActionTrigger()
    {
        HudMenuManager.instance.gameplayMenu.infoPopup.SetActive(true);
        HudMenuManager.instance.gameplayMenu.inforPopupText.text = "Task Complete";
        Invoke("DisableInfo", 2f);
        GameConstants.isPlayerWin = true;
        HudMenuManager.instance.GameOver(4);
    }

    public void DisableInfo()
    {
        HudMenuManager.instance.gameplayMenu.infoPopup.SetActive(false);
    }
}
