using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Invector.vCharacterController;

public class FieldInfoController : MonoBehaviour
{
    private HudMenuManager hudMenuManager;
    public string OwnerName;
    public string fieldTypeName;
    private void Awake()
    {
        hudMenuManager = FindObjectOfType<HudMenuManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<vThirdPersonController>())
        {
            if (!hudMenuManager)
            {
                hudMenuManager = FindObjectOfType<HudMenuManager>();
            }
            hudMenuManager.gameplayMenu.fieldInfoAnim.SetTrigger("ShowFieldInfo");
            hudMenuManager.gameplayMenu.fieldOwnerNameText.text = OwnerName;
            hudMenuManager.gameplayMenu.fieldTypeNameText.text = fieldTypeName;
        }
        if (other.GetComponent<RCC_CarControllerV3>())
        {
            if (!hudMenuManager)
            {
                hudMenuManager = FindObjectOfType<HudMenuManager>();
            }
            if (other.CompareTag("Harvester"))
            {
                other.GetComponent<RCC_CarControllerV3>().maxspeed = 5;
                other.GetComponent<RCC_CarControllerV3>().KillEngine();
                hudMenuManager.rccMobileButtons.HarvesterControl.SetActive(true);
                //hudMenuManager.vehicleControls.rccMobileButtons.HarvesterControl..getSetActive(true);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<vThirdPersonController>())
        {
            hudMenuManager.gameplayMenu.fieldInfoAnim.SetTrigger("HideFieldInfo");
        }
    }
}
