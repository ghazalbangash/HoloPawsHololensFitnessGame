using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ErrorDialog : MonoBehaviour
{
    [SerializeField]
    private GameObject dialogPanel;
    [SerializeField]
    private TextMeshProUGUI dialogText;
    [SerializeField]
    private Button closeButton;

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
        if (closeButton == null)
        {
            Debug.LogError("Close Button is not assigned in the inspector.");
        }

        closeButton.onClick.AddListener(HideDialog);
        HideDialog(); // Initially hide the dialog
    }

    public void ShowDialog(string message)
    {
        if (dialogText != null && dialogPanel != null)
        {
            dialogText.text = message;
            dialogPanel.SetActive(true);
            Debug.Log("Showing dialog with message: " + message);
        }
        else
        {
            Debug.LogError("DialogText or DialogPanel is not assigned.");
        }
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
}