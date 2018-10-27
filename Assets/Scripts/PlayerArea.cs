using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class PlayerArea : MonoBehaviour {

    private GameObject playerMap;
    private float leftBound;
    private float rightBound;

	void Start () {
        this.FoundPlayerMap();
	}
	
	void Update () {
        if (transform.position.x < this.leftBound)
        {
            transform.position = new Vector3(this.leftBound, transform.position.y, transform.position.z);
        }
        if (transform.position.x > this.rightBound)
        {
            transform.position = new Vector3(this.rightBound, transform.position.y, transform.position.z);
        }
	}

    void FoundPlayerMap()
    {
        GameObject[] maps = GameObject.FindGameObjectsWithTag("Map");
        Renderer playerRenderer = gameObject.GetComponent<Renderer>();

        foreach (GameObject map in maps)
        {
            Tilemap tilemap = map.GetComponent<Tilemap>();
            float startX = map.transform.position.x;
            float endX = startX + (tilemap.cellSize.x * tilemap.size.x);

            if (transform.position.x > startX && transform.position.x < endX)
            {
                this.playerMap = map;
                this.leftBound = startX + tilemap.cellSize.x;
                this.rightBound = endX - tilemap.cellSize.x - playerRenderer.bounds.size.x / 2;
            }
        }
    }

    public GameObject GetPlayerMap()
    {
        return (this.playerMap);
    }
}
