using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueScript : MonoBehaviour {

    public GameObject statueBase;
    private Colorable statueColorable;
    private Colorable statueBaseColorable;
	// Use this for initialization
	void Start () {
        statueColorable = GetComponent<Colorable>();
        statueBaseColorable = statueBase.GetComponent<Colorable>();
	}
	
	// Update is called once per frame
	void Update () {
        if (statueColorable.curState == state.D && statueBaseColorable.curState == state.D)
        {
            if (PhotonNetwork.isMasterClient)
            {
                PhotonNetwork.LoadLevel("Prototype Scene - PreMaster");
            }

        }
	}
}
