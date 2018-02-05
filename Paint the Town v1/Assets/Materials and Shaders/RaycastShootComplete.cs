using UnityEngine;
using System.Collections;

public class RaycastShootComplete : MonoBehaviour {

	public float weaponRange = 50f;                                     // Distance in Unity units over which the player can fire
	public Transform gunEnd;                                            // Holds a reference to the gun end object, marking the muzzle location of the gun
                                            //increase the color of 
	private Camera fpsCam;                                              // Holds a reference to the first person camera

	void Start () 
	{

		// Get and store a reference to our Camera by searching this GameObject and its parents
		fpsCam = GetComponentInParent<Camera>();
	}

	void Update () 
	{
		// Check if the player has pressed the fire button and if enough time has elapsed since they last fired
		if (Input.GetButton("Fire1"))// && Time.time > nextFire) 
		{

			// Create a vector at the center of our camera's viewport
			Vector3 rayOrigin = fpsCam.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));

			// Declare a raycast hit to store information about what our raycast has hit
			RaycastHit hit;

			// Check if our raycast has hit anything
			if (Physics.Raycast (rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
			{
				Material mat;
				//red play
				if (this.tag == "PlayerRed") {
					mat = hit.collider.GetComponent<Renderer>().material;
					if (mat != null) {
						
						//Texture2D redTex = (mat.GetTexture("_Red") as Texture2D);
						Texture2D redTex = (Texture2D)GameObject.Instantiate(mat.GetTexture("_Red"));
						//Debug.Log ("name:"+redTex.name);
						Vector2 pixelUV = hit.textureCoord;
						pixelUV.x *= redTex.width;
						pixelUV.y *= redTex.height;
						int x =(int) pixelUV.x;
						int y = 0;
						//radius of the brush size
						int radius = 2;
						x = radius - 1;
						int dx = 1;
						int dy = 1;
						int err = dx - (radius << 1);
						while (x >= y) {
						redTex.SetPixel ((int)pixelUV.x+x, (int)pixelUV.y+y, Color.white);
							redTex.SetPixel ((int)pixelUV.x+y, (int)pixelUV.y+x, Color.white);
							redTex.SetPixel ((int)pixelUV.x-y, (int)pixelUV.y+x, Color.white);
							redTex.SetPixel ((int)pixelUV.x-x, (int)pixelUV.y+y, Color.white);
							redTex.SetPixel ((int)pixelUV.x-x, (int)pixelUV.y-y, Color.white);
							redTex.SetPixel ((int)pixelUV.x-y, (int)pixelUV.y-x, Color.white);
							redTex.SetPixel ((int)pixelUV.x+y, (int)pixelUV.y-x, Color.white);
							redTex.SetPixel ((int)pixelUV.x+x, (int)pixelUV.y-y, Color.white);
							redTex.SetPixel ((int)pixelUV.x, (int)pixelUV.y, Color.white);
							if (err <= 0) {
								y++;
								err += dy;
								dy += 2;
							}
							if (err > 0) {
								x--;
								dx += 2;
								err += dx - (radius << 1);
							}
						}

						//redTex.Apply ();
						MeshRenderer render = GetComponent<MeshRenderer> ();
						render.sharedMaterials [0].mainTexture = redTex;
						mat.SetTexture("_Red", redTex);
						//mat.color = Color.green;
					}
				}
				//green play
				else if (this.tag == "PlayerGreen") {
					mat = hit.collider.GetComponent<Renderer>().material;
					if (mat != null) {
						mat.SetFloat ("_Green", 1.0f);
						//	mat.color = Color.green;
					}
				}
				//Blue play
				else if (this.tag == "PlayerBlue") {
					mat = hit.collider.GetComponent<Renderer>().material;
					if (mat != null) {
						mat.SetFloat ("_Blue", 1.0f);
						//	mat.color = Color.green;
					}
				}
					

			}
			else
			{}
		}
	}
}