using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaloryCalculator : MonoBehaviour
{
    public float userWeight; // in kg
    public float userHeight; // in cm (not used in this calculation but can be used for more accurate calculations)
    public float restingHeartRate = 70f; // average resting heart rate
    public float maxHeartRate; // should be set based on the user's age
    private float met = 8.0f; // MET value for running
    private float timeElapsed = 0.0f; // total time in hours
    private float averageHeartRate = 0.0f;

    public void UpdateHeartRate(float newHeartRate, float deltaTime)
    {
        // Update average heart rate (you can improve this by keeping a history of heart rates)
        averageHeartRate = (averageHeartRate + newHeartRate) / 2;
        timeElapsed += deltaTime / 3600.0f; // convert deltaTime to hours
    }

    public float CalculateCaloriesBurned()
    {
        return ((averageHeartRate - restingHeartRate) / (maxHeartRate - restingHeartRate)) * met * userWeight * timeElapsed;
    }

    public float GetAverageHeartRate(){
        Debug.Log("test");
        return 0.0f;
    }
}
