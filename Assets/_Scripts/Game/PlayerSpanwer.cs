using Invector.Utils;
using Invector.vCharacterController;
using MTAssets.EasyMinimapSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpanwer : MonoBehaviour
{
    [SerializeField] private HudMenuManager hudMenu;

    public GameObject farmerCamera;
    public GameObject[] farmers;
    public GameObject[] farmersModel;
    public GameObject vehicleCamera;
    public GameObject droneCamera;
    public GameObject tractor;
    public GameObject harvestor;
    public GameObject pickupTruck;
    public GameObject loader; 
    public GameObject drone;
    //public GameObject point;

    //[HideInInspector]
    public GameObject currentPlayer;
    public bool isPlayerSpawn;
    public NavmeshPathDraw navmeshPathDraw;
    public GameObject currentPlayerModel;
    public GameObject currentCamera;

    public DroneObjects droneObjects;

    public vThirdPersonController thirdPersonController;
    public vTargetLookAt targetLookAt;
    public TaskObjects taskObjects;

    public RCC_CarControllerV3 rccCarControllerV3;
    public CropFieldController cropFieldController;
    public HarvesterAudioSource harvesterAudioSource;
    public MinimapCamera minimapCamera;

    public void SpawnPlayer(LevelManager level , Transform playerPosition)
    {
        if (level.levelType == LevelType.ThirdPerson)
        {
            currentPlayer = Instantiate(farmers[GameConstants.currentlySelectedFarmer], playerPosition.position, playerPosition.rotation) as GameObject;
            currentCamera = Instantiate(farmerCamera) as GameObject;
            currentPlayerModel = farmersModel[GameConstants.currentlySelectedFarmer];

            thirdPersonController = currentPlayer.GetComponent<vThirdPersonController>();
            targetLookAt = currentPlayer.GetComponent<vTargetLookAt>();
            taskObjects = currentPlayer.GetComponent<TaskObjects>();
            minimapCamera = currentPlayer.GetComponent<MinimapCamera>();
        }
        else
        {
            if (level.vehicleType == VehicleType.Harvestor)
            {
                currentPlayer = Instantiate(harvestor, playerPosition.position, playerPosition.rotation) as GameObject;
                currentCamera = Instantiate(vehicleCamera) as GameObject;
                //HudMenuManager.instance.vehicleControls.rccMobileButtons.HarvesterControl.SetActive(true);
                harvesterAudioSource = currentPlayer.GetComponent<HarvesterAudioSource>();
                rccCarControllerV3 = currentPlayer.GetComponent<RCC_CarControllerV3>();
                cropFieldController = currentPlayer.GetComponent<CropFieldController>();
                minimapCamera = currentPlayer.GetComponent<MinimapCamera>();
            }
            else if (level.vehicleType == VehicleType.Loader)
            {
                currentPlayer = Instantiate(loader, playerPosition.position, playerPosition.rotation) as GameObject;
                currentCamera = Instantiate(vehicleCamera) as GameObject;

                rccCarControllerV3 = currentPlayer.GetComponent<RCC_CarControllerV3>();
                minimapCamera = currentPlayer.GetComponent<MinimapCamera>();
            }
            else if (level.vehicleType == VehicleType.PickupTruck)
            {
                currentPlayer = Instantiate(pickupTruck, playerPosition.position, playerPosition.rotation) as GameObject;
                currentCamera = Instantiate(vehicleCamera) as GameObject;

                rccCarControllerV3 = currentPlayer.GetComponent<RCC_CarControllerV3>();
                minimapCamera = currentPlayer.GetComponent<MinimapCamera>();
            }
            else if (level.vehicleType == VehicleType.Tractor)
            {
                currentPlayer = Instantiate(tractor, playerPosition.position, playerPosition.rotation) as GameObject;
                currentCamera = Instantiate(vehicleCamera) as GameObject;

                rccCarControllerV3 = currentPlayer.GetComponent<RCC_CarControllerV3>();
                minimapCamera = currentPlayer.GetComponent<MinimapCamera>();
            }
            else if (level.vehicleType == VehicleType.Drone)
            {
                currentPlayer = Instantiate(drone, playerPosition.position, playerPosition.rotation) as GameObject;
                //currentCamera = Instantiate(droneCamera) as GameObject;
                hudMenu.thirdPersonControl.droneJoystickToggle.SetActive(true);

                droneObjects = currentPlayer.GetComponent<DroneObjects>();
                minimapCamera = currentPlayer.GetComponent<MinimapCamera>();
            }
        }
        navmeshPathDraw = GameObject.FindObjectOfType<NavmeshPathDraw>();
        TriggerAction triggerAction = FindObjectOfType<TriggerAction>();
        if (triggerAction != null)
        {
            triggerAction.NavmeshPathDraw();
        }
        MInimapItemController mInimapItemController = FindObjectOfType<MInimapItemController>();
        if (mInimapItemController != null)
        {
            mInimapItemController.NavmeshPath();
        }
        isPlayerSpawn = true;
    }
    public  void NavmeshPathDraw(Transform t,bool b)
    {
        /*if (navmeshPathDraw != null)
        {
            navmeshPathDraw.gameObject.SetActive(b);
            navmeshPathDraw.destination = t;
        }*/
    }
}
