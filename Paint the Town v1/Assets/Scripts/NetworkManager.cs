using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    //typeName must be unique to avoid clash with other projects
    private const string typeName = "PainttheTown";
    private const string gameName = "TestRoom";

    //stores all info req to join server from master server
    private HostData[] hostList;
    public GameObject playerPrefab;

    private void StartServer()
    {
        Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
        MasterServer.RegisterHost(typeName, gameName);
    }

    private void OnServerInitialized()
    {
        //Debug.Log("Server Initialized");
        SpawnPlayer();
    }

    private void OnGUI()
    {
        if(!Network.isClient && !Network.isServer)
        {
            if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
            {
                StartServer();
            }
            if(GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
            {
                RefreshHostList();
            }
            if(hostList != null)
            {
                for(int i = 0; i < hostList.Length; i++)
                {
                    if(GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
                    {
                        JoinServer(hostList[i]);
                    }
                }
            }
        }
    }

    private void RefreshHostList()
    {
        MasterServer.RequestHostList(typeName);
    }

    private void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if(msEvent == MasterServerEvent.HostListReceived)
        {
            hostList = MasterServer.PollHostList();
        }
    }

    private void JoinServer(HostData hostData)
    {
        Network.Connect(hostData);
    }

    private void OnConnectedToServer()
    {
        //Debug.Log("Server Joined");
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        Network.Instantiate(playerPrefab, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
    }

    private void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        playerPrefab.SetActive(false);
    }
}
