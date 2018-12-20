using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField] private GameRules gameRules;
    [SerializeField] private bool DebugEnableDeath = true;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!DebugEnableDeath) return;

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

    private IEnumerator WaitEndOfInvicibility(GameObject entity)
    {
        PlayerMovement player = entity.GetComponent<PlayerMovement>();

        yield return (new WaitUntil(() => !player.Invicible));
        if (entity.transform.position.y < gameObject.transform.position.y)
            this.KillPlayer(entity);
    }

    private void KillPlayer(GameObject player)
    {
        gameRules.UpdateNbPlayersAlive(player);
        player.SetActive(false);
    }
}