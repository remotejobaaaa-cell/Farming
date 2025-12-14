using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.vCharacterController;
public class MInimapItemController : MonoBehaviour
{
    public GameObject nextWaypoint;
    public NavmeshPathDraw navmeshPathDraw;

    public enum Waypoint { None, LastWaypoint, LastPointMinimapRoute, LastPointNoGameOver, }
    public Waypoint waypoint;
    //public bool isLastWaypoint;

    //private void OnEnable()
    //{
    //    if (HudMenuManager.instance != null)
    //    {
    //        if (HudMenuManager.instance.playerSpanwer.isPlayerSpawn==true)
    //            Invoke("NavmeshPath", 0.5f);
    //    }
       
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<vThirdPersonController>())
        {

            if (waypoint == Waypoint.LastPointMinimapRoute)
            {
                HudMenuManager.instance.gameplayMenu.minimapRoute.gameObject.SetActive(false);
            }
            if (nextWaypoint)
            {
                if (AudioManager.instance)
                    AudioManager.instance.Play_CheckPointSound();
                nextWaypoint.SetActive(true);
            }
            if (waypoint == Waypoint.LastWaypoint)
            {
                GameConstants.isPlayerWin = true;
                HudMenuManager.instance.GameOver(3);
            }
            this.gameObject.SetActive(false);
        }

        if (other.GetComponent<RCC_CarControllerV3>())
        {
            if (nextWaypoint)
            {
                if (AudioManager.instance)
                    AudioManager.instance.Play_CheckPointSound(); nextWaypoint.SetActive(true);
            }
            if (waypoint == Waypoint.LastWaypoint)
            {
                HudMenuManager.instance.ShowVehicleControl(false);
                other.GetComponent<RCC_CarControllerV3>().KillEngine();
                other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                GameConstants.isPlayerWin = true;
                HudMenuManager.instance.GameOver(3);
            }
            if (waypoint == Waypoint.LastPointNoGameOver)
            {
                HudMenuManager.instance.ShowVehicleControl(false);
                other.GetComponent<RCC_CarControllerV3>().KillEngine();
                other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

               
            }
            this.gameObject.SetActive(false);
        }
        if (other.GetComponent<DroneMovement>())
        {
            Debug.Log("DRONE");
            if (nextWaypoint)
            {
                if (AudioManager.instance)
                    AudioManager.instance.Play_CheckPointSound(); nextWaypoint.SetActive(true);
            }
            if (waypoint == Waypoint.LastWaypoint)
            {
                HudMenuManager.instance.ShowThirdPersonControl(false);
                other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                GameConstants.isPlayerWin = true;
                HudMenuManager.instance.GameOver(3);
            }
            if (waypoint == Waypoint.LastPointNoGameOver)
            {
                HudMenuManager.instance.ShowThirdPersonControl(false);
                other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

               
            }
            this.gameObject.SetActive(false);
        }
        //if (HudMenuManager.instance != null)
        //{
        //    HudMenuManager.instance.playerSpanwer.NavmeshPathDraw(this.gameObject.transform, false);
        //}
    }
  public  void NavmeshPath()
    {
        if (HudMenuManager.instance != null)
        {

            //HudMenuManager.instance.playerSpanwer.NavmeshPathDraw(this.gameObject.transform, true);
        }
    }
}
