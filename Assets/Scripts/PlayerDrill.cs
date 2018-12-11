using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrill : MonoBehaviour {

    public enum DrillType
    {
        DEFAULT,
        DIAMOND
    }

    DrillType type;

	void Start ()
    {
        this.type = DrillType.DEFAULT;
    }
	
	void Update ()
    {
        
	}

    public void SetDrillType(DrillType type)
    {
        this.type = type;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (this.type == DrillType.DIAMOND && collision.gameObject.tag != "FuelTank")
        {
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.type == DrillType.DIAMOND && collision.gameObject.tag == "FallingRock")
        {
            Destroy(collision.gameObject);
        }
    }
}
