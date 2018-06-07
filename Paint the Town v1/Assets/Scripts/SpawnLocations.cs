using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLocations : MonoBehaviour {

    public GameObject[] spawns;
    public Vector3[] spawnPos;
    public Vector3[] spawnRots;

	// Use this for initialization
	void Start () {
		for(int i=0; i < spawns.Length-1; i++)
        {
            spawnPos[i] = spawns[i].transform.position;
            spawnRots[i] = new Vector3(spawns[i].transform.localRotation.x, spawns[i].transform.localRotation.y, spawns[i].transform.localRotation.z);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
