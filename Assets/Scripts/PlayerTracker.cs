using System.Collections;
using UnityEngine;
using TMPro;


public class PlayerTracker : MonoBehaviour
{
    private int totalSteps;
    private ActivityLevel currentActivityLevel;
    private bool levelCompleted;
    private bool isPopupVisible;
    private float popupTimer;

    public PlayerGoals playerGoals;
    public GameObject popupMessage;
    public TMP_Text popupText;
    public float popupDuration = 6.0f;
    private Logger logger;

    void Start()
    {
        // Set the initial conditions
        totalSteps = 0;
        currentActivityLevel = ActivityLevel.BriskWalking;
        levelCompleted = false;
        isPopupVisible = false;
        popupTimer = 0.0f;

        if (popupMessage != null)
        {
            popupMessage.SetActive(false);
        }
        else
        {
            Debug.LogError("Popup message GameObject not assigned.");
        }

        // Initialize the logger
        logger = new Logger("PlayerDataLog1.csv");
        if (logger == null)
        {
            Debug.LogError("Logger failed to initialize.");
        }
        else
        {
            logger.Log("Game 1 started");
        }

        // Log the player's position every second
        InvokeRepeating("LogPlayerPosition", 1f, 1f);
    }

    void Update()
    {
        if (isPopupVisible)
        {
            popupTimer += Time.deltaTime;
            if (popupTimer >= popupDuration)
            {
                HidePopupMessage();
                TransitionToNextLevel();
            }
        }
    }

    public void UpdateSteps(int newSteps)
    {
        totalSteps = newSteps; // Set the total steps to the latest received value
        logger.Log($"TotalSteps: {totalSteps}, ActivityLevel: {currentActivityLevel}, Time: {Time.time}");

        Debug.Log("Total steps: " + totalSteps);

        if (!levelCompleted)
        {
            CheckGoals();
        }
    }

    private void CheckGoals()
    {
        int previousSteps = playerGoals.GetStepsRequiredForPreviousLevels(currentActivityLevel);
        int stepsInCurrentLevel = totalSteps - previousSteps;

        int stepsRequiredForCurrentLevel = playerGoals.GetStepsRequiredForLevel(currentActivityLevel);

        if (stepsInCurrentLevel >= stepsRequiredForCurrentLevel)
        {
            Debug.Log($"Goal reached for {currentActivityLevel}");
            logger.Log($"GoalReached: {currentActivityLevel} at {totalSteps} steps, Time: {Time.time}");
            levelCompleted = true;
            ShowPopupMessage();
        }
    }

    private void ShowPopupMessage()
    {
        isPopupVisible = true;
        popupTimer = 0.0f;

        if (popupMessage != null)
        {
            popupMessage.SetActive(true);
            popupText.text = $"Goal reached for {currentActivityLevel}!";
            Debug.Log("Popup message activated");
            logger.Log($"PopupShown: {currentActivityLevel}, Time: {Time.time}");
        }
        else
        {
            Debug.LogError("Popup message GameObject is null");
        }
    }

    private void HidePopupMessage()
    {
        isPopupVisible = false;

        if (popupMessage != null)
        {
            popupMessage.SetActive(false);
            Debug.Log("Popup message deactivated");
            logger.Log($"PopupHidden: {currentActivityLevel}, Time: {Time.time}");
        }
    }

    private void TransitionToNextLevel()
    {
        currentActivityLevel = GetNextActivityLevel();
        levelCompleted = false;
        Debug.Log("Transitioned to next level: " + currentActivityLevel);
        logger.Log($"LevelTransition: {currentActivityLevel}, Time: {Time.time}");
    }

    private ActivityLevel GetNextActivityLevel()
    {
        switch (currentActivityLevel)
        {
            case ActivityLevel.BriskWalking:
                return ActivityLevel.Jogging;
            case ActivityLevel.Jogging:
                return ActivityLevel.Running;
            case ActivityLevel.Running:
                return ActivityLevel.Running; // Maintain Running if it's the final level
            default:
                return ActivityLevel.BriskWalking;
        }
    }

    public void SetTotalStepGoal(int totalStepGoal)
    {
        playerGoals = new PlayerGoals(totalStepGoal);
        Debug.Log("Total step goal set: " + playerGoals.TotalStepGoal);
        logger.Log($"TotalStepGoalSet: {totalStepGoal}, Time: {Time.time}");
    }

    private void LogPlayerPosition()
    {
        Vector3 position = transform.position;
        logger.Log($"PlayerPosition: {position}, Time: {Time.time}");
    }
}
