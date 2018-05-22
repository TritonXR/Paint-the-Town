using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementthing : MonoBehaviour {

    public string test;

    GameObject player;
    public float speed;
    public Camera headsetCamera;
    public GameObject lefthand;
    public GameObject righthand;
    private LineRenderer lineR;
    private LineRenderer lineL;
    // Use this for initialization
    void Start()
    { 
            player = this.gameObject;
            speed = 0.5f;
            lineL = lefthand.GetComponent<LineRenderer>();
            lineL.SetVertexCount(2);

            lineR = righthand.GetComponent<LineRenderer>();
            lineR.SetVertexCount(2);
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

            player.transform.Translate(newMove * Time.deltaTime * speed);

            Debug.DrawLine(righthand.transform.forward, righthand.transform.forward * 3, Color.blue);
            Debug.DrawLine(lefthand.transform.forward, lefthand.transform.forward * 3, Color.blue);

            lineL.SetPosition(0, lefthand.transform.position);
            lineL.SetPosition(1, lefthand.transform.forward * 5 + lefthand.transform.position);

            lineR.SetPosition(0, righthand.transform.position);
            lineR.SetPosition(1, righthand.transform.forward * 5 + righthand.transform.position);


            /*if (OVRInput.Get(OVRInput.Touch.PrimaryIndexTrigger))
            {
                Vector3 start = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
                Vector3 ray = SampleCurve(OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger)), 
                Physics.Raycast()
            } */

    }
}
