using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Photon.PunBehaviour {

    static public GameManager Instance;
    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;

    private bool red;
    private bool blue;
    private bool green;

    #region Photon Messages

    void Start()
    {
        /*Instance = this;

        if (playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            Debug.Log("We are Instantiating LocalPlayer from " + Application.loadedLevelName);
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            if (PlayerManager.LocalPlayerInstance == null)
            {
                Debug.Log("We are Instantiating LocalPlayer from " + Application.loadedLevelName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 1.5f, 0f), Quaternion.identity, 0);
            }
            else
            {
                Debug.Log("Ignoring scene load for " + Application.loadedLevelName);
            }
        }*/
    }

    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer other)
    {
        //Debug.Log("OnPhotonPlayerConnected() " + other.NickName); // not seen if you're the player connecting
        if (PhotonNetwork.isMasterClient)
        {
            
        }

    }


    public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
    {
        //Debug.Log("OnPhotonPlayerDisconnected() " + other.NickName); // seen when other disconnects


        if (PhotonNetwork.isMasterClient)
        {
            
        }
    }


    #endregion


    #region Public Methods


    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    #endregion

    #region Private Methods


    void LoadArena()
    {
        /*if (!PhotonNetwork.isMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }*/
        //Debug.Log("PhotonNetwork : Loading Level : " + PhotonNetwork.room.PlayerCount);
        PhotonNetwork.LoadLevel("Prototype Scene - PreMaster");

        /*int viewId = PhotonNetwork.AllocateViewID();

        PhotonNetwork.RaiseEvent(123, viewId, true, new RaiseEventOptions()
        {
            CachingOption = EventCaching.AddToRoomCache,
            Receivers = ReceiverGroup.All
        });

        Debug.Log("spawning from LoadArena"); */
    }



    #endregion
}
