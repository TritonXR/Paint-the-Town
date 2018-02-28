using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInput : MonoBehaviour
{
    public Transform controllerTransform;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        //if (OVRInput.Touch.(OVRInput.Button.PrimaryHandTrigger))
        if(!OVRInput.Get(OVRInput.Touch.PrimaryIndexTrigger) && !OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
        {
            Debug.Log("touched");

            RaycastHit hit;

            if(Physics.Raycast(controllerTransform.position, controllerTransform.forward, out hit))
            {
                if(hit.collider.gameObject.CompareTag("Cube"))
                {
                    Destroy(hit.collider.gameObject);
                }
            }
        }
	}

   
}
