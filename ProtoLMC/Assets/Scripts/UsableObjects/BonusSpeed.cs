using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSpeed : MonoBehaviour {

    [SerializeField] private float speedBonusValue = 10f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject entity = collision.gameObject;

        if (entity.tag == "Player")
        {
            PlayerMovement playerMovement = entity.GetComponent<PlayerMovement>();

            playerMovement.climbingSpeed += this.speedBonusValue;
            playerMovement.jetpackVAcceleration += this.speedBonusValue;
            Destroy(gameObject);
        }
    }
}
