using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textScaleScript : MonoBehaviour {

    private bool isAdding = true;
    public float speed = 0.05f;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isAdding)
        {
            if (transform.localScale.x > 1.2)
                isAdding = false;
            transform.localScale += new Vector3(speed, speed);
        }
        else
        {
            if (transform.localScale.x < 0.8)
                isAdding = true ;
            transform.localScale -= new Vector3(speed, speed);
        }
	}
}
