using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Invector.vCharacterController;
using Invector.Utils;

public class TPSPositionChange : MonoBehaviour
{
    private HudMenuManager hudMenu;
    public enum NeedPopup { Yes, No }
    public enum NeedLookAt { Yes, No }
    public Transform newPostion;
    public Transform lastPostion;
    public GameObject particle;

    [Header("Popup About Task")]
    public NeedPopup needPopup;
    public string TaskInfoText;

    [Header("Look At")]
    public NeedLookAt needLookAt;
    public Transform lookAtTarget;

    public UnityEvent onTriggerEnterEvent;
    public float delayEventTime;
    public UnityEvent delayEvent;
    
    private void Awake()
    {
        hudMenu = HudMenuManager.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<vThirdPersonController>())
        {
            if (!hudMenu)
            {
                hudMenu = HudMenuManager.instance;
            }
            onTriggerEnterEvent.Invoke();
            hudMenu.ShowThirdPersonControl(false);
            hudMenu.gameplayMenu.dimmer.SetActive(true);
            hudMenu.gameplayMenu.dimmer.GetComponent<Animator>().SetTrigger("Blink");
            other.transform.position = newPostion.position;
            other.transform.rotation = newPostion.rotation;
            if (needLookAt == NeedLookAt.Yes)
            {
                hudMenu.playerSpanwer.targetLookAt.enabled = true;
                hudMenu.playerSpanwer.targetLookAt.target = lookAtTarget;
            }
            if (needPopup == NeedPopup.Yes)
            {
                hudMenu.gameplayMenu.infoPopup.SetActive(true);
                hudMenu.gameplayMenu.inforPopupText.text = TaskInfoText;
                Invoke("HidePopup", 2f);
            }
            Invoke("InvokeDelayEvent", delayEventTime);
            Invoke("resetPlayer", delayEventTime * 3);
        }
    }
    public void HidePopup()
    {
        hudMenu.gameplayMenu.infoPopup.SetActive(false);
    }

    public void InvokeDelayEvent()
    {
        delayEvent.Invoke();
    }


    public void resetPlayer()
    {
        hudMenu.playerSpanwer.targetLookAt.enabled = false;
        hudMenu.playerSpanwer.currentPlayer.transform.SetPositionAndRotation(lastPostion.position, lastPostion.rotation);
        //HudMenuManager.instance.playerSpanwer.currentPlayer.transform.position = lastPostion.position;
        //HudMenuManager.instance.playerSpanwer.currentPlayer.transform.rotation = lastPostion.rotation;
        hudMenu.gameplayMenu.dimmer.SetActive(false);
        hudMenu.ShowThirdPersonControl(true);
        particle.SetActive(false);
        hudMenu.GetComponent<AudioSource>().Stop();
    }
}
