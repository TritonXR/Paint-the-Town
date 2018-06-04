using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Raycast : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public float interactionDistance = 3f;
    public bool imnear;

    void Update()
    {
        RaycastHit hit;

        Debug.DrawRay(transform.position, transform.forward * interactionDistance);

        if (Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance))
        {
            imnear = (hit.collider.tag == "Clickable");
            Debug.Log("collide");
        }
    }
}
