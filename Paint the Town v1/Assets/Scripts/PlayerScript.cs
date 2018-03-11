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
    void Update () {
        if (!isLocalPlayer) {
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

    Vector3 SampleCurve(Vector3 start, Vector3 end, Vector3 control, float t)
    {
        // Interpolate along line S0: control - start;
        Vector3 Q0 = Vector3.Lerp(start, control, t);
        // Interpolate along line S1: S1 = end - control;
        Vector3 Q1 = Vector3.Lerp(control, end, t);
        // Interpolate along line S2: Q1 - Q0
        Vector3 Q2 = Vector3.Lerp(Q0, Q1, t);

      return Q2; // Q2 is a point on the curve at time t
    }
}
