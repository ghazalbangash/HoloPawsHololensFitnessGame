using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

#if !UNITY_EDITOR
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using System.Threading.Tasks;
#endif

public class MainServer : MonoBehaviour
{
    public TMP_Text logging;
    public TMP_Text cadence;
    public PlayerTracker playerTracker;
    public CaloryCalculator caloriesManager;

#if !UNITY_EDITOR
    private StreamSocketListener listener;
    private string port = "9090";
#endif

    private int steps;
    private int age;
    private int Weight;

    void Start()
    {
#if !UNITY_EDITOR
        listener = new StreamSocketListener();
        listener.ConnectionReceived += OnConnectionReceived;
        StartListener();
#endif
    }

    public void OnStepsDataReceived(int newStepsData)
    {
        StepsDataManager.Instance.SetStepsData(newStepsData);

        if (playerTracker != null)
        {
            playerTracker.UpdateSteps(newStepsData);
        }
        else
        {
            Debug.LogError("PlayerTracker is null");
        }
    }
    public void OnCadenceDataReceived(int newCadenceData)
    {
        CadenceDataManager.Instance.SetCadenceData(newCadenceData);

        if (caloriesManager != null)
        {
            caloriesManager.UpdateCadence(newCadenceData);
        }
        else
        {
            Debug.LogError("CaloriesManager is null");
        }
    }

    public void getWeight(int weight)
    {
        if (caloriesManager != null)
        {
            caloriesManager.UpdateWeight(weight);
        }
        else
        {
            Debug.LogError("CaloriesManager is null");
        }
    }

    public void OnStepGoalReceived(int newStepGoal)
    {
        Debug.Log("Entered OnStepGoalReceived function with step goal: " + newStepGoal);
        if (playerTracker != null)
        {
            Debug.Log("PlayerTracker is not null");
            playerTracker.SetTotalStepGoal(newStepGoal);
        }
        else
        {
            Debug.LogError("PlayerTracker is null");
        }
    }



#if !UNITY_EDITOR
    private async void StartListener()
    {
        try
        {
            await listener.BindServiceNameAsync(port);
            logging.text = "Listener started on port " + port;
            Debug.Log("Listener started on port " + port);
        }
        catch (Exception e)
        {
            logging.text = "Failed to start listener: " + e.Message;
            Debug.LogError("Failed to start listener: " + e.Message);
            Debug.LogError("Exception details: " + e.ToString());
        }
    }

    private async void OnConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
    {
        var dw = new DataWriter(args.Socket.OutputStream);
        var dr = new DataReader(args.Socket.InputStream);

        try
        {
            while (true)
            {
                dw.WriteString("Hello from HoloLens");
                await dw.StoreAsync();
                
                dr.InputStreamOptions = InputStreamOptions.Partial;
                await dr.LoadAsync(1024);
                string response = dr.ReadString(dr.UnconsumedBufferLength);
                Debug.Log("Received: " + response);
                
                UnityEngine.WSA.Application.InvokeOnAppThread(() =>
                {
                    logging.text = "Received: " + response;
                    string[] dataParts = response.Split(',');

                    Debug.Log("Data parts length: " + dataParts.Length);
                    for (int i = 0; i < dataParts.Length; i++)
                    {
                        Debug.Log($"Data part {i}: {dataParts[i]}");
                    }

                    // Handle user data (steps, age, height)
                    if (dataParts.Length == 3 &&
                        dataParts[0].StartsWith("Steps:") &&
                        dataParts[1].StartsWith(" Age:") &&
                        dataParts[2].StartsWith(" Weight:"))
                    {
                        Debug.Log("Entered user data handling if statement");
                        string stepsStr = dataParts[0].Replace("Steps:", "").Trim();
                        string ageStr = dataParts[1].Replace(" Age:", "").Trim();
                        string WeightStr = dataParts[2].Replace(" Weight:", "").Trim();

                        Debug.Log($"Parsed values - Steps: {stepsStr}, Age: {ageStr}, Weight: {WeightStr}");

                        if (int.TryParse(stepsStr, out steps) &&
                            int.TryParse(ageStr, out age) &&
                            int.TryParse(WeightStr, out Weight))
                        {
                            //logging.text += $"\nUser Data - Steps: {steps}, Age: {age}, Weight: {Weight}";
                            Debug.Log($"User Data - Steps: {steps}, Age: {age}, Weight: {Weight}");
                            OnStepGoalReceived(steps);
                        }
                        else
                        {
                            logging.text = "Invalid user data received: " + response;
                            Debug.LogError("Invalid user data received: " + response);
                        }
                    }
                    // Handle step count and cadence data
                    else if (dataParts.Length == 2 &&
                            int.TryParse(dataParts[0], out int stepsData1) &&
                            double.TryParse(dataParts[1], out double cadenceData1))
                    {
                        Debug.Log("Entered step count and cadence data handling if statement");
                        logging.text = stepsData1.ToString();
                        cadence.text = cadenceData1.ToString("F2");
                        OnStepsDataReceived(stepsData1);
                        OnCadenceDataReceived((int)cadenceData1); // Casting to int if necessary
                    }
                    // Combined data format: User data and step count/cadence data
                    else if (dataParts.Length == 5 &&
                            dataParts[0].StartsWith("Steps:") &&
                            dataParts[1].StartsWith(" Age:") &&
                            dataParts[2].StartsWith(" Weight:") &&
                            int.TryParse(dataParts[3], out int stepsData2) &&
                            double.TryParse(dataParts[4], out double cadenceData2))
                    {
                        Debug.Log("Entered combined data handling if statement");
                        string stepsStr = dataParts[0].Replace("Steps:", "").Trim();
                        string ageStr = dataParts[1].Replace(" Age:", "").Trim();
                        string WeightStr = dataParts[2].Replace(" Weight:", "").Trim();

                        if (int.TryParse(stepsStr, out steps) &&
                            int.TryParse(ageStr, out age) &&
                            int.TryParse(WeightStr, out Weight))
                        {
                            //logging.text += $"\nUser Data - Steps: {steps}, Age: {age}, Weight: {Weight}";
                            Debug.Log($"User Data - Steps: {steps}, Age: {age}, Weight: {Weight}");
                            OnStepGoalReceived(steps);
                            getWeight(Weight);

                        }
                        else
                        {
                            logging.text = "Invalid user data received: " + response;
                            Debug.LogError("Invalid user data received: " + response);
                        }

                        OnStepsDataReceived(stepsData2);
                        OnCadenceDataReceived((int)cadenceData2); // Casting to int if necessary
                    }
                    else
                    {
                        logging.text = "Invalid data received: " + response;
                        Debug.LogError("Invalid data received: " + response);
                    }
                }, false);
            }
        }
        catch (Exception e)
        {
            logging.text = "Connection error: " + e.Message;
            Debug.LogError("Connection error: " + e.Message);
            Debug.LogError("Exception details: " + e.ToString());
        }
    }

#endif
}
