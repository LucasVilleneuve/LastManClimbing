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
        Vector3 rockPosition;
        Vector3 cameraPosition = Camera.main.transform.position;
        float fallingRockHeight = fallingRockPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject tmp;

        foreach (GameObject player in players)
        {
            if (player != entity)
            {
                //rockPosition = player.transform.position;
                //rockPosition.y = cameraPosition.y + Camera.main.orthographicSize + fallingRockHeight;
                for (ushort i = 0; i<2; ++i)
                {
                    rockPosition = player.transform.position;
                    rockPosition.y = cameraPosition.y + Camera.main.orthographicSize + fallingRockHeight;
                    print("spawning rock");
                    rockPosition.x += Random.Range(-10f, 10.0f);
                    tmp = Instantiate(this.fallingRockPrefab, rockPosition, Quaternion.identity);
                    tmp.transform.localScale = new Vector3(Random.Range(0.75f, 2.5f), Random.Range(0.75f, 2.5f), 0);
                }
            }
        }
    }
}
