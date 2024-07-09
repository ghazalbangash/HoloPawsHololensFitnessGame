using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepsDataManager : MonoBehaviour
{
    // Singleton instance
    private static StepsDataManager _instance;

    // Property to access the singleton instance
    public static StepsDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("StepsDataManager");
                _instance = obj.AddComponent<StepsDataManager>();
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }

    // Steps data
    private int stepsData;

    // Method to set steps data
    public void SetStepsData(int steps)
    {
        stepsData = steps;
    }

    // Method to get steps data
    public int GetStepsData()
    {
        return stepsData;
    }
}
