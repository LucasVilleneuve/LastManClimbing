using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fuelTankScript : MonoBehaviour {

    [SerializeField] [Range(30, 100)] public int fuelUnit = 30;
	void Start ()
    {
        if (fuelUnit < 55)
            GetComponent<SpriteRenderer>().color = new Color(0.43f, 0.61f, 200);
        else if (fuelUnit > 80)
            GetComponent<SpriteRenderer>().color = new Color(1, 0.39f, 0.39f) ;
        else
            GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.43f, 0.6f);
    }
}
