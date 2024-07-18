using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CadenceBar : MonoBehaviour
{
    public Slider alertBar; // Assign this in the Inspector
    public float alertDecayRate = 1.0f; // Rate at which the alert decreases per second
    public float minCadenceThreshold = 50f; // Minimum cadence to maintain alert bar
    public float maxCadenceThreshold = 300f; // Maximum cadence
    public float targetCadence = 140.9f; // Target cadence value
    public Image fill;
    public Gradient gradient;
    public float smoothTime = 0.5f; // Time it takes to smooth the transition

    private float currentAlertValue;
    private bool isAlerting;
    private float currentCadence;

    void Start()
    {
        if (alertBar == null)
        {
            Debug.LogError("Alert bar is not assigned.");
            return;
        }

        currentAlertValue = alertBar.maxValue;
        alertBar.value = currentAlertValue;
        fill.color = gradient.Evaluate(1f);
    }

    void Update()
    {
        if (CadenceDataManager.Instance == null)
        {
            Debug.LogError("CadenceDataManager instance is not available.");
            return;
        }

        float newCadence = CadenceDataManager.Instance.GetCadenceData();
        if (newCadence != currentCadence)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothTransition(newCadence));
        }
    }

    private IEnumerator SmoothTransition(float newCadence)
    {
        currentCadence = newCadence;

        while (Mathf.Abs(alertBar.value - newCadence) > 0.01f)
        {
            alertBar.value = Mathf.Lerp(alertBar.value, newCadence, smoothTime * Time.deltaTime);
            fill.color = gradient.Evaluate(alertBar.normalizedValue);
            yield return null;
        }

        alertBar.value = newCadence;
        fill.color = gradient.Evaluate(alertBar.normalizedValue);
    }

    private void TriggerAlert()
    {
        Debug.Log("Alert: Cadence too low!");
        // Additional alert logic (e.g., show warning message, play sound, etc.)
    }

    public void SetCad(float cad)
    {
        alertBar.value = cad;
        fill.color = gradient.Evaluate(alertBar.normalizedValue);
    }


}
