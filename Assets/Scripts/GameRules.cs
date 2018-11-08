using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameRules : MonoBehaviour
{
    /* Serialized fields */
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private WinnerCutScene winCs;
    [SerializeField] private GameObject mapManagerPrefab;

    /* Debug */
    [SerializeField] [Range(2, 4)] private int debugNbPlayers = 2;

    /* Private fields */
    private int nbPlayers = 0;
    private List<GameObject> players = new List<GameObject>();
    private List<int> playersAlive = new List<int>();

    private void Awake()
    {
        nbPlayers = MainMenuManager.NbPlayers;
        if (nbPlayers == 0)
            nbPlayers = debugNbPlayers;
        Debug.Log("There is " + nbPlayers + "players.");
        SpawnPlayers(nbPlayers);
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
        lastPlayer.transform.rotation = new Quaternion(0, 0, 0, 0);
        lastPlayer.GetComponent<PlayerMovement>().EnablePlayerControls(false);
        lastPlayer.GetComponent<PlayerArea>().EnablePlayerArea(false);
        mainCamera.GetComponent<CameraMovement>().EnableMovement(false);
        Debug.Log(lastPlayer.name + " won !");
        winCs.Activate(lastPlayer);
    }

    private void SpawnPlayers(int nbOfPlayers)
    {
        int seed = (int)System.DateTime.Now.Ticks;

        for (int i = 1; i <= nbOfPlayers; ++i)
        {
            // Spawn Player
            GameObject player = Instantiate(playerPrefab, new Vector3(10 + 20 * (i - 1), 2.5f, 0), Quaternion.identity);
            player.name = "Player " + i;
            player.GetComponent<PlayerMovement>().playerId = i;
            players.Add(player);
            playersAlive.Add(i);

            // Spawn map associated
            GameObject map = Instantiate(mapManagerPrefab, new Vector3(20 * (i - 1), -1, 2), Quaternion.identity);
            map.GetComponent<TilemapsManager>().Init(seed);
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

    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}