using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BonusGhost : CollectableObject
{

    [SerializeField] private int bonusDuration = 3;

    void Start()
    {
        this.realtimeBeforeDestruction = this.bonusDuration;
        this.actionOnTriggerEnter = DisablePlayerCollider;
        this.actionBeforeDestruction = EnablePlayerCollider;
        this.actionInNextFrame = TeleportPlayerOutsideBloc;
    }

    void Update()
    {

    }

    void DisablePlayerCollider(GameObject entity)
    {
        Color ghost = entity.GetComponent<SpriteRenderer>().color;
        Collider2D playerCollider = entity.GetComponent<Collider2D>();

        ghost.a = 0.2f;
        playerCollider.enabled = false;
        entity.GetComponent<SpriteRenderer>().color = ghost;
    }

    void EnablePlayerCollider(GameObject entity)
    {
        Color regular = entity.GetComponent<SpriteRenderer>().color;
        Collider2D playerCollider = entity.GetComponent<Collider2D>();

        regular.a = 1.0f;
        playerCollider.enabled = true;
        entity.GetComponent<SpriteRenderer>().color = regular;
    }

    void TeleportPlayerOutsideBloc(GameObject entity)
    {
        TilemapsManager map = entity.GetComponent<PlayerArea>().GetPlayerMap();
        Transform playerTransform = entity.GetComponent<Transform>();
        
        while (!map.CellIsEmpty(playerTransform.position))
        {
            playerTransform.Translate(new Vector3(0, 1, 0));
        }
    }
}
