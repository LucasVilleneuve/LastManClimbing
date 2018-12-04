using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpeedDown : MonoBehaviour
{

    private float value = -5.0f;
    private float duration = .5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();

            playerMovement.addSpeedModifier(new SpeedModifier(value, duration));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
