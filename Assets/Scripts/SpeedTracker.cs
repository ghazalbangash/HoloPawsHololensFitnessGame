using UnityEngine;

public class SpeedTracker : MonoBehaviour
{
    public Camera trackingCamera; // Assign this in the Inspector

    private Vector3 previousPosition;
    private float currentSpeed;
    private float deltaTime;

    void Start()
    {
        if (trackingCamera == null)
        {
            Debug.LogError("Tracking camera is not assigned.");
            return;
        }

        // Initialize previous position to the camera's starting position
        previousPosition = trackingCamera.transform.position;
    }

    void Update()
    {
        if (trackingCamera == null)
        {
            return;
        }

        // Calculate time elapsed since the last frame
        deltaTime = Time.deltaTime;

        // Get the current position of the camera
        Vector3 currentPosition = trackingCamera.transform.position;

        // Calculate the distance moved since the last frame
        float distanceMoved = Vector3.Distance(currentPosition, previousPosition);

        // Calculate speed (distance / time)
        currentSpeed = distanceMoved / deltaTime;

        // Update the previous position to the current position for the next frame
        previousPosition = currentPosition;

        // Optionally, log the speed for debugging
        Debug.Log("Current Speed: " + currentSpeed);
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }
}
