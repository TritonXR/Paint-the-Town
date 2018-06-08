using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Launcher : Photon.PunBehaviour
{
    #region Public Variables

    public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;
    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    public byte MaxPlayersPerRoom = 4;


    /// <summary>
    /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon, 
    /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
    /// Typically this is used for the OnConnectedToMaster() callback.
    /// </summary>
    bool isConnecting;
    public readonly byte InstantiateVrAvatarEventCode = 123;

    #endregion

    #region Private Variables

    private bool red;
    private bool blue;
    private bool green;

    /// <summary>
    /// This client's version number. Users are separated from each other by gameversion (which allows you to make breaking changes).
    /// </summary>
    string _gameVersion = "1";

    public SpawnLocations spawnScript;

    public GameObject singlePlayer;
    public GameObject mainMenu;
    public GameObject logo;
    public GameObject credits;
    public GameObject controls;


    #endregion


    #region MonoBehaviour CallBacks


    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
    /// </summary>
    void Awake()
    {
        PhotonNetwork.logLevel = Loglevel;

        // #Critical
        // we don't join the lobby. There is no need to join a lobby to get the list of rooms.
        PhotonNetwork.autoJoinLobby = false;

        //Debug.Log(PhotonNetwork.countOfPlayers);

        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.automaticallySyncScene = true;
    }


    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during initialization phase.
    /// </summary>
    void Start()
    {
    }


    #endregion


    #region Public Methods


    /// <summary>
    /// Start the connection process. 
    /// - If already connected, we attempt joining a random room
    /// - if not yet connected, Connect this application instance to Photon Cloud Network
    /// </summary>
    public void Connect()
    {
        isConnecting = true;


        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.connected)
        {
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnPhotonRandomJoinFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            // #Critical, we must first and foremost connect to Photon Online Server.
            PhotonNetwork.ConnectUsingSettings(_gameVersion);
        }
    }


    #endregion

    #region Photon.PunBehaviour CallBacks


    public override void OnConnectedToMaster()
    {
        if(isConnecting)
            PhotonNetwork.JoinRandomRoom();
        //Debug.Log("DemoAnimator/Launcher: OnConnectedToMaster() was called by PUN");
    }


    public override void OnDisconnectedFromPhoton()
    {
       // Debug.LogWarning("DemoAnimator/Launcher: OnDisconnectedFromPhoton() was called by PUN");
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        //Debug.Log("DemoAnimator/Launcher:OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = MaxPlayersPerRoom }, null);
        PhotonNetwork.LoadLevel("Lobby");
    }

    public override void OnJoinedRoom()
    {
        // #Critical: We only load if we are the first player, else we rely on  PhotonNetwork.automaticallySyncScene to sync our instance scene

        //PhotonNetwork.LoadLevel("Prototype Scene - PreMaster");

        // Debug.Log("DemoAnimator/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");

        PhotonNetwork.Destroy(singlePlayer);
        //singlePlayer.SetActive(false);

        mainMenu.SetActive(false);
        logo.SetActive(false);
        credits.SetActive(false);
        controls.SetActive(false);
        

        int viewId = PhotonNetwork.AllocateViewID();

        PhotonNetwork.RaiseEvent(InstantiateVrAvatarEventCode, viewId, true, new RaiseEventOptions()
        {
            CachingOption = EventCaching.AddToRoomCache,
            Receivers = ReceiverGroup.All
        });



        //Debug.Log("spawning from OnJoinedRoom");

    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(0);
    }

    private void OnEvent(byte eventcode, object content, int senderid)
    {
        //Debug.Log("VR SPAWN CODE ACTIVATED \n" + eventcode );

        if (eventcode == InstantiateVrAvatarEventCode)
        {
            //Debug.Log("VR SPAWN CODE ACTIVATED");
            GameObject go = null;

            if (PhotonNetwork.player.ID == senderid)
            {
                go = Instantiate(Resources.Load("OVRCameraLocal")) as GameObject;
            }
            else
            {
                go = Instantiate(Resources.Load("OVRCameraRemote")) as GameObject;
            }

            if (go != null)
            {
                PhotonView pView = go.GetComponent<PhotonView>();

                if (pView != null)
                {
                    pView.viewID = (int)content;
                }
            }

            if(spawnScript != null)
            {
                if(PhotonNetwork.playerList[0].ID == senderid)
                {
                    go.transform.position = spawnScript.spawnPos[0];
                    go.transform.Rotate(spawnScript.spawnRots[0]);
                    go.tag = "PlayerRed";
                }
                else if (PhotonNetwork.playerList[1].ID == senderid)
                {
                    go.transform.position = spawnScript.spawnPos[1];
                    go.transform.Rotate(spawnScript.spawnRots[1]);
                    go.tag = "PlayerBlue";
                }
                else 
                {
                    go.transform.position = spawnScript.spawnPos[2];
                    go.transform.Rotate(spawnScript.spawnRots[2]);
                    go.tag = "PlayerGreen";
                }
            }
        }
    }

    public void OnEnable()
    {
        PhotonNetwork.OnEventCall += this.OnEvent;
    }

    public void OnDisable()
    {
        PhotonNetwork.OnEventCall -= this.OnEvent;
    }

    /*[PunRPC]
    void paintWithTex(byte[] tex, string objectID, string color)
    {
        GameObject dummy = GameObject.Find(objectID);

        Material mat = dummy.GetComponent<Material>();

        Texture2D texCopy = (Texture2D)GameObject.Instantiate(mat.GetTexture(color));

        texCopy.LoadRawTextureData(tex);

        texCopy.Apply();

        mat.SetTexture(color, texCopy);
    }*/



    #endregion
}
