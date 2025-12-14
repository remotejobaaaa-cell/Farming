using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.vCharacterController;

public class LevelManager : MonoBehaviour
{
    private LevelLoaderManager levelLoaderManager;

    [Header("Level Type")]
    public LevelType levelType;
    public VehicleType vehicleType;

    [Header("Time")]
    public TimeMode timeMode;

    [Header("Cinematics")]
    public Cinematics cinematics;
    public GameObject cinematicObject;

    public enum DiableField { No, Yes, }
    [Header("Field To Crop")]
    public DiableField diableField;
    public GameObject fieldToCrop;
    public FieldType fieldType;

    [Header("Player Attributes")]
    public Transform playerInsPos;
    public bool hasTrailor;
    void Start()
    {
        levelLoaderManager = FindObjectOfType<LevelLoaderManager>();
        HudMenuManager.instance.SetCurrentLevel(this);
        SetActiveFields(DiableField.Yes);
    }
    private void Update()
    {
        if (!hasTrailor)
        {
            return;
        }
        else if (hasTrailor)
        {
            if (DroneObjects.instance) {
                DroneObjects.instance.objects[4].SetActive(true);
            }
        }
       
    }
    public void ShowCinametics()
    {
        cinematicObject.SetActive(true);
        HudMenuManager.instance.ShowThirdPersonControl(false);
    }
    public void HideCinametics()
    {
        cinematicObject.SetActive(false);
    }

    public void CutSceneCompleted()
    {
        HudMenuManager.instance.CinameticsCompleted();
    }

    public void GetFields()
    {
        if (fieldType == FieldType.Wheat)
        {
            fieldToCrop = levelLoaderManager.wheatCrop;
        }
        else if (fieldType == FieldType.Corn)
        {
            fieldToCrop = levelLoaderManager.cornCrop;
        }
        else if (fieldType == FieldType.pumpkin)
        {
            fieldToCrop = levelLoaderManager.pumpkinCrop;
        }
        else if (fieldType == FieldType.Watermellon)
        {
            fieldToCrop = levelLoaderManager.watermelonCrop;
        }
        else if (fieldType == FieldType.Mix)
        {
            fieldToCrop = levelLoaderManager.mixCrop;
        }
    }

    public void SetActiveFields(DiableField diableFields)
    {
        GetFields();
        if (diableFields == DiableField.Yes)
        {
            fieldToCrop.SetActive(false);
        }
        else if(diableFields == DiableField.No)
        {
            fieldToCrop.SetActive(true);
        }
    }

    public void LevelComplete(float timeDelay)
    {
        levelLoaderManager.hudMenuManager.ShowThirdPersonControl(false);
        levelLoaderManager.hudMenuManager.ShowVehicleControl(false);
        GameConstants.isPlayerWin = true;
        levelLoaderManager.hudMenuManager.GameOver(timeDelay);
    }

    public void SetEnablePlayerInput(bool setInput)
    {
        HudMenuManager.instance.ShowThirdPersonControl(setInput);
    }
}
