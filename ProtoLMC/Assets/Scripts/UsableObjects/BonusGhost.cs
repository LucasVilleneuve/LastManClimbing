using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusGhost : CollectableObject {

    [SerializeField] private int bonusDuration = 3;
    
    void Start () {
        this.realtimeBeforeDestruction = this.bonusDuration;
        this.actionOnTriggerEnter = DisablePlayerCollider;
        this.actionBeforeDestruction = EnablePlayerCollider;
    }
	
	void Update () {
		
	}

    void DisablePlayerCollider(GameObject entity)
    {
        Color ghost = entity.GetComponent<SpriteRenderer>().color;

        ghost.a = 0.2f;
        entity.GetComponent<Collider2D>().enabled = false;
        entity.GetComponent<SpriteRenderer>().color = ghost;
    }

    void EnablePlayerCollider(GameObject entity)
    {
        Color regular = entity.GetComponent<SpriteRenderer>().color;

        regular.a = 1.0f;
        entity.GetComponent<Collider2D>().enabled = true;
        entity.GetComponent<SpriteRenderer>().color = regular;
    }
}
