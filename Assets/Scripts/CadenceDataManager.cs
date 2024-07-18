using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CadenceDataManager : MonoBehaviour
{
    public static CadenceDataManager Instance { get; private set; }

    private int cadenceData;

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

    public virtual  void SetCadenceData(int newCadenceData)
    {
        cadenceData = newCadenceData;
        Debug.Log("Cadence Data Updated: " + cadenceData);
    }

    public virtual  int GetCadenceData()
    {
        return cadenceData;
    }
}
