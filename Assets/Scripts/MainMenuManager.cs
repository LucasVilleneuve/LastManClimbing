using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    static public int NbPlayers;

    public void TwoPButtonClick()
    {
        NbPlayers = 2;
        SceneManager.LoadScene("MainScene");
    }

    public void TreePButtonClick()
    {
        NbPlayers = 3;
        SceneManager.LoadScene("MainScene");
    }

    public void FourPButtonClick()
    {
        NbPlayers = 4;
        SceneManager.LoadScene("MainScene");
    }

    public void OptionsMenuButtonClick()
    {

    }

    public void QuitMenuButtonClick()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
