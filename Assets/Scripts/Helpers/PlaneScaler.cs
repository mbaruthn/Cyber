using UnityEngine;

public class PlaneScaler : MonoBehaviour
{
    public Camera mainCamera; // Reference to the main camera
    public GameObject plane;  // Reference to the Plane object
    public float marginPercentage = 0.1f; // Margin to leave around the plane (10% default)

    private void Start()
    {
        if (mainCamera == null || plane == null)
        {
            Debug.LogError("Main Camera or Plane is not assigned!");
            return;
        }

        ScalePlaneToScreen();
    }

    private void Update()
    {
        // Detect screen size changes or orientation changes
        if (Screen.width != Screen.currentResolution.width || Screen.height != Screen.currentResolution.height)
        {
            ScalePlaneToScreen();
        }
    }

    private void ScalePlaneToScreen()
    {
        // Calculate screen dimensions in world units
        float screenHeight = 2f * mainCamera.orthographicSize;
        float screenWidth = screenHeight * mainCamera.aspect;

        // Apply margins
        float targetWidth = screenWidth * (1 - marginPercentage);
        float targetHeight = screenHeight * (1 - marginPercentage);

        // Adjust the Plane's scale to match the target dimensions
        MeshRenderer planeRenderer = plane.GetComponent<MeshRenderer>();
        if (planeRenderer != null)
        {
            Vector3 planeSize = planeRenderer.bounds.size;

            // Calculate the scale factors for width and height
            float scaleX = targetWidth / planeSize.x;
            float scaleZ = targetHeight / planeSize.z;

            // Apply the scale to the Plane's localScale
            plane.transform.localScale = new Vector3(
                plane.transform.localScale.x * scaleX,
                plane.transform.localScale.y,
                plane.transform.localScale.z * scaleZ
            );
        }
        else
        {
            Debug.LogError("Plane does not have a MeshRenderer component!");
        }
    }
}
