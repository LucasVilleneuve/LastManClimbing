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
        float startPos = 0;
        float spacing = 0;

        if (nbOfPlayers == 2)
        {
            startPos = 12.5f;
            spacing = 15;
        }
        else if (nbOfPlayers == 3)
        {
            startPos = 5;
            spacing = 5;
        }

        for (int i = 1; i <= nbOfPlayers; ++i)
        {
            int nb = i - 1;
            float posx = startPos + 10 + spacing * nb + 20 * nb;
            float posxMap = posx - 10;
            SpawnPlayerAndMap(i, new Vector3(posx, 2.5f, 0), new Vector3(posxMap, -1, 2), seed);
        }
    }

    private void SpawnPlayerAndMap(int playerNb, Vector3 posPlayer, Vector3 posMap, int seed)
    {
        // Spawn Player
        GameObject player = Instantiate(playerPrefab, posPlayer, Quaternion.identity);
        player.name = "Player " + playerNb;
        player.GetComponent<PlayerMovement>().playerId = playerNb;
        players.Add(player);
        playersAlive.Add(playerNb);

        // Spawn map associated
        GameObject map = Instantiate(mapManagerPrefab, posMap, Quaternion.identity);
        map.GetComponent<TilemapsManager>().Init(seed);
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