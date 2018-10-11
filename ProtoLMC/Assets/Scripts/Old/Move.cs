using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

    public CharacterController2D controller;

    public float climbingSpeed = 50f;
    private float horizontalMovement = 0f;

    void Awake()
    {
        controller = GetComponent<CharacterController2D>();
    }

    void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal") * climbingSpeed;
    }

    void FixedUpdate ()
    {
        //bool jump = Input.GetButtonDown("Jump");

        //controller.Move(horizontalMovement * Time.fixedDeltaTime, 0f);
    }
}
