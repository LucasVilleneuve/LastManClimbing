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
        GameObject playerMap = entity.GetComponent<PlayerArea>().GetPlayerMap();
        Collider2D playerCollider = entity.GetComponent<Collider2D>();
        TilemapCollider2D tilemapCollider = playerMap.GetComponent<TilemapCollider2D>();

        ghost.a = 0.2f;
        Physics2D.IgnoreCollision(playerCollider, tilemapCollider);
        entity.GetComponent<SpriteRenderer>().color = ghost;
    }

    void EnablePlayerCollider(GameObject entity)
    {
        Color regular = entity.GetComponent<SpriteRenderer>().color;
        GameObject playerMap = entity.GetComponent<PlayerArea>().GetPlayerMap();
        Collider2D playerCollider = entity.GetComponent<Collider2D>();
        TilemapCollider2D tilemapCollider = playerMap.GetComponent<TilemapCollider2D>();
        Vector3 playerPosition = entity.GetComponent<Transform>().position;

        regular.a = 1.0f;
        Physics2D.IgnoreCollision(playerCollider, tilemapCollider, false);
        entity.GetComponent<SpriteRenderer>().color = regular;
    }

    void TeleportPlayerOutsideBloc(GameObject entity)
    {
        GameObject playerMap = entity.GetComponent<PlayerArea>().GetPlayerMap();
        Transform playerTransform = entity.GetComponent<Transform>();
        Tilemap tilemap = playerMap.GetComponent<Tilemap>();
        
        while (!PlayerIsOnEmptyTile(playerTransform.position, tilemap))
        {
            playerTransform.Translate(new Vector3(0, 1, 0));
        }
    }

    bool PlayerIsOnEmptyTile(Vector3 playerPosition, Tilemap tilemap)
    {
        Vector3Int cellPosition = tilemap.WorldToCell(playerPosition);
        TileBase tile = tilemap.GetTile(cellPosition);

        return (tile == null);
    }
}
