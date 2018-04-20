using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class Shader_change : MonoBehaviour {

    public Transform controllerTransform;
    public GameObject lefthand;
    public float weaponRange = 50f;                                     // Distance in Unity units over which the player can fire
	public Transform gunEnd;                                            // Holds a reference to the gun end object, marking the muzzle location of the gun
	//increase the color of 
	private Camera fpsCam;                                              // Holds a reference to the first person camera
	private Vector3 rayOrigin;
	private int penSize = 5;
	private Color[] color;

	private float posX, posY;
	private float lastX, lastY;
	private int textureSize = 2048;
	private bool hitLast, hitCurr;
	private int lerpX, lerpY;
	private bool redPaint, greenPaint, bluePaint;

	void Start () 
	{
		// Get and store a reference to our Camera by searching this GameObject and its parents
		fpsCam = GetComponentInParent<Camera>();
	}

	void Update () 
	{
        // Check if the player has pressed the fire button and if enough time has elapsed since they last fired
        if ((!OVRInput.Get(OVRInput.Touch.PrimaryIndexTrigger) && !OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && OVRInput.Get(OVRInput.Button.PrimaryHandTrigger)) ||
            (!OVRInput.Get(OVRInput.Touch.SecondaryIndexTrigger) && !OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger) && OVRInput.Get(OVRInput.Button.SecondaryHandTrigger)))
        {
            color_change ();
		} 
		else {
			hitCurr = false;
			hitLast = false;
		}
		hitLast = hitCurr;


		//animation

		//if (redPaint && greenPaint && bluePaint) {

		//	Animator.
		//}
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
					redPaint = true;
					// create a new texture to paint on
					Texture2D redTex = (Texture2D)GameObject.Instantiate (mat.GetTexture ("_Red"));
					Vector2 pixelUV = hit.textureCoord;
					pixelUV.x *= redTex.width;
					pixelUV.y *= redTex.height;

					int x = (int)(posX * textureSize - (penSize / 2));
					int y = (int)(posY * textureSize - (penSize / 2));

					//new pensize with color red
					color = Enumerable.Repeat<Color> (Color.red, penSize * penSize).ToArray<Color> ();
						
					if (hitLast) {
						//connecting current ray position to last ray position
						for (float t = 0.01f; t < 1.00f; t += 0.01f) {

							redTex.SetPixels ((int)pixelUV.x, (int)pixelUV.y, penSize, penSize, color);

						
							lerpX = (int)Mathf.Lerp (lastX, (float)pixelUV.x, t);
							lerpY = (int)Mathf.Lerp (lastY, (float)pixelUV.y, t);
							redTex.SetPixels (lerpX, lerpY, penSize, penSize, color);
						}

					}

					if (hitCurr) {
						redTex.Apply ();
					}
				
					this.lastX = (float)pixelUV.x;
					this.lastY = (float)pixelUV.y;


					mat.SetTexture ("_Red", redTex);
					if (hitLast == false) {
						hitLast = true;
					} 
				}
			}
			//green play
			else if (this.tag == "PlayerGreen") {
				if (hit.collider.GetComponent<Colorable> () != null) {
					mat = hit.collider.GetComponent<Renderer> ().material;
					greenPaint = true;
					// create a new texture to paint on
					Texture2D greenTex = (Texture2D)GameObject.Instantiate (mat.GetTexture ("_Green"));
					Vector2 pixelUV = hit.textureCoord;
					pixelUV.x *= greenTex.width;
					pixelUV.y *= greenTex.height;

					int x = (int)(posX * textureSize - (penSize / 2));
					int y = (int)(posY * textureSize - (penSize / 2));

					//new pensize with color red
					color = Enumerable.Repeat<Color> (Color.green, penSize * penSize).ToArray<Color> ();

					if (hitLast) {
						//connecting current ray position to last ray position
						for (float t = 0.01f; t < 1.00f; t += 0.01f) {

							greenTex.SetPixels ((int)pixelUV.x, (int)pixelUV.y, penSize, penSize, color);


							lerpX = (int)Mathf.Lerp (lastX, (float)pixelUV.x, t);
							lerpY = (int)Mathf.Lerp (lastY, (float)pixelUV.y, t);
							greenTex.SetPixels (lerpX, lerpY, penSize, penSize, color);
						}

					}

					if (hitCurr) {
						greenTex.Apply ();
					}

					this.lastX = (float)pixelUV.x;
					this.lastY = (float)pixelUV.y;


					mat.SetTexture ("_Green", greenTex);
					if (hitLast == false) {
						hitLast = true;
					} 
				}
			}
			//Blue play
			else if (this.tag == "PlayerBlue") {
				if (hit.collider.GetComponent<Colorable> () != null) {
					mat = hit.collider.GetComponent<Renderer> ().material;
					bluePaint = true;
					// create a new texture to paint on
					Texture2D blueTex = (Texture2D)GameObject.Instantiate (mat.GetTexture ("_Blue"));
					Vector2 pixelUV = hit.textureCoord;
					pixelUV.x *= blueTex.width;
					pixelUV.y *= blueTex.height;

					int x = (int)(posX * textureSize - (penSize / 2));
					int y = (int)(posY * textureSize - (penSize / 2));

					//new pensize with color red
					color = Enumerable.Repeat<Color> (Color.blue, penSize * penSize).ToArray<Color> ();

					if (hitLast) {
						//connecting current ray position to last ray position
						for (float t = 0.01f; t < 1.00f; t += 0.01f) {

							blueTex.SetPixels ((int)pixelUV.x, (int)pixelUV.y, penSize, penSize, color);


							lerpX = (int)Mathf.Lerp (lastX, (float)pixelUV.x, t);
							lerpY = (int)Mathf.Lerp (lastY, (float)pixelUV.y, t);
							blueTex.SetPixels (lerpX, lerpY, penSize, penSize, color);
						}

					}

					if (hitCurr) {
						blueTex.Apply ();
					}

					this.lastX = (float)pixelUV.x;
					this.lastY = (float)pixelUV.y;


					mat.SetTexture ("_Blue", blueTex);
					if (hitLast == false) {
						hitLast = true;
					} 
				}
			}

		}
	}
}

