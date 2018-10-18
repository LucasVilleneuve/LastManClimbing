using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRules : MonoBehaviour
{
    private List<string> playerNames = new List<string>();

    private void Awake()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // Storing all player names
        foreach (GameObject player in players)
        {
            playerNames.Add(player.name);
        }

        if (playerNames.Count == 1)
        {
            EndGame(playerNames[0]);
        }
    }

    // Called whenever a player is killed by the lava.
    public void UpdateNbPlayersAlive(string pName)
    {
        playerNames.Remove(pName);
        if (playerNames.Count == 1)
        {
            EndGame(playerNames[0]);
        }
    }

    public void EndGame(string winnerName)
    {
        Debug.Log(winnerName + " won !");
    }
}