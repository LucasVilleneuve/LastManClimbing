using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartArea : MonoBehaviour
{
    [SerializeField] private CameraMovement cam;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            cam.StartMove();
        }
    }
}