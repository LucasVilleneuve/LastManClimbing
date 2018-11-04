using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour {

    private float minY = -10;

    // if the chunk goes under the camera, it returns false to be deleted by the manager
	public bool Scroll (Vector3 relativeSpeed) {
        if (transform.position.y > minY)
        {
            transform.Translate(relativeSpeed);
        }
        else { return false; }
        return true;
    }
}
