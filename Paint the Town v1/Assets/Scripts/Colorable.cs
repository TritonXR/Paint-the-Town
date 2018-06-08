using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum state {N, R, G, B, RG, RB, GB, RGB, D};

public class Colorable : MonoBehaviour {


	private Animator animator;
	private Texture texture;
	private List<GameObject> particleObj;
	private ParticleSystem particle;
    private Color[] color;
    private int penSize = 5;
    private int lerpX, lerpY;
    private AudioSource audio;
    private float volumeFloat = 1.0f;
    private bool isAudio;

    public state curState;
	Material mat;
	float t = 0.01f;

    void Awake()
    {
 
    }

    // Use this for initialization
    void Start () {


        curState = state.N;
		animator = GetComponent<Animator> ();
		if (animator != null) {
			//set each animation accordingly
			if (gameObject.name == "gramophone v2") {
				animator.SetBool ("Play", false);
			}
			/*
			else if(gameObject.name == "") {

			}
			*/
			//Debug.Log("disabled");
		}
		
		mat = GetComponent<Renderer> ().material;

        audio = GetComponent<AudioSource>();
        if (audio != null)
        {
            volumeFloat = audio.volume;
            audio.volume = 0.0f;
        }
        
    }

	
	// Update is called once per frame
	void Update () {


		if (curState != state.D) {
			//if all 3 color are painted
			if (curState == state.RGB) {
				GetComponent<Renderer> ().material.SetFloat ("_Transition", Mathf.Lerp (0.01f, 1.0f, t));
                if (isAudio)
                {
                    audio.Play();
                }
				t += 0.5f * Time.deltaTime;

                /*if (this.transform.parent.tag != "Room") {
					Component[] childrenColorable = this.transform.parent.GetComponentsInChildren<Colorable> ();
					foreach (Colorable child in childrenColorable) {
						child.curState = state.RGB;
					}
				}*/
                /*				else if(){

                                }
                                */
                if (audio != null)
                {
                    audio.volume = volumeFloat;
                }

                for (int i = 0; i < transform.GetChildCount(); i++)
                {
                    if (transform.GetChild(i).GetComponent<ParticleSystem>() != null)
                    {
                        transform.GetChild(i).gameObject.SetActive(true);
                    }
                }
            }

            if (mat != null) {
                if (mat.HasProperty("_Transition"))
                {
                    if (mat.GetFloat("_Transition") >= 1.0f)
                        curState = state.D;
                }
			}

        }
/*		if(animator != null)
			Debug.Log(animator.GetBool ("Play"));
		if (Input.GetMouseButton (0)) {
			curState = state.RGB;
			Debug.Log ("something");
		}
		if (curState == state.RGB) {
			if (animator != null) {
				animator.SetBool ("Play", true);
			}
			Debug.Log ("works");
		}*/
    }

	public void ChangeState(string states){
		if (curState == state.D) {
			return;
		}
		else if (states == "Red") {
			if (curState == state.N)
				curState = state.R;
			else if (curState == state.G)
				curState = state.RG;
			else if (curState == state.B)
				curState = state.RB;
			else if (curState == state.GB)
				curState = state.RGB;
		} else if (states == "Blue") {
			if (curState == state.N)
				curState = state.B;
			else if (curState == state.G)
				curState = state.GB;
			else if (curState == state.R)
				curState = state.RB;
			else if (curState == state.RG)
				curState = state.RGB;
		} else if (states == "Green") {
			if (curState == state.N)
				curState = state.G;
			else if (curState == state.R)
				curState = state.RG;
			else if (curState == state.B)
				curState = state.GB;
			else if (curState == state.RB)
				curState = state.RGB;
		}
	}


    //Copy what ever happens during painting
    [PunRPC]
    void paintWithTex(int photonViewID, string tag, float pixelX, float pixelY, float lastX, float lastY, bool hitLast, bool hitCurr)
    {
        GameObject thing = PhotonView.Find(photonViewID).gameObject;

        if(thing == null)
        {
            Debug.Log("Photon View could not find the game object");
        }

        Material matCopy = thing.GetComponent<Renderer>().material;

        if(matCopy == null)
        {
            Debug.Log("material not found from thing we are coloring");
        }
        // create a new texture to paint on

        Texture2D tex = (Texture2D)GameObject.Instantiate(matCopy.GetTexture(tag));
        Vector2 pixelUV = new Vector2(pixelX, pixelY);
        /*pixelUV.x *= tex.width;
        pixelUV.y *= tex.height;*/

        /*int x = (int)(posX * textureSize - (penSize / 2));
        int y = (int)(posY * textureSize - (penSize / 2));*/

        //new pensize with color red
        if (tag == "_Red")
        {
            color = Enumerable.Repeat<Color>(Color.red, penSize * penSize).ToArray<Color>();
            ChangeState("Red");
        }
        else if (tag == "_Green")
        {
            color = Enumerable.Repeat<Color>(Color.green, penSize * penSize).ToArray<Color>();
            ChangeState("Green");
        }
        else
        {
            color = Enumerable.Repeat<Color>(Color.blue, penSize * penSize).ToArray<Color>();
            ChangeState("Blue");
        }

        if (hitLast)
        {
            //connecting current ray position to last ray position
            for (float t = 0.01f; t < 1.00f; t += 0.01f)
            {

                tex.SetPixels((int)pixelUV.x, (int)pixelUV.y, penSize, penSize, color);


                lerpX = (int)Mathf.Lerp(lastX, (float)pixelUV.x, t);
                lerpY = (int)Mathf.Lerp(lastY, (float)pixelUV.y, t);
                tex.SetPixels(lerpX, lerpY, penSize, penSize, color);
            }

        }

        if (hitCurr)
        {
            tex.Apply();
        }

        matCopy.SetTexture(tag, tex);

    }

}
