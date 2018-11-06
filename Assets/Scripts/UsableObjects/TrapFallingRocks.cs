using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapFallingRocks : CollectableObject {

    public GameObject fallingRockPrefab = null;

	void Start () {
        this.actionOnTriggerEnter = InstantiateFallingRock;
    }
	
	void Update () {
		
	}

    void InstantiateFallingRock(GameObject entity)
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 rockPosition = entity.transform.position;
        float fallingRockHeight = fallingRockPrefab.GetComponent<SpriteRenderer>().bounds.size.y;

        rockPosition.y = cameraPosition.y + Camera.main.orthographicSize + fallingRockHeight;
        GameObject newRock = Instantiate(this.fallingRockPrefab, rockPosition, Quaternion.identity);
    }
}
