using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalusSpeed : CollectableObject
{

    [SerializeField] private float speedMalusValue = 10f;
    [SerializeField] private int malusDuration = 3;

    void Start()
    {
        this.type = CollectableObjectType.MALUS;
        this.realtimeBeforeDestruction = this.malusDuration;
        this.actionOnTriggerEnter = GiveBonusToPlayer;
        this.actionBeforeDestruction = ResumeBonusToPlayer;
    }

    void Update()
    {

    }

    void GiveBonusToPlayer(GameObject entity)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        PlayerMovement playerMovement;

        foreach (GameObject player in players)
        {
            if (player != entity)
            {
                playerMovement = player.GetComponent<PlayerMovement>();
                playerMovement.AddSpeedValue(-this.speedMalusValue);
            }
        }
    }

    void ResumeBonusToPlayer(GameObject entity)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        PlayerMovement playerMovement;

        foreach (GameObject player in players)
        {
            if (player != entity)
            {
                playerMovement = player.GetComponent<PlayerMovement>();
                playerMovement.AddSpeedValue(this.speedMalusValue);
            }
        }
    }
}
