using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalusVision : CollectableObject
{
    
    void Start()
    {
        this.actionOnTriggerEnter = GiveBonusToPlayer;
    }


    void Update()
    {

    }

    void ResumeBonusToPlayer(GameObject entity)
    {
    }

    void GiveBonusToPlayer(GameObject entity)
    {
        GameObject[] torches = GameObject.FindGameObjectsWithTag("Torch");
        float initialPlayerPosX = entity.GetComponent<PlayerMovement>().xInitialPosition;
        float posTorchX;
        foreach (GameObject torch in torches)
        {
            posTorchX = torch.transform.position.x;
            if (posTorchX > initialPlayerPosX + 9 || posTorchX < initialPlayerPosX - 9)
            {
                torch.GetComponent<TorchScript>().isLight = 1;
                torch.GetComponent<TorchScript>().LightDone();
            }
        }
    }
}
