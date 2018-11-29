using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField] private GameRules gameRules;
    [SerializeField] private bool DebugEnable = true;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!DebugEnable) return;

        if (collider.tag.Equals("Player"))
        {
            string pName = collider.gameObject.name;
            Debug.Log(pName + " touched lava floor");

            PlayerMovement player = collider.GetComponent<PlayerMovement>();

            if (!player.Invicible)
            {
                this.KillPlayer(collider.gameObject);
            }
            else
            {
                StartCoroutine(WaitEndOfInvicibility(collider.gameObject));
            }
        }
    }

    IEnumerator WaitEndOfInvicibility(GameObject entity)
    {
        PlayerMovement player = entity.GetComponent<PlayerMovement>();

        yield return (new WaitUntil(() => !player.Invicible));
        if (entity.GetComponent<Collider2D>().IsTouching(gameObject.GetComponent<Collider2D>()))
            this.KillPlayer(entity);
    }

    void KillPlayer(GameObject player)
    {
        gameRules.UpdateNbPlayersAlive(player);
        player.SetActive(false);
    }
}