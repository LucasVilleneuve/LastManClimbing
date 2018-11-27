using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class PlayerArea : MonoBehaviour
{
    private TilemapsManager playerMap;
    private float leftBound;
    private float rightBound;
    private bool enablePlayerArea = true;
    [SerializeField] private int CELL_SIZE = 2;

	void Start () {
        this.FoundPlayerMap();
	}
	
	void Update () {
        if (enablePlayerArea)
        {
            if (transform.position.x < this.leftBound)
            {
                transform.position = new Vector3(this.leftBound, transform.position.y, transform.position.z);
            }
            if (transform.position.x > this.rightBound)
            {
                transform.position = new Vector3(this.rightBound, transform.position.y, transform.position.z);
            }

            float playerHeight = gameObject.GetComponent<SpriteRenderer>().bounds.size.y;
            Vector3 cameraPosition = Camera.main.transform.position;
            float topCameraPosition = cameraPosition.y + Camera.main.orthographicSize;

            if ((transform.position.y + playerHeight / 2) > topCameraPosition)
            {
                transform.position = new Vector3(transform.position.x, topCameraPosition - playerHeight / 2, transform.position.z);
            }
        }
	}

    void FoundPlayerMap()
    {
        GameObject[] maps = GameObject.FindGameObjectsWithTag("Map");
        SpriteRenderer playerRenderer = gameObject.GetComponent<SpriteRenderer>();

        foreach (GameObject map in maps)
        {
            TilemapsManager tilemap = map.GetComponent<TilemapsManager>();
            float startX = map.transform.position.x;
            float endX = startX + (tilemap.chunkWidth);

            if (transform.position.x > startX && transform.position.x < endX)
            {
                this.playerMap = tilemap;
                this.leftBound = startX + CELL_SIZE + playerRenderer.bounds.size.x / 2;
                this.rightBound = endX - CELL_SIZE - playerRenderer.bounds.size.x / 2;
            }
        }
    }

    public TilemapsManager GetPlayerMap()
    {
        return (this.playerMap);
    }

    public Bounds GetPlayerMapBounds()
    {
        float width = this.rightBound - this.leftBound;
        Vector3 center = new Vector3(this.leftBound + (width / 2), 0, 0);
        Vector3 size = new Vector3(width, 0, 0);

        return (new Bounds(center, size));
    }

    public void EnablePlayerArea(bool enable)
    {
        enablePlayerArea = enable;
    }
}
