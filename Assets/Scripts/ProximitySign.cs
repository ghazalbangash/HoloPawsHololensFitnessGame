using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProximitySign : MonoBehaviour
{
    public GameObject sign; // The sign UI element
    public TextMeshProUGUI heartRateText;
    public TextMeshProUGUI caloriesBurnedText;
    private CaloryCalculator calorieCalculator;
    private float updateInterval = 5.0f; // Interval to update the sign
    private float lastUpdateTime = 0.0f;

    void Start()
    {
        sign.SetActive(false);
        calorieCalculator = FindObjectOfType<CaloryCalculator>();
        if (calorieCalculator == null)
        {
            Debug.LogError("CalorieCalculator component not found in the scene.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UpdateSign();
            sign.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            sign.SetActive(false);
        }
    }

    void Update()
    {
        if (sign.activeSelf && Time.time - lastUpdateTime > updateInterval)
        {
            UpdateSign();
            lastUpdateTime = Time.time;
        }
    }

    private void UpdateSign()
    {
        if (calorieCalculator != null)
        {
            float averageHeartRate = calorieCalculator.GetAverageHeartRate(); // Assuming you have a method for this
            float caloriesBurned = calorieCalculator.CalculateCaloriesBurned();
            heartRateText.text = $"Average Heart Rate: {averageHeartRate:F1} bpm";
            caloriesBurnedText.text = $"Calories Burned: {caloriesBurned:F1} kcal";
        }
    }
}
