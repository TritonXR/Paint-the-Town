using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCharacters : Photon.PunBehaviour {

    public readonly byte InstantiateVrAvatarEventCode = 123;

    // Use this for initialization
    void Start () {

        int viewId = PhotonNetwork.AllocateViewID();

        PhotonNetwork.RaiseEvent(InstantiateVrAvatarEventCode, viewId, true, new RaiseEventOptions()
        {
            CachingOption = EventCaching.AddToRoomCache,
            Receivers = ReceiverGroup.All
        });


    }

    // Update is called once per frame
    void Update () {
		
	}
}
