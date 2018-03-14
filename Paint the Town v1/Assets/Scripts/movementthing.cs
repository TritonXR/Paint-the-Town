using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementthing : MonoBehaviour {

    public string test;

    GameObject player;
    public Camera headsetCamera;
    public GameObject lefthand;
    public GameObject righthand;
    // Use this for initialization
    void Start()
    {
        player = this.gameObject;
    }



    // Update is called once per frame
    void Update()
    {

        OVRInput.Update();
        test = OVRInput.IsControllerConnected(OVRInput.Controller.RTrackedRemote).ToString();
        Vector2 rightInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        Vector2 leftInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        
        Vector3 newMove = Vector3.zero;
        
        //forward movement
        if (rightInput.y > 0.3)
            newMove += righthand.transform.forward;
        if (leftInput.y > 0.3)
            newMove += lefthand.transform.forward;

        if (rightInput.y < -0.3)
            newMove += -righthand.transform.forward;
        if (leftInput.y < -0.3)
            newMove += -lefthand.transform.forward;

        if (rightInput.x > 0.3)
            newMove += righthand.transform.right;
        if (leftInput.x > 0.3)
            newMove += lefthand.transform.right;

        if (rightInput.x < -0.3)
            newMove += -righthand.transform.right;
        if (leftInput.x < -0.3)
            newMove += -lefthand.transform.right;

        newMove.y = 0;

        player.transform.Translate(newMove * Time.deltaTime * 1f);


        /*if (OVRInput.Get(OVRInput.Touch.PrimaryIndexTrigger))
        {
            Vector3 start = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
            Vector3 ray = SampleCurve(OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger)), 
            Physics.Raycast()
        } */


    }
}
