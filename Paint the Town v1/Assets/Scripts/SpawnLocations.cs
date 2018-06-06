using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLocations : MonoBehaviour {

    public GameObject[] spawns;
    public Vector3[] spawnPos;

	// Use this for initialization
	void Start () {
		for(int i=0; i < spawns.Length; i++)
        {
            spawnPos[i] = spawns[i].transform.position;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
