using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gramophone_Play : MonoBehaviour {

    public AudioClip[] playlist;
    private AudioSource audio;
    private bool childCol = false;
    private Component[] childrenColorable;
    private int colorTally;

	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>();
        childrenColorable = this.transform.GetComponentsInChildren<Colorable>();
        audio.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (!childCol)
        {
            colorTally = 0;
            Debug.Log("we are false");
            foreach (Colorable child in childrenColorable)
            {
                if (child.curState == state.RGB || child.curState == state.D)
                {
                    colorTally++;
                }
                Debug.Log("tally count is " + colorTally);
            }
            if (colorTally == childrenColorable.Length)
            {
                childCol = true;
                Debug.Log("setting it to true");
            }
        } else
        {
            if (!audio.isPlaying)
            {
                playRandomMusic();
            }
                
        }

    }
    void playRandomMusic()
    {
        audio.clip = playlist[Random.Range(0,playlist.Length-1)] as AudioClip;
        audio.Play();
    }
}
