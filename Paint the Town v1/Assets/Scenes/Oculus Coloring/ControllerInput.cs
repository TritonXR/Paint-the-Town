using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: upon creating rooms/characters, have each InputManager object have this script attached, but each object has a different tag
//troubleshooting tips: make sure whatever is being colored has the Colorable script attached to it; make sure InputManager is tagged as PlayerRed, PlayerBlue, or PlayerGreen
//make sure InputManager has ControllerInput.cs attached to it; make sure colored object's material is using the shader Custom/TestingShader

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
            Material mat;

            if(Physics.Raycast(controllerTransform.position, controllerTransform.forward, out hit))
            {
                // red player
                if (this.tag == "PlayerRed")
                {

                    // checking the raycast hit a paintable target
                    if (hit.collider.GetComponent<Colorable>() != null)
                    {
                        mat = hit.collider.GetComponent<Renderer>().material;

                        // create a new texture to paint on
                        Texture2D redTex = GameObject.Instantiate(mat.GetTexture("_Red")) as Texture2D;

                        // paint the whole thing red
                        for (int x = 0; x < redTex.width; x++)
                        {
                            for (int y = 0; y < redTex.height; y++)
                            {
                                redTex.SetPixel(x, y, Color.white);
                            }
                        }
                        redTex.Apply();
                        mat.SetTexture("_Red", redTex);
                    }
                }
                //green player
                else if (this.tag == "PlayerGreen")
                {
                    if (hit.collider.GetComponent<Colorable>() != null)
                    {
                        mat = hit.collider.GetComponent<Renderer>().material;

                        // create a new texture to paint on
                        Texture2D tex = GameObject.Instantiate(mat.GetTexture("_Green")) as Texture2D;

                        // paint the whole thing red
                        for (int x = 0; x < tex.width; x++)
                        {
                            for (int y = 0; y < tex.height; y++)
                            {
                                tex.SetPixel(x, y, Color.white);
                            }
                        }
                        tex.Apply();
                        mat.SetTexture("_Green", tex);
                    }
                }
                //Blue play
                else if (this.tag == "PlayerBlue")
                {
                    if (hit.collider.GetComponent<Colorable>() != null)
                    {
                        mat = hit.collider.GetComponent<Renderer>().material;

                        // create a new texture to paint on
                        Texture2D tex = GameObject.Instantiate(mat.GetTexture("_Blue")) as Texture2D;

                        // paint the whole thing red
                        for (int x = 0; x < tex.width; x++)
                        {
                            for (int y = 0; y < tex.height; y++)
                            {
                                tex.SetPixel(x, y, Color.white);
                            }
                        }
                        tex.Apply();
                        mat.SetTexture("_Blue", tex);
                    }
                }
            }
        }
	}

   
}
