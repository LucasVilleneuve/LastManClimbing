﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : MonoBehaviour {

    // entity will be the player GameObject.
    protected delegate void Delegate(GameObject entity);

    protected Delegate actionOnTriggerEnter = null;
    protected Delegate actionBeforeDestruction = null;
    // Time in seconds before destroy gameObject after OnTriggerEnter2D call
    // Set to -1 to avoid the destruction.
    protected int realtimeBeforeDestruction = 0;

	void Start () {
		
	}
	
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject entity = collision.gameObject;

        if (entity.tag == "Player")
        {
            if (this.realtimeBeforeDestruction != -1)
            {
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
            if (this.actionOnTriggerEnter != null)
                this.actionOnTriggerEnter(entity);
            StartCoroutine(WaitAndDestroy(entity));
        }
    }

    IEnumerator WaitAndDestroy(GameObject entity)
    {
        if (this.realtimeBeforeDestruction > 0)
            yield return (new WaitForSecondsRealtime(this.realtimeBeforeDestruction));
        if (this.actionBeforeDestruction != null)
            this.actionBeforeDestruction(entity);
        if (this.realtimeBeforeDestruction != -1)
            Destroy(gameObject);
    }
}
