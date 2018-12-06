using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRock : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
        this.RockDestruction();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();

            playerMovement.HitThePlayer();
        }
    }

    private void RockDestruction()
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        float bottomCameraPosition = cameraPosition.y - Camera.main.orthographicSize - GetComponent<SpriteRenderer>().bounds.size.y;

        if (transform.position.y < bottomCameraPosition)
            Destroy(gameObject);
    }
}
