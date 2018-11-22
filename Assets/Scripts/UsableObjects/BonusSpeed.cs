using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSpeed : CollectableObject
{

    [SerializeField] private float speedBonusValue = 10f;
    [SerializeField] private int bonusDuration = 3;

    void Start () {
        this.realtimeBeforeDestruction = this.bonusDuration;
        this.actionOnTriggerEnter = GiveBonusToPlayer;
        this.actionBeforeDestruction = ResumeBonusToPlayer;
    }
	
	
	void Update () {
		
	}

    void GiveBonusToPlayer(GameObject entity)
    {
        PlayerMovement playerMovement = entity.GetComponent<PlayerMovement>();

        playerMovement.AddSpeedValue(this.speedBonusValue);
    }

    void ResumeBonusToPlayer(GameObject entity)
    {
        PlayerMovement playerMovement = entity.GetComponent<PlayerMovement>();

        playerMovement.AddSpeedValue(-this.speedBonusValue);
    }
}
