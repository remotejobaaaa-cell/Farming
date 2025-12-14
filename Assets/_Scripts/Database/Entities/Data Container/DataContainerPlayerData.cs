using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataContainerPlayerData : MonoBehaviour
{
    [Header("Player Profile")]
    [Space(2)]
    public float playerCash;

    //[HideInInspector]
    //public Image selectedLevelImage;

    [Header("Farmers")]
    public List<DataContainerPlayerFarmer> farmers;

    [Header("Maps")]
    public List<DataContainerMap> maps;
}