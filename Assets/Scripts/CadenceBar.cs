using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CadenceBar : MonoBehaviour
{
    public Slider alertBar; // Assign this in the Inspector
    public float alertDecayRate = 1.0f; // Rate at which the alert decreases per second
    public float minCadenceThreshold = 50f; // Minimum cadence to maintain alert bar
    public float maxCadenceThreshold = 200; // Maximum cadence
    public float targetCadence = 180.9f; // Target cadence value
    public Image fill; // Fill image of the slider
    public Gradient gradient; // Gradient for color change
    public float smoothTime = 0.5f; // Time it takes to smooth the transition

    private float currentCadence;

    void Start()
    {
        if (alertBar == null)
        {
            Debug.LogError("Alert bar is not assigned.");
            return;
        }

        currentCadence = 0;
        alertBar.value = currentCadence;
        fill.color = gradient.Evaluate(0f); // Start with the lowest value on the gradient

    }

    void Update()
    {
        if (CadenceDataManager.Instance == null)
        {
            Debug.LogError("CadenceDataManager instance is not available.");
            return;
        }

        float newCadence = CadenceDataManager.Instance.GetCadenceData();
        Debug.LogError("new cad here." +newCadence);
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
