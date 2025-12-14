using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class VehiclePositionChange : MonoBehaviour
{
    public enum NeedPopup { Yes, No }
    public enum NeedRotate { Yes, No }

    public Transform newPostion;

    [Header("Popup About Task")]
    public NeedPopup needPopup;
    public NeedRotate needRotate;
    public string TaskInfoText;

    public UnityEvent onTriggerEnterEvent;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<RCC_CarControllerV3>())
        {
            onTriggerEnterEvent.Invoke();
            HudMenuManager.instance.ShowVehicleControl(false);
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            HudMenuManager.instance.gameplayMenu.dimmer.SetActive(true);
            HudMenuManager.instance.gameplayMenu.dimmer.GetComponent<Animator>().SetTrigger("Blink");
            if(needRotate == NeedRotate.Yes)
            {
                other.transform.position = newPostion.position;
                other.transform.rotation = newPostion.rotation;
            }
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            HudMenuManager.instance.ShowVehicleControl(true);
            if(needPopup == NeedPopup.Yes)
            {
                HudMenuManager.instance.gameplayMenu.infoPopup.SetActive(true);
                HudMenuManager.instance.gameplayMenu.inforPopupText.text = TaskInfoText;
                Invoke("HidePopup", 2f);
            }
        }
    }
    public void HidePopup()
    {
        HudMenuManager.instance.gameplayMenu.infoPopup.SetActive(false);
        HudMenuManager.instance.gameplayMenu.dimmer.SetActive(false);
    }


}
