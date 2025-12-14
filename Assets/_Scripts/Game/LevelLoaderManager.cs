using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class LevelLoaderManager : MonoBehaviour
{
    public HudMenuManager hudMenuManager;
    public LevelManager[] levels;
    public bool testingMode;
    public GameObject wheatCrop;
    public GameObject cornCrop;
    public GameObject pumpkinCrop;
    public GameObject watermelonCrop;
    public GameObject mixCrop;

    public LevelManager currentLevel;
    private GameObject level;
    public int levelNumber;
    int levlNo;
    private void OnEnable()
    {
        if (testingMode)
        {
            levels[levelNumber - 1].gameObject.SetActive(true);
        }
        else
        {
            level = Instantiate(Resources.Load("Level (" + (GameConstants.currentlySelectedLevel + 1) + ")") as GameObject);
            currentLevel = level.GetComponent<LevelManager>();

            int levelStart = GameConstants.currentlySelectedLevel + 1;
            Analytics(levelStart.ToString());
#if HAVE_ADS
            if (AdsManager.instance)
                AdsManager.instance.LogAnalyticsEvent("level_start_" + levelStart);
#endif
        }
    }
    //private void Update()
    //{
    //    /* levlNo = levelNumber - 1;
    //    Debug.Log(levels[levelNumber-1]);*/
    //    /*if (levels[levlNo = 18].gameObject && levels[levlNo = 20].gameObject)
    //    {
    //        DroneObjects.instance.objects[4].SetActive(true);
    //    }*/
    //    /*if(levelNumber!=6 && levelNumber!=11 && levelNumber!=13)
    //    {
    //        DroneObjects.instance.objects[4].SetActive(true);
    //    }*/
    //}
    private void Analytics(string massage)
    {
    }

}
