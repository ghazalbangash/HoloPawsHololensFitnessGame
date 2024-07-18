using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulateCadenceData : CadenceDataManager
{
    public float minCadence = 0;
    public float maxCadence = 150;
    public float simulationInterval = 1.0f;

    private float simulatedCadence;
    private Coroutine simulationCoroutine;

    private void OnEnable()
    {
        if (simulationCoroutine == null)
        {
            simulationCoroutine = StartCoroutine(SimulateCadence1());
        }
    }

    private void OnDisable()
    {
        if (simulationCoroutine != null)
        {
            StopCoroutine(simulationCoroutine);
            simulationCoroutine = null;
        }
    }

    private IEnumerator SimulateCadence1()
    {
        while (true)
        {
            simulatedCadence = Random.Range(minCadence, maxCadence);
            yield return new WaitForSeconds(simulationInterval);
        }
    }

    public override int GetCadenceData()
    {
        return (int)simulatedCadence;
    }

}
