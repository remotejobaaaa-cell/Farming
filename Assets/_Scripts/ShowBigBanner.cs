using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBigBanner : MonoBehaviour
{
    public bool dontDisable;
    // Start is called before the first frame update
    private void OnEnable()
    {
#if HAVE_ADS
        if (AdsManager.instance != null)
        {
            AdsManager.instance.ShowRectBanner();
        }
#endif
    }

    private void OnDisable()
    {
        if (!dontDisable)
        {
#if HAVE_ADS
            if (AdsManager.instance != null)
            {
                AdsManager.instance.HideRectBanner();
            }
#endif
        }
    }

}
