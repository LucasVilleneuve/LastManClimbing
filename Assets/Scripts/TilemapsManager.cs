﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapsManager : MonoBehaviour {

    public GameObject starter;
    public GameObject[] maps;
    public float posX;
    public float speed;

    private GameObject lastInstance;
    private List<GameObject> chunks = new List<GameObject>();
    private Vector3 pos;

	void Start () {
        pos = new Vector3(posX, 0, 0);
        addStarter(new Vector3(posX, -5, 0));
        addChunk(new Vector3(posX, 5, 0));
    }

    private void addStarter(Vector3 pos)
    {
        chunks.Add(Instantiate(starter, pos, Quaternion.identity));
    }

    private void addChunk(Vector3 pos)
    {
        int mapNb = Random.Range(0, maps.Length);

        print("new chunk [" + mapNb + "] at position " + pos.x + "," + pos.y);
        chunks.Add(Instantiate(maps[mapNb], pos, Quaternion.identity));
    }

    // put this update as first
	void Update () {

        Vector3 relativeSpeed = new Vector3(0, Time.deltaTime * -speed, 0);

        for (int i = chunks.Count - 1; i >= 0; i--)
        {
            if (!chunks[i].GetComponent<ChunkManager>().Scroll(relativeSpeed))
            {
                // add new chunk
                pos.y = chunks[chunks.Count-1].transform.position.y + 10;
                addChunk(pos);

                // remove old chunk
                Destroy(chunks[i]);
                chunks.RemoveAt(i);
            }
        }
	}
}