using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CadenceDataManager : MonoBehaviour
{
    public static CadenceDataManager Instance { get; private set; }

    private int cadenceData;
    private float lastUpdateTime;
    private float inactivityThreshold = 5f; // Time in seconds to consider as inactivity

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCadenceData(int newCadenceData)
    {
        cadenceData = newCadenceData;
        lastUpdateTime = Time.time;
        Debug.Log("Cadence Data Updated: " + cadenceData);
    }

    public int GetCadenceData()
    {
        if (Time.time - lastUpdateTime > inactivityThreshold)
        {
            cadenceData = 0; // Reset cadence if inactive for too long
        }
        return cadenceData;
    }
}
