using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingManager : MonoBehaviour
{

    private float freezeSpeed = 0;
    private CameraMovement cam;

    void Start()
    {
        cam = gameObject.GetComponentInParent<CameraMovement>();
    }

    private void OnTriggerEnter2D(Collider2D entity)
    {
        if (entity.tag == "Player")
        {
            //PlayerMovement player = entity.GetComponent<PlayerMovement>();
            //freezeSpeed = cam.getCameraSpeed();
            //cam.setCameraSpeed(player.climbingSpeed);
        }
    }

    private void OnTriggerExit2D(Collider2D entity)
    {
        if (entity.tag == "Player")
        {
            //cam.setCameraSpeed(freezeSpeed);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
