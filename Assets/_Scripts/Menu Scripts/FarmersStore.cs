using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmersStore : MonoBehaviour
{
    public MainMenuManager mainMenuManager;

    public bl_CameraOrbit cameraOrbit;
    private CameraFocus currentFocusPoint;

    public List<GameObject> farmerPrefabs;
    private GameObject currentFarmer;
    public int currentIndex;

    private void OnEnable()
    {
        GameConstants.currentlySelectedFarmer =mainMenuManager.playerData.currentlySelectedFarmer;
        mainMenuManager.garageVehicles.ChangeCameraFocusPoint(CameraFocus.StartMenu);
    }

    void Start()
    {
        currentIndex = GameConstants.currentlySelectedFarmer;
        ShowFarmer();
    }

    public void NextFarmer()
    {
        currentIndex++;
        if (currentIndex >= farmerPrefabs.Count)
        {
            currentIndex = 0;
        }
        ShowFarmer();
    }

    public void PreviousFarmer()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = farmerPrefabs.Count - 1;
        }
        ShowFarmer();
    }

    public void ShowFarmer()
    {
        if (currentFarmer != null)
        {
            currentFarmer.SetActive(false);
        }
        currentFarmer = farmerPrefabs[currentIndex];

        farmerPrefabs[currentIndex].SetActive(true);
        mainMenuManager.ownVehicleMenu.currentFarmerName.text = mainMenuManager.playerData.farmers[currentIndex].farmerName;
    } 
    public void ShowFarmer(int index)
    {
        currentIndex = index;

        if (currentFarmer != null)
        {
            currentFarmer.SetActive(false);
        }
        currentFarmer = farmerPrefabs[currentIndex];

        farmerPrefabs[currentIndex].SetActive(true);
        mainMenuManager.ownVehicleMenu.currentFarmerName.text = mainMenuManager.playerData.farmers[currentIndex].farmerName;
    }

    public void ChangeCameraFocusPoint(CameraFocus focusOn)
    {
        if (currentFocusPoint == focusOn)
            return;
        currentFocusPoint = focusOn;
        //cameraOrbit.SetViewPoint(CameraFocus.FarmerSelection);
        if (focusOn == CameraFocus.StartMenu)
        {
            mainMenuManager.mobileDrag.SetActive(false);
        }
        else
        {
            mainMenuManager.mobileDrag.SetActive(true);
        }
    }
}
