using System.Collections.Generic;
using UnityEngine;

public class DataContainerMap : MonoBehaviour
{
    public int id;
    public bool isLocked;
    public string mapName;

    public int unlockPrice;

    public string mapDescription;

    public List<DataContainerLevel> routes;
}