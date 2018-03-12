using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerScript : NetworkBehaviour {

    public string test;

    GameObject player;
    Camera headsetCamera;
	// Use this for initialization
	void Start () {
        player = this.gameObject;
        headsetCamera = Camera.main;
        
    }

    public override void OnStartLocalPlayer()
    {
        player.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            OVRInput.Update();
            test = OVRInput.IsControllerConnected(OVRInput.Controller.RTrackedRemote).ToString();
            Vector2 moveInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

            Vector3 newMove = Vector3.zero;
            newMove.z = moveInput.y;
            newMove.x = moveInput.x;

            player.transform.Translate(newMove * Time.deltaTime * 1f);

            /*if (OVRInput.Get(OVRInput.Touch.PrimaryIndexTrigger))
            {
                Vector3 start = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
                Vector3 ray = SampleCurve(OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger)), 
                Physics.Raycast()
            } */
        }

    }

}
