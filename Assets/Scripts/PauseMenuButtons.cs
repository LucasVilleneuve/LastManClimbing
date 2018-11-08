using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuButtons : MonoBehaviour {

	public void ResumeMenuButtonClick()
    {
        Time.timeScale = 1f;
        SceneManager.UnloadSceneAsync("PauseHUD");
    }

    public void OptionsMenuButtonClick()
    {

    }

    public void QuitMenuButtonClick()
    {
        SceneManager.UnloadSceneAsync("PauseHUD");
        SceneManager.LoadScene("MainMenu");
    }
}
