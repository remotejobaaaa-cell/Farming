using UnityEngine;

public class PropVisibilityController : MonoBehaviour
{
    public Camera mainCamera; // Drag your main camera here
    public float viewDistance = 50f; // Adjust this to set the culling distance

    private void Update()
    {
        AdjustCameraCullingMask();
    }

    void AdjustCameraCullingMask()
    {
        float distanceToProps = Vector3.Distance(mainCamera.transform.position, transform.position);
        if (distanceToProps <= viewDistance)
        {
            // Enable props layer
            mainCamera.cullingMask |= (1 << 30);
        }
        else
        {
            // Disable props layer
            mainCamera.cullingMask &= ~(1 << 30);
        }
    }
}
