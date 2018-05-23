using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colorable : MonoBehaviour {

	enum state {N, R, G, B, RG, RB, GB, RGB};

	private Animator animator;
	private Texture texture;
	private ParticleSystem particle;

	state curState;

	// Use this for initialization
	void Start () {
		curState = state.N;
		animator = GetComponent<Animator> ();
		if (animator != null) {
			animator.SetBool("Play", false);
			Debug.Log("disabled");
		}
	}
	
	// Update is called once per frame
	void Update () {
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

	void TurnOn(){
		
	}
}
