using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartArea : MonoBehaviour
{
    [SerializeField] private CameraMovement cam;
    private int nbPlayerReady = 0;

    public void updateReady(bool add)
    {
        if (add)
            nbPlayerReady++;
        else
            nbPlayerReady--;
        Debug.Log(nbPlayerReady);
        if (MainMenuManager.NbPlayers == nbPlayerReady)
            cam.StartMove();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            cam.StartMove();
        }
    }
}