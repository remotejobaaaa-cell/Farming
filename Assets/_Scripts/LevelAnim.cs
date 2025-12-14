using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAnim : MonoBehaviour
{
    public Transform unlockAllLevels;
    public float duration;
    public LeanTweenType easeType;
    private void OnEnable()
    {
        LeanArray();
    }

    public void LeanArray()
    {
        unlockAllLevels.GetComponent<RectTransform>().localPosition = new Vector2(0, -800);

        LeanTween.move(unlockAllLevels.GetComponent<RectTransform>(), new Vector2(0, 30), duration);
    }
}
