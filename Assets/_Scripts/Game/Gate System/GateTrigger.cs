using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateTrigger : MonoBehaviour
{
    public GameObject door;
    public bool isThisTruck;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            door.GetComponent<Animator>().SetTrigger("Open");
            if (!isThisTruck)
            {
                other.GetComponent<GateTrigger>().door = this.gameObject;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Invoke("DoorCloseAnim", 3f);
        }
    }
    public void DoorCloseAnim()
    {
        door.GetComponent<Animator>().SetTrigger("Close");
    }
}
