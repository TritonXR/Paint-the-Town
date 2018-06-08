using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gramophone_Play : MonoBehaviour {

    public AudioClip[] playlist;
    public GameObject disk;
    public GameObject grambase;
    public GameObject head;
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

            if (disk.GetComponent<Colorable>().curState == state.D && grambase.GetComponent<Colorable>().curState == state.D && head.GetComponent<Colorable>().curState == state.D)
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
