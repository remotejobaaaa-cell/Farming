using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTAssets.EasyMinimapSystem;

public class MinimapDestination : MonoBehaviour
{
    public void Start()
    {
        HudMenuManager.instance.gameplayMenu.minimapRoute.destinationPoint = this.transform;
    }
   
    public void OnEnable()
    {
        delayDestinationPoint(0.5f);
    }

    private void DestinationPoint()
    {
        if (HudMenuManager.instance.gameplayMenu.minimapRoute.gameObject.activeInHierarchy == false)
        {
            HudMenuManager.instance.gameplayMenu.minimapRoute.gameObject.SetActive(true);
        }
        if (HudMenuManager.instance.gameplayMenu.minimapRoute.destinationPoint == null)
        {
            HudMenuManager.instance.gameplayMenu.minimapRoute.destinationPoint = this.transform;
            HudMenuManager.instance.gameplayMenu.minimapRoute.StartCalculatingAndShowRotesToDestination();
        }
        //HudMenuManager.instance.gameplayMenu.minimapRoute.StartCalculatingAndShowRotesToDestination();
        HudMenuManager.instance.gameplayMenu.minimap.AddMinimapItemToBeHighlighted(this.GetComponent<MinimapItem>());

    }
    private void delayDestinationPoint(float delayTime)
    {
        Invoke("DestinationPoint", delayTime);

    }
}
