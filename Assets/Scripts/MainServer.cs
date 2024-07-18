using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

using UnityEngine.UI;



#if !UNITY_EDITOR
    using Windows.Networking.Sockets;
    using Windows.Storage.Streams;
    using System.Threading.Tasks;
#endif

public class MainServer : MonoBehaviour
{
    public TMP_Text logging;

#if !UNITY_EDITOR
    private StreamSocketListener listener;
    private string port = "9090";
#endif

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
    }

    public void OnCadenceDataReceived(int newCadenceData)
    {
        CadenceDataManager.Instance.SetCadenceData(newCadenceData);
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
                    
                    if (dataParts.Length == 2 &&
                        int.TryParse(dataParts[0], out int stepsData) &&
                        double.TryParse(dataParts[1], out double cadenceData))
                    {
                        OnStepsDataReceived(stepsData);
                        OnCadenceDataReceived((int)cadenceData); // Casting to int if necessary
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