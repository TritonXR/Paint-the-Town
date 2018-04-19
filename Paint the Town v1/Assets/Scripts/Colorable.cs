using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colorable : MonoBehaviour {

    Material mat;
    Renderer ren;
    float scale;
    AudioSource sound;

	// Use this for initialization
	void Start () {
        mat = GetComponent<Material>();
        ren = GetComponent<Renderer>();
        scale = 0.0f;
        sound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        //manipulate scale here
        //but you still have to get the _Transition variable in order to change volume and everything, right?
        //ren.material.SetFloat("_Transition", scale);
        //sound.volume
    }
}
