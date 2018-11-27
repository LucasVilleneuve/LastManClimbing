using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapFallingRocks : CollectableObject {

    public GameObject fallingRockPrefab = null;
    [SerializeField] private int fallingRocksNumber = 2;
    [SerializeField] private float fallingRocksMaxScale = 2.0f;
    [SerializeField] private float fallingRocksMinScale = 1.0f;

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

                Debug.Log(this.fallingRocksNumber);
                for (ushort i = 0; i < this.fallingRocksNumber; i++)
                {
                    float newScale = Random.Range(this.fallingRocksMinScale, this.fallingRocksMaxScale);

                    rockPosition = new Vector3(bounds.min.x, topCameraPosition, 0);
                    rockPosition.x += Random.Range(
                        fallingRockBounds.size.x * newScale / 2, 
                        bounds.size.x - (fallingRockBounds.size.x * newScale / 2));
                    rockPosition.y += Random.Range(0, fallingRockBounds.size.y * newScale * 2);
                    tmp = Instantiate(this.fallingRockPrefab, rockPosition, Quaternion.identity);
                    tmp.transform.localScale = tmp.transform.localScale * newScale;
                }
            }
        }
    }
}
