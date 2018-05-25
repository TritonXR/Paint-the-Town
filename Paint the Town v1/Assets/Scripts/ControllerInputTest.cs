using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

//TODO: upon creating rooms/characters, have each InputManager object have this script attached, but each object has a different tag
//troubleshooting tips: make sure whatever is being colored has the Colorable script attached to it; make sure InputManager is tagged as PlayerRed, PlayerBlue, or PlayerGreen
//make sure InputManager has ControllerInput.cs attached to it; make sure colored object's material is using the shader Custom/TestingShader
//to get hand models, just add the LocalAvatar Prefab from Assets/OvrAvatar/Content/Prefabs


public class ControllerInputTest : MonoBehaviour  
{
	public Transform controllerTransform;
	public GameObject lefthand;
	public GameObject settingsMenu;
	public GameObject mainMenu;
	public GameObject joinButton;
	public GameObject pauseScreen;
	public GameObject eyeCenter;

	RayCastObject prev;
	RayCastObject curr;


	// shader stuffs
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
	private bool paused;

	// Use this for initialization
	void Start()
	{
		paused = false;
	}

	// Update is called once per frame
	void Update()
	{
		//TODO add a line to visualize the raycast

		//if (OVRInput.Touch.(OVRInput.Button.PrimaryHandTrigger))
		/*if ((!OVRInput.Get(OVRInput.Touch.PrimaryIndexTrigger) && !OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && OVRInput.Get(OVRInput.Button.PrimaryHandTrigger)) ||
            (!OVRInput.Get(OVRInput.Touch.SecondaryIndexTrigger) && !OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger) && OVRInput.Get(OVRInput.Button.SecondaryHandTrigger)))
        { */

		if (Input.GetMouseButton(0))
		{
			//Debug.Log("press");

			//Change raycast so that ut us outside of the controller check because ti should be active every time
			RaycastHit hit;
			Material mat;
			Camera cam = GetComponentInChildren<Camera> ();

			if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
			{
					
			}
				// red player
				if (this.tag == "PlayerRed" && !paused) //disable painting
				{
					Colorable datThing = hit.collider.GetComponent<Colorable> ();
					// checking the raycast hit a paintable target
					if (datThing != null)
					{
						
						mat = hit.collider.GetComponent<Renderer>().material;
						redPaint = true;
						// create a new texture to paint on
						Texture2D redTex = (Texture2D)GameObject.Instantiate(mat.GetTexture("_Red"));
						Vector2 pixelUV = hit.textureCoord;
						datThing.ChangeState ("Red");
						
						pixelUV.x *= redTex.width;
						pixelUV.y *= redTex.height;

						int x = (int)(posX * textureSize - (penSize / 2));
						int y = (int)(posY * textureSize - (penSize / 2));

						//new pensize with color red
						color = Enumerable.Repeat<Color>(Color.red, penSize * penSize).ToArray<Color>();

						if (hitLast)
						{
							//connecting current ray position to last ray position
							for (float t = 0.01f; t < 1.00f; t += 0.01f)
							{

								redTex.SetPixels((int)pixelUV.x, (int)pixelUV.y, penSize, penSize, color);


								lerpX = (int)Mathf.Lerp(lastX, (float)pixelUV.x, t);
								lerpY = (int)Mathf.Lerp(lastY, (float)pixelUV.y, t);
								redTex.SetPixels(lerpX, lerpY, penSize, penSize, color);
							}

						}

						if (hitCurr)
						{
							redTex.Apply();
						}

						this.lastX = (float)pixelUV.x;
						this.lastY = (float)pixelUV.y;


						mat.SetTexture("_Red", redTex);
						if (hitLast == false)
						{
							hitLast = true;
						}
					}
				}
				//green player
				else if (this.tag == "PlayerGreen" && !paused) //disable painting
				{
				Colorable datThing = hit.collider.GetComponent<Colorable> ();
				// checking the raycast hit a paintable target
				if (datThing != null)
				{
					datThing.ChangeState ("Green");
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
				else if (this.tag == "PlayerBlue" && !paused) //disable painting
				{
				Colorable datThing = hit.collider.GetComponent<Colorable> ();
				// checking the raycast hit a paintable target
				if (datThing != null)
				{
					datThing.ChangeState ("Blue");
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

		// && (SceneManager.GetSceneByName("Prototype Scene - PreMaster") == SceneManager.GetActiveScene())

		//have pause menu toggle here here
		if (OVRInput.GetDown(OVRInput.Button.Start))
		{
			Debug.Log("pausing");
			if(paused)
			{
				//set pause screen to not active
			}
			else
			{
				Debug.Log("Hi we here");
				//move PauseScreen to in front of player
				//pauseScreen.transform = eyeCenter.transform;
				pauseScreen.transform.parent = eyeCenter.transform;

				// Debug.Log("Parent is" + pauseScreen.transform.parent.name);

				pauseScreen.transform.localPosition = new Vector3(0, 0, 2);

				// Debug.Log("Position is" + pauseScreen.transform.position);

				//  Debug.Break();

				//   Debug.Break();

				// Debug.Log("After Break");

				pauseScreen.transform.parent = null;

				pauseScreen.SetActive(true);

				//Debug.Break();
				//pauseScreen.GetComponent<RectTransform>().anchoredPosition = new Vector2(eyeCenter.transform.position.x, eyeCenter.transform.position.y);
				// pauseScreen.GetComponent<RectTransform>().

				//set pause screen to active
			}
			paused = !paused;
			//first, make something appear in front of player's head
		}
	}
}



//i'm sorry
/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;


//TODO: upon creating rooms/characters, have each InputManager object have this script attached, but each object has a different tag
//troubleshooting tips: make sure whatever is being colored has the Colorable script attached to it; make sure InputManager is tagged as PlayerRed, PlayerBlue, or PlayerGreen
//make sure InputManager has ControllerInput.cs attached to it; make sure colored object's material is using the shader Custom/TestingShader
//to get hand models, just add the LocalAvatar Prefab from Assets/OvrAvatar/Content/Prefabs

public class ControllerInput : MonoBehaviour
{
    public Transform controllerTransform;
    public GameObject lefthand;
    public float weaponRange = 50f;                                     // Distance in Unity units over which the player can fire
    public GameObject gunEnd;                                            // Holds a reference to the gun end object, marking the muzzle location of the gun
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

    // Use this for initialization
    void Start ()
    {
        // Get and store a reference to our Camera by searching this GameObject and its parents
        fpsCam = GetComponentInParent<Camera>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //TODO add a line to visualize the raycast

        //if (OVRInput.Touch.(OVRInput.Button.PrimaryHandTrigger))
        if((!OVRInput.Get(OVRInput.Touch.PrimaryIndexTrigger) && !OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && OVRInput.Get(OVRInput.Button.PrimaryHandTrigger)) ||
            (!OVRInput.Get(OVRInput.Touch.SecondaryIndexTrigger) && !OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger) && OVRInput.Get(OVRInput.Button.SecondaryHandTrigger)))
        {
            color_change();
            uiInteract();
        }
        else
        {
            hitCurr = false;
            hitLast = false;
        }
        hitLast = hitCurr;


        //animation

        //if (redPaint && greenPaint && bluePaint) {

        //	Animator.
        //}
    }

    void uiInteract()
    {
        // Create a vector at the center of our camera's viewport
        rayOrigin = gunEnd.transform.position;

        // Declare a raycast hit to store information about what our raycast has hit
        RaycastHit hit;

        // Check if our raycast has hit anything
        if (Physics.Raycast(rayOrigin, gunEnd.transform.forward, out hit, weaponRange))
        {
            Material mat;

            ///NEW STUFF

            // check if last hit was the on the same texture 
            hitCurr = true;

            Debug.Log("clicked");
            //if(hit.collider.name == "Button")
            //{
            //do whatever

            Debug.Log(hit.collider.name);
            Button uiButton = hit.collider.GetComponent<Button>();
            Color r = Color.red;
            ColorBlock cb = uiButton.colors;
            cb.highlightedColor = r;
            uiButton.colors = cb;

            //the line below changes the current scene to the apartment scene
            if (hit.collider.name == "CreateGameButton")
                SceneManager.LoadScene("Prototype Scene - PreMaster");
            //and so on with if statements
            if (hit.collider.name == "SettingsButton")
            {
                //show settings
                //uiButton.onClick();
                hit.collider.GetComponentInParent<MainMenu>().gameObject.SetActive(false);
            }

            //Quit the game when user hits quit from the menu
            if (hit.collider.name == "Quit")
                Application.Quit();

            ///END NEW STUFF
        }
    }


    void color_change()
    {

        // Create a vector at the center of our camera's viewport
        rayOrigin = gunEnd.transform.position;
        
        // Declare a raycast hit to store information about what our raycast has hit
        RaycastHit hit;

        // Check if our raycast has hit anything
        if (Physics.Raycast(rayOrigin, gunEnd.transform.forward, out hit, weaponRange))
        {
            Material mat;

        // red player
        if (this.tag == "PlayerRed")
        {
                // checking the raycast hit a paintable target
                if (hit.collider.GetComponent<Colorable>() != null)
                {
                    mat = hit.collider.GetComponent<Renderer>().material;
                    redPaint = true;
                    // create a new texture to paint on
                    Texture2D redTex = (Texture2D)GameObject.Instantiate(mat.GetTexture("_Red"));
                    Vector2 pixelUV = hit.textureCoord;
                    pixelUV.x *= redTex.width;
                    pixelUV.y *= redTex.height;

                    int x = (int)(posX * textureSize - (penSize / 2));
                    int y = (int)(posY * textureSize - (penSize / 2));

                    //new pensize with color red
                    color = Enumerable.Repeat<Color>(Color.red, penSize * penSize).ToArray<Color>();

                    if (hitLast)
                    {
                        //connecting current ray position to last ray position
                        for (float t = 0.01f; t < 1.00f; t += 0.01f)
                        {

                            redTex.SetPixels((int)pixelUV.x, (int)pixelUV.y, penSize, penSize, color);


                            lerpX = (int)Mathf.Lerp(lastX, (float)pixelUV.x, t);
                            lerpY = (int)Mathf.Lerp(lastY, (float)pixelUV.y, t);
                            redTex.SetPixels(lerpX, lerpY, penSize, penSize, color);
                        }

                    }

                    if (hitCurr)
                    {
                        redTex.Apply();
                    }

                    this.lastX = (float)pixelUV.x;
                    this.lastY = (float)pixelUV.y;


                    mat.SetTexture("_Red", redTex);
                    if (hitLast == false)
                    {
                        hitLast = true;
                    }
                }
        }
        //green play
        else if (this.tag == "PlayerGreen")
         {
                if (hit.collider.GetComponent<Colorable>() != null)
                {
                    mat = hit.collider.GetComponent<Renderer>().material;
                    greenPaint = true;
                    // create a new texture to paint on
                    Texture2D greenTex = (Texture2D)GameObject.Instantiate(mat.GetTexture("_Green"));
                    Vector2 pixelUV = hit.textureCoord;
                    pixelUV.x *= greenTex.width;
                    pixelUV.y *= greenTex.height;

                    int x = (int)(posX * textureSize - (penSize / 2));
                    int y = (int)(posY * textureSize - (penSize / 2));

                    //new pensize with color red
                    color = Enumerable.Repeat<Color>(Color.green, penSize * penSize).ToArray<Color>();

                    if (hitLast)
                    {
                        //connecting current ray position to last ray position
                        for (float t = 0.01f; t < 1.00f; t += 0.01f)
                        {

                            greenTex.SetPixels((int)pixelUV.x, (int)pixelUV.y, penSize, penSize, color);


                            lerpX = (int)Mathf.Lerp(lastX, (float)pixelUV.x, t);
                            lerpY = (int)Mathf.Lerp(lastY, (float)pixelUV.y, t);
                            greenTex.SetPixels(lerpX, lerpY, penSize, penSize, color);
                        }

                    }

                    if (hitCurr)
                    {
                        greenTex.Apply();
                    }

                    this.lastX = (float)pixelUV.x;
                    this.lastY = (float)pixelUV.y;


                    mat.SetTexture("_Green", greenTex);
                    if (hitLast == false)
                    {
                        hitLast = true;
                    }
                }
            }
            //Blue play
            else if (this.tag == "PlayerBlue")
            {
                if (hit.collider.GetComponent<Colorable>() != null)
                {
                    mat = hit.collider.GetComponent<Renderer>().material;
                    bluePaint = true;
                    // create a new texture to paint on
                    Texture2D blueTex = (Texture2D)GameObject.Instantiate(mat.GetTexture("_Blue"));
                    Vector2 pixelUV = hit.textureCoord;
                    pixelUV.x *= blueTex.width;
                    pixelUV.y *= blueTex.height;

                    int x = (int)(posX * textureSize - (penSize / 2));
                    int y = (int)(posY * textureSize - (penSize / 2));

                    //new pensize with color red
                    color = Enumerable.Repeat<Color>(Color.blue, penSize * penSize).ToArray<Color>();

                    if (hitLast)
                    {
                        //connecting current ray position to last ray position
                        for (float t = 0.01f; t < 1.00f; t += 0.01f)
                        {

                            blueTex.SetPixels((int)pixelUV.x, (int)pixelUV.y, penSize, penSize, color);


                            lerpX = (int)Mathf.Lerp(lastX, (float)pixelUV.x, t);
                            lerpY = (int)Mathf.Lerp(lastY, (float)pixelUV.y, t);
                            blueTex.SetPixels(lerpX, lerpY, penSize, penSize, color);
                        }

                    }

                    if (hitCurr)
                    {
                        blueTex.Apply();
                    }

                    this.lastX = (float)pixelUV.x;
                    this.lastY = (float)pixelUV.y;


                    mat.SetTexture("_Blue", blueTex);
                    if (hitLast == false)
                    {
                        hitLast = true;
                    }
                }
            }

        }
	}

   
}
*/
