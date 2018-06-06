using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordDisk_Movement : MonoBehaviour
{

    public float speed = 1.0f;
    private Material mat;
    private float transition;

    // Use this for initialization
    void Start()
    {
        mat = this.gameObject.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.GetComponent<Colorable>().curState == state.RGB || this.gameObject.GetComponent<Colorable>().curState == state.D)
        {
            transition = mat.GetFloat("_Transition");
            this.transform.Rotate(0.0f, 0.0f, 1 * speed * transition);
        }
    }
}
