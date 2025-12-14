using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemController : MonoBehaviour
{
    public List<GameObject> shopItem;

    public void BuyItems()
    {
        HudMenuManager.instance.gameplayMenu.dimmer.SetActive(true);
        HudMenuManager.instance.gameplayMenu.dimmer.GetComponent<Animator>().SetTrigger("Blink");
        foreach (var item in shopItem)
        {
            item.SetActive(true);
        }
        Invoke("disableDimmer", 2f);
        this.gameObject.GetComponent<ShopItemController>().enabled = false;
    }

    public void SellItems()
    {
        HudMenuManager.instance.gameplayMenu.dimmer.SetActive(true);
        HudMenuManager.instance.gameplayMenu.dimmer.GetComponent<Animator>().SetTrigger("Blink");
        foreach (var item in shopItem)
        {
            item.SetActive(false);
        }
        Invoke("disableDimmer", 2f);
        this.gameObject.GetComponent<ShopItemController>().enabled = false;
    }

    public void disableDimmer()
    {
        HudMenuManager.instance.gameplayMenu.dimmer.SetActive(false);
       
    }
}
