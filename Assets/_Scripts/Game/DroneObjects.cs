using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneObjects : MonoBehaviour
{
    public GameObject[] objects;
    public static DroneObjects instance;
    
    void Start()
    {
        instance = this;
        foreach(GameObject items in objects)
        {
            items.SetActive(false);
        }
       
    }
    public void BucketOn(int index)
    {
    //[0]=bucket.....[1]basket.....[2]egg....[3]watering....[4]trailer....
        objects[index].SetActive(true);
    }
    
    public void BucketOff()
    {
        foreach (GameObject items in objects)
        {
            items.SetActive(false);
        }
    }
    
}
