using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class Texture_change : MonoBehaviour {

	public float weaponRange = 50f;                                     // Distance in Unity units over which the player can fire
	public Transform gunEnd;                                            // Holds a reference to the gun end object, marking the muzzle location of the gun
	//increase the color of 
	private Camera fpsCam;                                              // Holds a reference to the first person camera
	private Vector3 rayOrigin;
	private int penSize = 5;
	private Color[] colors;

	private float posX, posY;
	private float lastX, lastY;
	private int textureSize = 2048;
	private bool hitLast, hitCurr;
	private int lerpX, lerpY;

	void Start () 
	{
		// Get and store a reference to our Camera by searching this GameObject and its parents
		fpsCam = GetComponentInParent<Camera>();
	}

	void Update () 
	{	
		// Check if the player has pressed the fire button and if enough time has elapsed since they last fired
		if (Input.GetButton ("Fire1")) {
			color_change ();
		} 
		else {
			hitCurr = false;
			hitLast = false;
		}
		hitLast = hitCurr;
	}

	void color_change(){

		// Create a vector at the center of our camera's viewport
		rayOrigin = fpsCam.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, 0.0f));

		// Declare a raycast hit to store information about what our raycast has hit
		RaycastHit hit;

		// Check if our raycast has hit anything
		if (Physics.Raycast (rayOrigin, fpsCam.transform.forward, out hit, weaponRange)) {
			Material mat;

			// check if last hit was the on the same texture 
			hitCurr = true;

			// red player
			if (this.tag == "PlayerRed") {

				// checking the raycast hit a paintable target
				if (hit.collider.GetComponent<Colorable> () != null) {
					mat = hit.collider.GetComponent<Renderer> ().material;

					// create a new texture to paint on
					Texture2D redTex = GameObject.Instantiate (mat.GetTexture ("_Red")) as Texture2D;

					// paint the whole thing red
					for (int x = 0; x < redTex.width; x++) {
						for (int y = 0; y < redTex.height; y++) {
							redTex.SetPixel (x, y, Color.white);
						}
					}
					redTex.Apply ();
					mat.SetTexture ("_Red", redTex);
				}
			}
			//green player
			else if (this.tag == "PlayerGreen") {
				if (hit.collider.GetComponent<Colorable> () != null) {
					mat = hit.collider.GetComponent<Renderer> ().material;

					// create a new texture to paint on
					Texture2D tex = GameObject.Instantiate (mat.GetTexture ("_Green")) as Texture2D;

					// paint the whole thing red
					for (int x = 0; x < tex.width; x++) {
						for (int y = 0; y < tex.height; y++) {
							tex.SetPixel (x, y, Color.white);
						}
					}
					tex.Apply ();
					mat.SetTexture ("_Green", tex);
				}
			}
			//Blue play
			else if (this.tag == "PlayerBlue") {
				if (hit.collider.GetComponent<Colorable> () != null) {
					mat = hit.collider.GetComponent<Renderer> ().material;

					// create a new texture to paint on
					Texture2D tex = GameObject.Instantiate (mat.GetTexture ("_Blue")) as Texture2D;

					// paint the whole thing red
					for (int x = 0; x < tex.width; x++) {
						for (int y = 0; y < tex.height; y++) {
							tex.SetPixel (x, y, Color.white);
						}
					}
					tex.Apply ();
					mat.SetTexture ("_Blue", tex);
				}
			}

		}
	}
}

