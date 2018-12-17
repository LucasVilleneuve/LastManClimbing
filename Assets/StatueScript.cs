using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueScript : MonoBehaviour {

    public GameObject okPanel;
    private StartArea scriptStart;

	// Use this for initialization
	void Start ()
    {
        scriptStart = GameObject.FindGameObjectWithTag("StartArea").GetComponent<StartArea>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            scriptStart.updateReady(true);
        okPanel.SetActive(true);
    }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            scriptStart.updateReady(false);
            okPanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
