using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SpecialOffer : MonoBehaviour
{
    public Button offerIsAvailable;
    public Button hostess;
    public GameObject offerPopup;

    private void OnEnable()
    {
        offerIsAvailable.gameObject.SetActive(true);
        hostess.gameObject.SetActive(true);
        offerIsAvailable.onClick.AddListener(ClickOnOffer);
        hostess.onClick.AddListener(ClickOnOffer);
        offerPopup.SetActive(false);
    }

    public void ClickOnOffer()
    {
        offerPopup.SetActive(true);
        offerIsAvailable.gameObject.SetActive(false);
        transform.DOMove(new Vector3(transform.position.x, 0, transform.position.z), 1);
        hostess.onClick.RemoveAllListeners();
    }

    public void OnClick_WantOffer()
    {
        Debug.Log("Need Ad Implementation");
    }

    public void OnClick_DontWant()
    {
        offerIsAvailable.onClick.RemoveAllListeners();
        transform.DOMove(new Vector3(transform.position.x, -100, transform.position.z), 0);
        gameObject.SetActive(false);
    }
}
