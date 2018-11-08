using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapsManager : MonoBehaviour
{
    /* Serialized fields */
    [SerializeField] public int chunkHeight = 48;
    [SerializeField] public int chunkWidth = 20;
    [SerializeField] private GameObject starter;
    [SerializeField] private GameObject[] maps;
    [SerializeField] private GameObject gameRules;

    /* Private fields */
    private Queue<GameObject> chunks = new Queue<GameObject>();
    private Vector3 pos;
    private int lastPosCam = 0;
    private bool firstPassInUpdate = true;
    private int _seed;

    public void Init(int seed)
    {
        _seed = seed;
        SetSeed(seed);
        pos = transform.position;
        pos.y += chunkHeight;
        AddStarter(pos);
        pos.y += chunkHeight;
        AddChunk(pos);
    }

    private void AddStarter(Vector3 pos)
    {
        GameObject tmp = Instantiate(starter, pos, Quaternion.identity);
        chunks.Enqueue(tmp);
    }

    private void AddChunk(Vector3 pos)
    {
        SetSeed(_seed);
        int mapNb = Random.Range(0, maps.Length);

        GameObject tmp = Instantiate(maps[mapNb], pos, Quaternion.identity);
        chunks.Enqueue(tmp);
    }

    private void Update()
    {
        int posCam = (int)Camera.main.transform.position.y;
        if (lastPosCam != posCam)
        {
            if (posCam % (24 * 2) == 0)
            {
                pos.y += chunkHeight;
                AddChunk(pos);

                if (firstPassInUpdate)
                {
                    firstPassInUpdate = false;
                }
                else
                {
                    Destroy(chunks.Dequeue());
                }
            }
        }
        lastPosCam = posCam;
    }

    public bool CellIsEmpty(Vector3 position)
    {
        Debug.Log("CellIsEmpty ?");
        foreach (GameObject map in chunks)
        {
            foreach (Transform tile in map.transform)
            {
                float sizeX = tile.GetComponent<SpriteRenderer>().bounds.size.x;
                float sizeY = tile.GetComponent<SpriteRenderer>().bounds.size.y;
                float tileX = tile.position.x - sizeX / 2;
                float tileY = tile.position.y - sizeY / 2;

                if (position.x > tileX && position.x < (tileX + sizeX)
                    && position.y > tileY && position.y < (tileY + sizeY))
                {
                    Debug.Log("FALSE !!!!!!!!!!!!!!!!!!!!!!!!");
                    Debug.Log("tileX : " + tileX + " / playerX : " + position.x + " / tileY : " + tileY + " / playerY : " + position.y + " / sizeX : " + sizeX + " / sizeY : " + sizeY);
                    return (false);
                }
            }
        }
        return (true);
    }

    public GameObject[] GetMaps()
    {
        return (maps);
    }

    public void SetSeed(int newSeed)
    {
        Random.InitState(newSeed);
    }
}