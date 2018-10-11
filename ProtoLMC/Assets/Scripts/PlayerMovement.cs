using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    // Serialized fields
    [SerializeField] private float climbingSpeed = 5.0f;
    [SerializeField] private float jetpackHSpeed = 25.0f;
    [SerializeField] private float maxJetpackVerticalVelocity = 10.0f; // Max vertical velocity while using jetpack.
    [SerializeField] private float maxJetpackHorizontalVelocity = 10.0f; // Max horizontal velocity while using jetpack.
    [SerializeField] private float gravityScale = 3f;
    [SerializeField] [Range (30, 100)] private float jetpackForce = 40f;

    // Components
    private Rigidbody2D rb;
    private ParticleSystem ps;
    private CharacterController2D controller;

    // Private fields
    private float horizontalMovement = 0f;
    private float verticalMovement = 0f;
    private bool usingJetpack = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ps = GetComponent<ParticleSystem>();
        controller = GetComponent<CharacterController2D>();
        //PlayJetpackEmission(false);

        if (usingJetpack)
        {
            rb.gravityScale = 3;
        }
        else
        {
            rb.gravityScale = 0;
        }
    }

    void Update()
    {
        horizontalMovement = Input.GetAxis("Horizontal");
        // TODO  Which is better ?
        //horizontalMovement = Input.GetAxisRaw("Horizontal");

        verticalMovement = Input.GetAxis("Vertical");

        usingJetpack = Input.GetButton("Jetpack");

        if (usingJetpack)
        {
            horizontalMovement *= jetpackHSpeed;
            verticalMovement = 0;
        }
        else
        {
            horizontalMovement *= climbingSpeed;
            verticalMovement *= climbingSpeed;
        }

        //if (Input.GetKeyDown("space"))
        //{
        //    usingJetpack = !usingJetpack;
        //    rb.gravityScale = usingJetpack ? 1 : 0;

        //    if (!usingJetpack)
        //    {
        //        rb.velocity = Vector3.zero;
        //    }
        //}
    }

    void FixedUpdate ()
    {
        if (usingJetpack)
        {
            // Player movement
            rb.AddForce(new Vector2(horizontalMovement, jetpackForce));

            // Clamp vertical velocity to no go too fast
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxJetpackHorizontalVelocity, maxJetpackHorizontalVelocity),
                Mathf.Clamp(rb.velocity.y, -maxJetpackVerticalVelocity, maxJetpackVerticalVelocity));
        }
        else // Climbing
        {
            controller.Move(horizontalMovement * Time.fixedDeltaTime,
                verticalMovement * Time.fixedDeltaTime);
        }

        //float moveHorizontal = Input.GetAxis("Horizontal");
        //float moveVertical = Input.GetAxis("Vertical");

        //if (usingJetpack) // Jetpack movement
        //{
        //    if (moveVertical > 0.0f)
        //    {
        //        PlayJetpackEmission();

        //        var x = moveHorizontal * jetpackHSpeed;
        //        var y = moveVertical * jetpackVSpeed;

        //        rb.AddForce(new Vector2(x, y));

        //        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxJetpackVelocity);

        //    }
        //    else
        //    {
        //        PlayJetpackEmission(false);
        //    }

        //}
        //else // Climbing movement
        //{
        //    var x = moveHorizontal * Time.deltaTime * climbingSpeed;
        //    var y = moveVertical * Time.deltaTime * climbingSpeed;

        //    transform.Translate(x, y, 0);
        //}
    }

    void    PlayJetpackEmission(bool play = true)
    {
        //if (!play)
        //{
        //    if (ps.isPlaying)
        //    {
        //        Debug.Log("Stopping emission");
        //        ps.Stop();
        //    }
        //}
        //else
        //{
        //    if (!ps.isEmitting)
        //    {
        //        Debug.Log("Play emission");
        //        ps.Play();
        //    }
        //}
    }
}
