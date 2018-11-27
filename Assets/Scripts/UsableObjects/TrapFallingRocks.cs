using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapFallingRocks : CollectableObject {

    public GameObject fallingRockPrefab = null;
    [SerializeField] private int fallingRocksNumber = 2;

    void Start () {
        this.actionOnTriggerEnter = InstantiateFallingRock;
    }
	
	void Update () {
		
	}

    void InstantiateFallingRock(GameObject entity)
    {
        Vector3 rockPosition;
        Vector3 cameraPosition = Camera.main.transform.position;
        Bounds fallingRockBounds = fallingRockPrefab.GetComponent<SpriteRenderer>().bounds;
        float topCameraPosition = cameraPosition.y + Camera.main.orthographicSize + fallingRockBounds.size.y;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject tmp;

        foreach (GameObject player in players)
        {
            if (player != entity)
            {
                Bounds bounds = player.GetComponent<PlayerArea>().GetPlayerMapBounds();

                for (ushort i = 0; i < fallingRocksNumber; i++)
                {
                    float newScale = Random.Range(0.5f, 1.0f);
                    print("spawning rock");
                    rockPosition = new Vector3(bounds.min.x, topCameraPosition, 0);
                    rockPosition.x += Random.Range(fallingRockBounds.size.x / 2, bounds.size.x - (fallingRockBounds.size.x / 2));
                    rockPosition.y += Random.Range(0, fallingRockBounds.size.y * 2);
                    tmp = Instantiate(this.fallingRockPrefab, rockPosition, Quaternion.identity);
                    //tmp.transform.localScale = new Vector3(newScale, newScale, 0); // TODO Scale create an issue with the material
                }
            }
        }
    }
}
