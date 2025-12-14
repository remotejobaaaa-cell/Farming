using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Invector.vCharacterController;

public class OnEnableAction : MonoBehaviour
{
    public UnityEvent onEnableAction;
    public UnityEvent onDisableAction;

    [Header("Delay Actions")]
    public float onEnableTime;
    public UnityEvent onEnableActionDelay;
    public float onDisableTime;
    public UnityEvent onDisableActionDelay;
    private void OnEnable()
    {
        onEnableAction.Invoke();
        Invoke("DelayOnEnable", onEnableTime);
    }
    private void OnDisable()
    {
        onDisableAction.Invoke();
        Invoke("DelayOnEnable", onDisableTime);
    }

    void DelayOnEnable()
    {
        onEnableActionDelay.Invoke();
    }
    void DelayOnDisablee()
    {
        onDisableActionDelay.Invoke();
    }
}
