using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("p"))
        {
            if (Time.timeScale != 0.00001f)
            {
                Time.timeScale = 0.00001f;
                SceneManager.LoadScene("PauseHUD", LoadSceneMode.Additive);
            }
            else
            {
                Time.timeScale = 1f;
                SceneManager.UnloadSceneAsync("PauseHUD");
            }
        }
    }
}
