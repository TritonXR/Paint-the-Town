using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueScript : MonoBehaviour {

    private Colorable statueColorable;
	// Use this for initialization
	void Start () {
        statueColorable = GetComponent<Colorable>();
	}
	
	// Update is called once per frame
	void Update () {
        if (statueColorable.curState == state.D)
        {
            if(PhotonNetwork.isMasterClient)
            {
                PhotonNetwork.LoadLevel("Prototype Scene - PreMaster");
            }
        }
	}
}
