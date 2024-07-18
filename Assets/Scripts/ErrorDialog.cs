using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ErrorDialog : MonoBehaviour
{
    [SerializeField]
    private GameObject dialogPanel;
    [SerializeField]
    private TextMeshProUGUI dialogText;
    [SerializeField]

    private Coroutine autoCloseCoroutine; // To manage the auto-close coroutine

    private void Start()
    {
        if (dialogPanel == null)
        {
            Debug.LogError("Dialog Panel is not assigned in the inspector.");
        }
        if (dialogText == null)
        {
            Debug.LogError("Dialog Text is not assigned in the inspector.");
        }
 
        HideDialog(); // Initially hide the dialog
    }

    public void ShowDialog(string message)
    {
        if (dialogText != null && dialogPanel != null)
        {
            dialogText.text = message;
            dialogPanel.SetActive(true);
            Debug.Log("Showing dialog with message: " + message);

            // Start the coroutine to auto-close the dialog
            if (autoCloseCoroutine != null)
            {
                StopCoroutine(autoCloseCoroutine);
            }
            autoCloseCoroutine = StartCoroutine(AutoCloseDialog(3f)); // Close after 3 seconds
        }
        else
        {
            Debug.LogError("DialogText or DialogPanel is not assigned.");
        }
    }

    private IEnumerator AutoCloseDialog(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideDialog();
    }

    public void HideDialog()
    {
        if (dialogPanel != null)
        {
            dialogPanel.SetActive(false);
            Debug.Log("Hiding dialog");
        }
        else
        {
            Debug.LogError("DialogPanel is not assigned.");
        }
    }

    private void OnDestroy()
    {
        // Stop the coroutine if the object is destroyed before the dialog auto-closes
        if (autoCloseCoroutine != null)
        {
            StopCoroutine(autoCloseCoroutine);
        }
    }
}
