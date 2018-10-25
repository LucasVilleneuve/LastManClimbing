using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRules : MonoBehaviour
{
    /* Serialized fields */
    [SerializeField] private GameObject playerPrefab;

    /* Debug */
    [SerializeField] private int nbPlayers = 2;

    private List<string> playerNames = new List<string>();

    private void Awake()
    {
        for (int i = 1; i <= nbPlayers; ++i)
        {
            GameObject player = Instantiate(playerPrefab, new Vector3(5 * i, 2.5f, 0), new Quaternion());
            player.name = "Player " + i;
            player.GetComponent<PlayerMovement>().playerId = i;
            playerNames.Add(player.name);
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