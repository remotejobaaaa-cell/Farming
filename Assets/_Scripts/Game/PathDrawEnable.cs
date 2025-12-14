using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathDrawEnable : MonoBehaviour
{
    
    private void OnEnable()
    {
            Invoke("NavmeshPathDraw", 1);
    }
    void NavmeshPathDraw()
    {
        if (HudMenuManager.instance != null)
        {
            //HudMenuManager.instance.playerSpanwer.NavmeshPathDraw(this.gameObject.transform);
        }

    }
}
