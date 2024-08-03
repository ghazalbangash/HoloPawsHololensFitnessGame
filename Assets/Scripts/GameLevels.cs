using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ActivityLevel
{
    BriskWalking,
    Jogging,
    Running
}

public class GameLevels
{
    public ActivityLevel Activity { get; set; }
    public int StepsRequired { get; set; }
}

public class PlayerGoals
{
    public int TotalStepGoal { get; set; }
    public Dictionary<ActivityLevel, int> StepGoals { get; set; }

    public PlayerGoals(int totalStepGoal)
    {
        TotalStepGoal = totalStepGoal;
        StepGoals = new Dictionary<ActivityLevel, int>
        {
            { ActivityLevel.BriskWalking, (int)(totalStepGoal * 0.33) },
            { ActivityLevel.Jogging, (int)(totalStepGoal * 0.33) },
            { ActivityLevel.Running, (int)(totalStepGoal * 0.34) }
        };
    }

    public int GetStepsRequiredForLevel(ActivityLevel level)
    {
        return StepGoals.ContainsKey(level) ? StepGoals[level] : 0;
    }

    public int GetStepsRequiredForPreviousLevels(ActivityLevel level)
    {
        int steps = 0;
        foreach (var kvp in StepGoals)
        {
            if (kvp.Key < level)
            {
                steps += kvp.Value;
            }
        }
        return steps;
    }
}


