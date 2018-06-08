using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsToggle : MonoBehaviour {

    public GameObject logo;
    public GameObject credits;
    private bool creditshow;
    private float lastclick;

	// Use this for initialization
	void Start () {
        creditshow = false;
        lastclick = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {
     
	}

    public void toggle()
    {
        float currenttime = Time.realtimeSinceStartup;
        //Debug.Log("curr" + currenttime);
        //Debug.Log("last"+ lastclick);
        if (currenttime - lastclick > 0.5f)
        {

            //Debug.Log(creditshow);
                if (!creditshow)
                {
                    //Debug.Log("hiding credits, show logo");
                    credits.SetActive(!creditshow);
                    logo.SetActive(creditshow);
                    creditshow = !creditshow;
                }
                else
                {
                    //Debug.Log("hiding logo, showing credits");
                    credits.SetActive(!creditshow);
                    logo.SetActive(creditshow);
                    creditshow = !creditshow;
                }
        }
        lastclick = Time.realtimeSinceStartup;
    }

    public void hi()
    {
        Debug.Log("hi");
    }
}
