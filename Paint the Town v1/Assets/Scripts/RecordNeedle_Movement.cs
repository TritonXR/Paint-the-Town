using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordNeedle_Movement : MonoBehaviour
{

    public float speed = 1.0f;
    private Material mat;
    private Vector3 startVec;
    private float transition;

    // Use this for initialization
    void Start()
    {
        mat = this.gameObject.GetComponent<Renderer>().material;
        startVec = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.GetComponent<Colorable>().curState == state.RGB || this.gameObject.GetComponent<Colorable>().curState == state.D)
        {

            //get the objects current position and put it in a variable so we can access it later with less code
            startVec = transform.position;
            //calculate what the new Y position will be
            float newZ = Mathf.Sin(Time.time * speed);
            //set the object's Y to the new calculated Y
            transform.Translate(new Vector3(0.0f, 0.0f, newZ) * 0.0002f);
        }
    }
}
