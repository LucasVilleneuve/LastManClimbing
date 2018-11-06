using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapsManager : MonoBehaviour {

    public const int CHUNK_W = 10;
    public const int CHUNK_H = 24;
    public const int STARTER_H = 6;


    public GameObject starter;
    public GameObject[] maps;
    public float posX;
    public float speed;

    private GameObject lastInstance;
    private List<GameObject> chunks = new List<GameObject>();
    private Vector3 pos;
    private Vector3 scale;
    private Vector2 totalSize;
    private float minY;

	void Start () {

        // Getting the preset scale for maps (and exiting if they have different ones)
        scale = maps[0].transform.localScale;
        foreach (GameObject map in maps)
        {
            if (map.transform.localScale != scale)
            {
                throw new System.Exception("tile chunks have different scales. Exiting...");
            }
        }

        // Setting relative size, pos and minimum posY of the chunks
        // (depending on the scale, the parent's position and the posX parameter)
        totalSize.x = CHUNK_W * scale.x;
        totalSize.y = CHUNK_H * scale.y;
        posX *= totalSize.x;

        pos = new Vector3(
            gameObject.transform.position.x + posX,
            gameObject.transform.position.y,
            gameObject.transform.position.z
        );

        minY = pos.y - totalSize.y;


        // Adding the starter chunk (empty, half the normal size) and the first chunk
        addStarter(new Vector3(0, -(totalSize.y / 3)*2, 0) + pos);
        addChunk(new Vector3(0, totalSize.y / 3, 0) + pos);
    }

    private void addStarter(Vector3 pos)
    {
        GameObject tmp = Instantiate(starter, pos, Quaternion.identity);
        tmp.GetComponent<ChunkManager>().setMinY(minY);
        chunks.Add(tmp);
    }

    private void addChunk(Vector3 pos)
    {
        int mapNb = Random.Range(0, maps.Length);

        print("new chunk [" + mapNb + "] at position " + pos.x + "," + pos.y);

        GameObject tmp = Instantiate(maps[mapNb], pos, Quaternion.identity);
        tmp.GetComponent<ChunkManager>().setMinY(minY);
        chunks.Add(tmp);
    }

    // put this update as first
	void Update () {

        Vector3 relativeSpeed = new Vector3(0, Time.deltaTime * -speed, 0);

        for (int i = chunks.Count - 1; i >= 0; i--)
        {
            if (!chunks[i].GetComponent<ChunkManager>().Scroll(relativeSpeed))
            {
                // add new chunk
                pos.y = chunks[chunks.Count-1].transform.position.y + totalSize.y;
                addChunk(pos);

                // remove old chunk
                Destroy(chunks[i]);
                chunks.RemoveAt(i);
            }
        }
	}
}
