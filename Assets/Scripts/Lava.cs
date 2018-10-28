using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField] private GameRules gameRules;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag.Equals("Player"))
        {
            string pName = collider.gameObject.name;
            Debug.Log(pName + " touched lava floor");
            gameRules.UpdateNbPlayersAlive(collider.gameObject);
            collider.gameObject.SetActive(false);
        }
    }
}