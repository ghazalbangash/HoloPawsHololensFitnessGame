using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    private int currentSteps;
    private float currentSpeed;
    private ActivityLevel currentActivityLevel;

    public PlayerGoals playerGoals;
    public GameLevels[] gameLevels;

    private SpeedTracker speedTracker;

    // Test Variables
    public int stepsIncrement = 1; // Number of steps to increment each update
    public float incrementInterval = 1.0f; // Interval in seconds between increments
    private float timeSinceLastIncrement = 0.0f;

    void Start()
    {
        // Initialize with a default total step goal
        SetTotalStepGoal(1000);

        currentSteps = 0;
        currentSpeed = 0.0f;
        currentActivityLevel = ActivityLevel.BriskWalking;

        // Get the SpeedTracker component
        speedTracker = GetComponent<SpeedTracker>();

        // Ensure the SpeedTracker component exists
        if (speedTracker == null)
        {
            Debug.LogError("SpeedTracker component not found on GameObject.");
        }
    }

    void Update()
    {
        if (speedTracker != null)
        {
            // Update current speed using SpeedTracker
            currentSpeed = speedTracker.GetCurrentSpeed();
        }

        // Determine current activity level based on speed
        currentActivityLevel = GetCurrentActivityLevel(currentSpeed);

        // Update steps (this should come from your step tracking system)

        // Simulate step data for testing
        //SimulateSteps();

        currentSteps = StepsDataManager.Instance.GetStepsData();

        // Check if the player has reached the step goal for the current activity
        CheckGoals();
    }

    public void SetTotalStepGoal(int totalStepGoal)
    {
        playerGoals = new PlayerGoals(totalStepGoal);

        // Define game levels based on the new total step goal
        gameLevels = new GameLevels[]
        {
            new GameLevels { Activity = ActivityLevel.BriskWalking, MinSpeed = 1.0f, MaxSpeed = 3.0f, StepsRequired = playerGoals.StepGoals[ActivityLevel.BriskWalking] },
            new GameLevels { Activity = ActivityLevel.Jogging, MinSpeed = 3.1f, MaxSpeed = 6.0f, StepsRequired = playerGoals.StepGoals[ActivityLevel.Jogging] },
            new GameLevels { Activity = ActivityLevel.Running, MinSpeed = 6.1f, MaxSpeed = 10.0f, StepsRequired = playerGoals.StepGoals[ActivityLevel.Running] }
        };
    }

    private ActivityLevel GetCurrentActivityLevel(float speed)
    {
        foreach (var level in gameLevels)
        {
            if (speed >= level.MinSpeed && speed <= level.MaxSpeed)
            {
                return level.Activity;
            }
        }
        return ActivityLevel.BriskWalking;
    }

    private void CheckGoals()
    {
        if (currentSteps >= playerGoals.StepGoals[currentActivityLevel])
        {
            Debug.Log($"Goal reached for {currentActivityLevel}");
            // Implement logic for moving to the next level or completing the game
        }
    }

    // Temporary function to simulate steps increment for testing
    private void SimulateSteps()
    {
        timeSinceLastIncrement += Time.deltaTime;
        if (timeSinceLastIncrement >= incrementInterval)
        {
            currentSteps += stepsIncrement;
            timeSinceLastIncrement = 0.0f;
            Debug.Log("Current Steps: " + currentSteps);
        }
    }
}
