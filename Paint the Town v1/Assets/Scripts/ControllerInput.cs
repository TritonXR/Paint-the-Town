using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//TODO: upon creating rooms/characters, have each InputManager object have this script attached, but each object has a different tag
//troubleshooting tips: make sure whatever is being colored has the Colorable script attached to it; make sure InputManager is tagged as PlayerRed, PlayerBlue, or PlayerGreen
//make sure InputManager has ControllerInput.cs attached to it; make sure colored object's material is using the shader Custom/TestingShader
//to get hand models, just add the LocalAvatar Prefab from Assets/OvrAvatar/Content/Prefabs

public class ControllerInput : MonoBehaviour
{
    public Transform controllerTransform;
    public GameObject lefthand;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        //TODO add a line to visualize the raycast

        //if (OVRInput.Touch.(OVRInput.Button.PrimaryHandTrigger))
        if((!OVRInput.Get(OVRInput.Touch.PrimaryIndexTrigger) && !OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && OVRInput.Get(OVRInput.Button.PrimaryHandTrigger)) ||
            (!OVRInput.Get(OVRInput.Touch.SecondaryIndexTrigger) && !OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger) && OVRInput.Get(OVRInput.Button.SecondaryHandTrigger)))
        {
            Debug.Log("touched");

            RaycastHit hit;
            Material mat;

            if(Physics.Raycast(controllerTransform.position, controllerTransform.forward, out hit) || 
                Physics.Raycast(lefthand.transform.position, lefthand.transform.forward, out hit))
            {
                // red player
                if (this.tag == "PlayerRed")
                {
                    if (hit.collider.GetComponent<UIClick>() != null)
                    {
                        Debug.Log("clicked");
                        if(hit.collider.tag == "Button")
                        {
                            //do whatever
                        }

                        //the line below changes the current scene to the apartment scene
                        //SceneManager.LoadScene("Prototype Scene - PreMaster");
                    }

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
