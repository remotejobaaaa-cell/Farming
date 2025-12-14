using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAnim : MonoBehaviour
{
    public Transform unlockAllGame;
    public RectTransform superSale;
    public RectTransform rewardBtn;
    public Transform noAds;
    public float duration;
    public LeanTweenType easeType;

    private void OnEnable()
    {
        LeanArray();
    }

    public void LeanArray()
    {
        unlockAllGame.GetComponent<RectTransform>().localPosition = new Vector2(-2000, 50);
        noAds.GetComponent<RectTransform>().localPosition = new Vector2(-2500, -1222);

        LeanTween.move(unlockAllGame.GetComponent<RectTransform>(), new Vector2( 50, -50), duration);
        LeanTween.move(noAds.GetComponent<RectTransform>(), new Vector2(44.8f, 22.67f), duration);
		LeanTween.size(superSale, superSale.sizeDelta * 1.1f, 0.5f).setDelay(3f).setEaseInOutCirc().setLoopPingPong();
        // LeanTween.(superSale.GetComponent<RectTransform>(), new Vector2(44.8f, 22.67f), duration);

    }
}
