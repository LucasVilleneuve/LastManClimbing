using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrill : MonoBehaviour {

    public enum DrillType
    {
        DEFAULT,
        DIAMOND
    }

    private AudioSource destroyRockSound;

    DrillType type;

	void Start ()
    {
        destroyRockSound = GetComponent<AudioSource>();
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
            destroyRockSound.Play();
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.type == DrillType.DIAMOND && collision.gameObject.tag == "FallingRock")
        {
            destroyRockSound.Play();
            Destroy(collision.gameObject);
        }
    }
}
