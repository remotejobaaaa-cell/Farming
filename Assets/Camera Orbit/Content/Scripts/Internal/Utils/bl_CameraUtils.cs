using UnityEngine;

public static class bl_CameraUtils
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
        {
            angle += 360f;
        }
        if (angle > 360f)
        {
            angle -= 360f;
        }
        return Mathf.Clamp(angle, min, max);
    }

    /// <summary>
    /// Helper for Cursor locked in Unity 5
    /// </summary>
    /// <param name="mLock">cursor state</param>
    public static void LockCursor(bool mLock)
    {
#if UNITY_5_3_OR_NEWER
        if (mLock == true)
        {
            ControlFreak2.CFCursor.visible = false;
            ControlFreak2.CFCursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            ControlFreak2.CFCursor.visible = true;
            ControlFreak2.CFCursor.lockState = CursorLockMode.None;
        }
#else
        ControlFreak2.CFScreen.lockCursor = mLock;
#endif
    }
    /// <summary>
    /// 
    /// </summary>
    public static bool GetCursorState
    {
        get
        {
#if UNITY_5_3_OR_NEWER
            if (ControlFreak2.CFCursor.visible && ControlFreak2.CFCursor.lockState != CursorLockMode.Locked)
            {
                return false;
            }
            else
            {
                return true;
            }
#else
            return ControlFreak2.CFScreen.lockCursor;
#endif
        }
    }
}