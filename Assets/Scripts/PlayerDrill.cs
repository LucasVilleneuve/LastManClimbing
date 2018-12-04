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

    public void setDrillType(DrillType type)
    {
        this.type = type;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (this.type == DrillType.DIAMOND)
        {
            Destroy(collision.gameObject);
        }
    }
}
