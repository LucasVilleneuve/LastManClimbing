using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRules : MonoBehaviour
{
    /* Serialized fields */
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private WinnerCutScene winCs;

    /* Debug */
    [SerializeField] [Range (2, 4)] private int nbPlayers = 2;

    /* Private fields */
    private List<GameObject> players = new List<GameObject>();
    private List<int>   playersAlive = new List<int>();

    private void Awake()
    {
        SpawnPlayers(nbPlayers);
        string[] s = Input.GetJoystickNames();
        foreach (string str in s)
        {
            Debug.Log(str);
        }
    }

    // Called whenever a player is killed by the lava.
    public void UpdateNbPlayersAlive(GameObject player)
    {
        int id = player.GetComponent<PlayerMovement>().playerId;

        playersAlive.Remove(id);
        if (playersAlive.Count == 1)
        {
            GameObject lastPlayer = FindPlayerById(playersAlive[0]);
            if (lastPlayer)
                EndGame(lastPlayer);
            else
                Debug.Log("Error, Cannot find last player alive");
        }
    }

    public void EndGame(GameObject lastPlayer)
    {
        lastPlayer.GetComponent<PlayerMovement>().EnablePlayerControls(false);
        mainCamera.GetComponent<CameraMovement>().EnableMovement(false);
        Debug.Log(lastPlayer.name + " won !");
        winCs.Activate(lastPlayer);
    }

    private void SpawnPlayers(int nbOfPlayers)
    {
        for (int i = 1; i <= nbOfPlayers; ++i)
        {
            GameObject player = Instantiate(playerPrefab, new Vector3(10 + 20 * (i - 1), 2.5f, 0), new Quaternion());
            player.name = "Player " + i;
            player.GetComponent<PlayerMovement>().playerId = i;
            players.Add(player);
            playersAlive.Add(i);
        }
    }

    private GameObject FindPlayerById(int id)
    {
        for (int i = 0; i < players.Count; ++i)
        {
            GameObject player = players[i];
            if (player.GetComponent<PlayerMovement>().playerId == id)
                return player;
        }
        return null;
    }
}