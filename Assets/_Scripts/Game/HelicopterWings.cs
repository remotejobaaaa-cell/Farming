using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HelicopterWings : MonoBehaviour
{
    public float xSpeed = 20f; 
    public float ySpeed = 20f;
    public float zSpeed = 20f;
    public GameObject[] wings;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
            foreach (GameObject items in wings)
            {
                items.transform.Rotate(xSpeed, ySpeed, zSpeed);
            }
       
    }
}
