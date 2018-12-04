using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondDrill : CollectableObject
{
    [SerializeField] private int bonusDuration = 3;

    void Start ()
    {
        this.realtimeBeforeDestruction = this.bonusDuration;
        this.actionOnTriggerEnter = EnableDiamondDrill;
        this.actionBeforeDestruction = DisableDiamondDrill;
    }
	
	void Update ()
    {
		
	}

    void EnableDiamondDrill(GameObject entity)
    {
        Transform drill = entity.transform.Find("Drill");
        PlayerDrill drillManager = drill.GetComponent<PlayerDrill>();

        drill.GetComponent<SpriteRenderer>().enabled = true;
        drill.GetComponent<BoxCollider2D>().enabled = true;
        drillManager.setDrillType(PlayerDrill.DrillType.DIAMOND);
    }

    void DisableDiamondDrill(GameObject entity)
    {
        Transform drill = entity.transform.Find("Drill");
        PlayerDrill drillManager = drill.GetComponent<PlayerDrill>();

        drill.GetComponent<SpriteRenderer>().enabled = false;
        drill.GetComponent<BoxCollider2D>().enabled = false;
        drillManager.setDrillType(PlayerDrill.DrillType.DEFAULT);
    }
}
