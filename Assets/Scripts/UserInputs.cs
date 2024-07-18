using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputs : MonoBehaviour
{
    [SerializeField]
    private TouchScreenKeyboard keyboard;
    private string keyboardText; 
    private string keyboardSteps; 
    private string keyboardWeight; 
    private string keyboardAge; 
    [SerializeField]
    private GameObject canvas; // Reference to the canvas GameObject

    [SerializeField]
    private ErrorDialog errorDialog; // Reference to the ErrorDialog script

    [SerializeField]
    private PlayerTracker playerTracker; // Reference to the PlayerTracker script

    public void OpenSystemKeyboard()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false);
    }

    private void Update()
    {
        if (keyboard != null)
        {
            keyboardText = keyboard.text;
            Debug.Log(keyboardText);
            // Do stuff with keyboardText
        }


        int currentStepsData = StepsDataManager.Instance.GetStepsData();
        Debug.Log("Current Steps Data: " + currentStepsData);
    }

    public void ReadInput()
    {
        keyboardText = keyboard.text;
        Debug.Log(keyboardText);
    }

    public void ReadWeight(string s)
    {
        keyboardWeight = s;
        Debug.Log(keyboardWeight);
        ValidateWeight(keyboardWeight);
    }

    public void ReadAge(string s)
    {
        keyboardAge = s;
        Debug.Log(keyboardAge);
        ValidateAge(keyboardAge);
    }

    public void ReadSteps(string s)
    {
        keyboardSteps = s;
        Debug.Log(keyboardSteps);
        ValidateSteps(keyboardSteps);
    }

    // Method to destroy the canvas
    public void CloseCanvas()
    {
        if (canvas != null)
        {
            Destroy(canvas);
        }
    }

    private bool ValidateWeight(string weight)
    {
        if (string.IsNullOrWhiteSpace(weight) || !float.TryParse(weight, out _))
        {
            Debug.LogError("Weight input is empty or invalid.");
            ShowErrorDialog("weight is empty or invalid.");
            return false;
        }
        return true;
    }

    private bool ValidateAge(string age)
    {
        if (string.IsNullOrWhiteSpace(age) || !int.TryParse(age, out int parsedAge) || parsedAge <= 0)
        {
            Debug.LogError("Age input is empty or invalid.");
            ShowErrorDialog("Age is empty or invalid.");
            return false;
        }
        return true;
    }

    private bool ValidateSteps(string steps)
    {
        if (string.IsNullOrWhiteSpace(steps) || !int.TryParse(steps, out int parsedSteps) || parsedSteps < 0)
        {
            Debug.LogError("Steps input is empty or invalid.");
            ShowErrorDialog("steps is empty or invalid.");
            return false;
        }
        return true;
    }

    private void ShowErrorDialog(string message)
    {
        if (errorDialog != null)
        {
            errorDialog.ShowDialog(message);
        }
    }

    // Method called by the okay button
    public void OnOkayButtonClick()
    {
        bool isWeightValid = ValidateWeight(keyboardWeight);
        bool isStepsValid = ValidateSteps(keyboardSteps);
        bool isAgeValid = ValidateAge(keyboardAge);

        if (isWeightValid && isStepsValid && isAgeValid)
        {
            Debug.Log("All inputs are valid. Closing canvas.");
            CloseCanvas();

            // Set the total step goal in the PlayerTracker
            if (playerTracker != null && int.TryParse(keyboardSteps, out int totalStepGoal))
            {
                playerTracker.SetTotalStepGoal(totalStepGoal);
            }
        }
    }
}
