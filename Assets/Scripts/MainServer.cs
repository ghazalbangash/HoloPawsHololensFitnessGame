using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

using UnityEngine.UI;


#if !UNITY_EDITOR
    using Windows.Networking;
    using Windows.Networking.Sockets;
    using Windows.Storage.Streams;
    using System.Threading.Tasks;
#endif

public class MainServer : MonoBehaviour
{
    public String _input = "Waiting";
    public String data = "test";

    public TMP_Text HeartRate;
    public TMP_Text Steps;


    public float speed = 5f; // Speed at which the dog follows the player
    
    public TMP_Text logging;


#if !UNITY_EDITOR
        StreamSocket socket;
        StreamSocketListener listener;
        String port;
        String message;

#endif

    // Use this for initialization
    void Start()
    {
        

#if !UNITY_EDITOR
        logging.text = "enters";
        Debug.Log("it enters here");
        listener = new StreamSocketListener();
        port = "9090";
        listener.ConnectionReceived += Listener_ConnectionReceived;
        listener.Control.KeepAlive = false;

        Listener_Start();
#endif
    }





#if !UNITY_EDITOR
    private async void Listener_Start()
    {
        Debug.Log("Listener started");
        try
        {
            await listener.BindServiceNameAsync(port);
            logging.text = "listener started here";
        }
        catch (Exception e)
        {
            Debug.Log("Error: " + e.Message);
        }

        Debug.Log("Listening");
    }

    private async void Listener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
    {
        Debug.Log("Connection received");
        var dw = new DataWriter(args.Socket.OutputStream);
        var dr = new DataReader(args.Socket.InputStream);

        try
        {
            while (true) {
                

                    dw.WriteString("Ghazal here");
                    await dw.StoreAsync();
                    //dw.DetachStream();
                    Debug.Log("end sedning");
                    
                    


                    //DataReader reader = new DataReader(sender.InputStream);
                    dr.InputStreamOptions = InputStreamOptions.Partial;
                    await dr.LoadAsync(5024);
                    string response = dr.ReadString(dr.UnconsumedBufferLength);
                    Debug.Log(response);
                    data = response;

                    

            }
        }
        catch (Exception e)
        {
            Debug.Log("disconnected!!!!!!!! " + e);
        }

    }

#endif


    // private void Update(){

    //     Debug.Log("here is the canvas position"+data );
    // }
}
