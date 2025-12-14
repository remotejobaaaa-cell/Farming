using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UnloadCropsToTruck : MonoBehaviour
{
    public GameObject playerVehicle;
    public DOTweenAnimation harvesterPipe;
    public GameObject cropParticle;
    public AudioClip wheatDropSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<RCC_CarControllerV3>())
        {
            playerVehicle = other.gameObject;
            other.GetComponent<RCC_CarControllerV3>().KillEngine();
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            HudMenuManager.instance.rccMobileButtons.DisableButtons();
            harvesterPipe.DOPlay();
            ShowWaitPopup();
        }
    }

    public void ShowWaitPopup()
    {
        HudMenuManager.instance.gameplayMenu.infoPopup.gameObject.SetActive(true);
        HudMenuManager.instance.gameplayMenu.inforPopupText.text = "Wait till trailer filled";
        cropParticle.GetComponent<AudioSource>().PlayOneShot(wheatDropSound);
        Invoke("UnloadCropComplete", 5f);
    }

    public void UnloadCropComplete()
    {
        cropParticle.GetComponent<AudioSource>().enabled = false;
        cropParticle.SetActive(false);
        playerVehicle.GetComponent<RCC_CarControllerV3>().attachedTrailer.CropObject.SetActive(true);
        HudMenuManager.instance.gameplayMenu.inforPopupText.text = "Process Completed";
        HudMenuManager.instance.rccMobileButtons.EnableButtons();
        playerVehicle.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        playerVehicle.GetComponent<RCC_CarControllerV3>().StartEngine();
        Invoke("HideWaitPopup", 2f);
    }
    public void HideWaitPopup()
    {
        HudMenuManager.instance.gameplayMenu.infoPopup.gameObject.SetActive(false);
    }

}
