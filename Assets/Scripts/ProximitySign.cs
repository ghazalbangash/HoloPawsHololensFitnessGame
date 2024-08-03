using UnityEngine;
using TMPro;

public class ProximitySign : MonoBehaviour
{
    public TMP_Text signText; // Reference to the TextMeshPro component
    public Canvas signCanvas; // Reference to the Canvas component
    public Transform userTransform; // Reference to the user's transform

    public float minDisplayInterval = 10f; // Minimum time between sign displays
    public float maxDisplayInterval = 20f; // Maximum time between sign displays
    public float distanceFromUser = 2f; // Distance in front of the user
    public float offsetToRight = 1f; // Offset to the right

    private float timer = 0f;
    private float nextDisplayTime = 0f;
    private bool isCurrentlyVisible = false;

    void Start()
    {
        SetSignVisibility(false);
        SetNextDisplayTime();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (!isCurrentlyVisible && timer >= nextDisplayTime)
        {
            Debug.Log("Display condition met, showing sign.");
            //UpdateSignPosition();
            SetSignVisibility(true);
            SetNextDisplayTime();
        }
        else if (isCurrentlyVisible && timer >= nextDisplayTime)
        {
            Debug.Log("Hide condition met, hiding sign.");
            SetSignVisibility(false);
            SetNextDisplayTime();
        }
    }

    void UpdateSignPosition()
    {
        Vector3 forwardOffset = userTransform.forward * distanceFromUser;
        //Vector3 rightOffset = userTransform.right * offsetToRight;
        signCanvas.transform.position = userTransform.position + forwardOffset;
    }

    void SetSignVisibility(bool visible)
    {
        signCanvas.enabled = visible;
        isCurrentlyVisible = visible;
        Debug.Log($"Sign visibility set to {visible}.");
    }

    void SetNextDisplayTime()
    {
        nextDisplayTime = timer + Random.Range(minDisplayInterval, maxDisplayInterval);
        Debug.Log($"Next display time set to {nextDisplayTime} seconds from now.");
    }

    public void UpdateSignText(string text)
    {
        Debug.Log("call update sign text");
        signText.text = text;
    }
}
