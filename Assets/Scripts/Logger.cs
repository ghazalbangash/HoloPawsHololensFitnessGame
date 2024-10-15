using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class Logger
{
    private string filePath;

    public Logger(string fileName)
    {
        // Always use Application.persistentDataPath to ensure compatibility across all platforms
        InitializeFilePath(fileName);
    }

    private void InitializeFilePath(string fileName)
    {
        filePath = Path.Combine(Application.persistentDataPath, fileName);
        Debug.Log("Log file path: " + filePath);

        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "Timestamp,Data\n");
            Debug.Log("Log file created at: " + filePath);
        }
    }

    public void Log(string data)
    {
        LogAsync(data);
    }

    private async void LogAsync(string data)
    {
        try
        {
            string logEntry = $"{System.DateTime.Now},{data}\n";
            await AppendTextAsync(logEntry);
            Debug.Log("Logged data: " + logEntry);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to log data: " + ex.Message);
        }
    }

    private async Task AppendTextAsync(string text)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                await writer.WriteAsync(text);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to append text: " + ex.Message);
        }
    }
}
