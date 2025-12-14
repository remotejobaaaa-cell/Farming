using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvectorInputController : MonoBehaviour
{
    public int actionTry;

    public void DeductActionTry()
    {
        actionTry -= 1;
    }

}
