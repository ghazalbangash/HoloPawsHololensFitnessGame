using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaloryCalculator : MonoBehaviour
{
    public float weightInKg; // User's weight
    private float totalCaloriesBurned;
    private float personalBestCalories;
    private int currentCadence;

    public ProximitySign signManager;

    // MET values based on cadence
    private float GetMetValue(int cadence)
    {
        if (cadence < 100) return 3.5f; // Brisk walking
        if (cadence < 150) return 5.0f; // Jogging
        return 8.0f; // Running
    }

    void Update()
    {
        currentCadence = CadenceDataManager.Instance.GetCadenceData();

        if (currentCadence > 0)
        {
            float timeInMinutes = Time.deltaTime / 60;
            float metValue = GetMetValue(currentCadence);
            float caloriesBurnedPerMinute = metValue * weightInKg * 0.0175f;
            totalCaloriesBurned += caloriesBurnedPerMinute * timeInMinutes;

            if (totalCaloriesBurned > personalBestCalories)
            {
                personalBestCalories = totalCaloriesBurned;
            }

            if (signManager != null)
            {
                signManager.UpdateSignText($"Personal Best: {personalBestCalories:F1} Calories");
            }
        }
    }

    public void UpdateCadence(int cadence)
    {
        currentCadence = cadence;
        Debug.Log("cadence sent " + currentCadence);
    }

   public void UpdateWeight(float newWeight)
    {
        weightInKg = newWeight;
        Debug.Log("Weight updated to: " + newWeight);
    }

    public void StopActivity()
    {
        currentCadence = 0;
        Debug.Log("Activity stopped, cadence reset to 0.");
    }
    
}
