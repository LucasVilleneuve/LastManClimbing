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

    /* Private fields */
    private Queue<GameObject> chunks = new Queue<GameObject>();
    private Vector3 pos;
    private int lastPosCam = 0;
    private bool firstPassInUpdate = true;

    private void Start()
    {
        pos = transform.position;
        pos.y += chunkHeight;
        AddStarter(pos);
        pos.y += chunkHeight;
        AddChunk(pos);
    }

    private void AddStarter(Vector3 pos)
    {
        GameObject tmp = Instantiate(starter, pos, Quaternion.identity);
        //tmp.GetComponent<ChunkManager>().setMinY(minY);
        chunks.Enqueue(tmp);
    }

    private void AddChunk(Vector3 pos)
    {
        int mapNb = Random.Range(0, maps.Length);

        GameObject tmp = Instantiate(maps[mapNb], pos, Quaternion.identity);
        //tmp.GetComponent<ChunkManager>().setMinY(minY);
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
}