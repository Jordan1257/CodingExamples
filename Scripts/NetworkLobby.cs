//2017 Jordan Black

//Description: Controls high-level implementation of networking services, uses Console.cs for text communication.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkLobby : NetworkBehaviour {

    int connectionId;
    int channelId;
    int hostId;

    private Console console;

    public bool isActive;

    private void Start()
    {
        console = GetComponent<Console>();
    }

    public void BeginService()
    {
        NetworkTransport.Init();

        // Create a connection config and add a Channel.
        ConnectionConfig config = new ConnectionConfig();
        channelId = config.AddChannel(QosType.Reliable);

        // Create a topology based on the connection config.
        HostTopology topology = new HostTopology(config, 10);

        // Create a host based on the topology we just created, and bind the socket to port 12345.
        hostId = NetworkTransport.AddHost(topology, 55155);

        isActive = true;
    }

    public void StopService()
    {
        NetworkTransport.Shutdown();

        isActive = false;
    }
   
    [Command]
    public void CmdSendConsoleMessage(string msg)
    {
        console.CreateConsoleText(msg);
    }


    public void JoinLobby()
    {
        byte error;
        connectionId = NetworkTransport.Connect(hostId, "172.23.150.107", 55155, 0, out error);
    }

    void Update()
    {

        if (isActive)
        {

            int outHostId;
            int outConnectionId;
            int outChannelId;
            byte[] buffer = new byte[1024];
            int bufferSize = 1024;
            int receiveSize;
            byte error;


            NetworkEventType evnt = NetworkTransport.Receive(out outHostId, out outConnectionId, out outChannelId, buffer, bufferSize, out receiveSize, out error);
            switch (evnt)
            {
                case NetworkEventType.ConnectEvent:
                    if (outHostId == hostId &&
                       outConnectionId == connectionId &&
                       (NetworkError)error == NetworkError.Ok)
                    {
                        Debug.Log("Connected");
                    }
                    break;
                case NetworkEventType.DisconnectEvent:
                    if (outHostId == hostId &&
                       outConnectionId == connectionId)
                    {
                        Debug.Log("Connected, error:" + error.ToString());
                    }
                    break;
            }

        }

    }



}
