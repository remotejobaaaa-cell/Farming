using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class PlayerDataController : MonoBehaviour
{
    public static PlayerDataController instance;
    //[HideInInspector]
    public PlayerData playerData;
    public DataContainerPlayerData defaultPlayerDataObject;

    readonly string fileName = "/GameState.Gd";

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        Load();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + fileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + fileName, FileMode.Open);
            Debug.Log("Path " + Application.persistentDataPath + fileName);
            playerData = (PlayerData)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            LoadSaveDeafultData();
        }
    }

    void LoadSaveDeafultData()
    {
        Debug.Log("First Time in Database");
        playerData = new PlayerData
        {
            playerCash = defaultPlayerDataObject.playerCash,
            currentPlayerLevel = 1,
            farmers = new List<PlayerFarmer>(),
            maps = new List<Map>(),
        };


        for (var i = 0; i < defaultPlayerDataObject.farmers.Count; i++)
        {
            var pFarmer = new PlayerFarmer
            {
                id = defaultPlayerDataObject.farmers[i].id,
                unlockPrice = defaultPlayerDataObject.farmers[i].unlockPrice,
                isLocked = defaultPlayerDataObject.farmers[i].isLocked,
                farmerName = defaultPlayerDataObject.farmers[i].farmerName,
            };

            playerData.farmers.Add(pFarmer);
        }

        for (var i = 0; i < defaultPlayerDataObject.maps.Count; i++)
        {
            var pMap = new Map
            {
                id = defaultPlayerDataObject.maps[i].id,
                isLocked = defaultPlayerDataObject.maps[i].isLocked,
                mapName = defaultPlayerDataObject.maps[i].mapName,
                unlockPrice = defaultPlayerDataObject.maps[i].unlockPrice,
                mapDescription = defaultPlayerDataObject.maps[i].mapDescription,

                routes = new List<Level>(),
            };

            playerData.maps.Add(pMap);

            //Regions
            for (int q = 0; q <= defaultPlayerDataObject.maps[i].routes.Count - 1; q++)
            {
                var pRoute = new Level
                {
                    id = defaultPlayerDataObject.maps[i].routes[q].id,
                    reward = defaultPlayerDataObject.maps[i].routes[q].reward,
                    isLocked = defaultPlayerDataObject.maps[i].routes[q].isLocked,
                    isPlayed = defaultPlayerDataObject.maps[i].routes[q].isPlayed,
                };
                playerData.maps[i].routes.Add(pRoute);
            }
            //End Regions
        }

        Save();
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        //Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
        FileStream file = File.Create(Application.persistentDataPath + fileName); //you can call it anything you want
        bf.Serialize(file, playerData);
        file.Close();
    }

    string ColorToHex(Color32 color)
    {
        string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
        return hex;
    }
}