using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    // Remap value
    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}

public class PlayerMovement : MonoBehaviour
{
    /* Serialized fields */
    [SerializeField] [Range(1, 4)] public int playerId = 1;
    [SerializeField] [Range(0, 50)] public float climbingSpeed = 5.0f;
    [SerializeField] [Range(0, 100)] public float jetpackVAcceleration = 10.0f;
    [SerializeField] [Range(0, 100)] public float jetpackHAcceleration = 10.0f;
    [SerializeField] public float maxJetpackVerticalVelocity = 10.0f; // Max vertical velocity while using jet pack.
    [SerializeField] public float maxJetpackHorizontalVelocity = 10.0f; // Max horizontal velocity while using jet pack.
    [SerializeField] private float gravityScale = 3.0f;
    [SerializeField] private Animator jetpackAnimator;

    /* Components */
    private Rigidbody2D rb;
    private CharacterController2D controller;
    //private new ParticleSystem particleSystem; // In children

    /* Private fields */
    private float horizontalInput = 0f;
    private float horizontalMovement = 0f;
    private float verticalMovement = 0f;
    private bool usingJetpack = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        //particleSystem = GetComponentInChildren<ParticleSystem>();
        controller = GetComponent<CharacterController2D>();
        jetpackAnimator.SetBool("useJetpack", false);

        if (usingJetpack)
        {
            rb.gravityScale = gravityScale;
        }
        else
        {
            rb.gravityScale = 0;
        }
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis(GetInputNameForPlayer("Direction X"));
        horizontalMovement = horizontalInput;
        // TODO  Which is better ?
        //horizontalMovement = Input.GetAxisRaw("Horizontal");

        verticalMovement = Input.GetAxis(GetInputNameForPlayer("Direction Y"));
        usingJetpack = Input.GetButton(GetInputNameForPlayer("LT"));

        if (usingJetpack)
        {
            horizontalMovement *= jetpackHAcceleration;
            verticalMovement = 0;
            jetpackAnimator.SetBool("useJetpack", true);
        }
        else
        {
            horizontalMovement *= climbingSpeed;
            verticalMovement *= climbingSpeed;
            jetpackAnimator.SetBool("useJetpack", false);
        }
    }

    private void FixedUpdate()
    {
        if (usingJetpack)
        {
            // Player movement
            rb.AddForce(new Vector2(horizontalMovement, jetpackVAcceleration));

            // Clamp vertical velocity to no go too fast
            rb.velocity = ClampVelocity(rb.velocity);
        }
        else // Climbing
        {
            controller.Move(horizontalMovement * Time.fixedDeltaTime,
                verticalMovement * Time.fixedDeltaTime);
        }

        // Rotate jet pack emission by emitting in the opposite direction of the player velocity
        float newRotAngle = rb.velocity.x.Remap(-10.0f, 10.0f, 30.0f, -30.0f);
        Quaternion target = Quaternion.Euler(0.0f, 0.0f, newRotAngle);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, target, Time.deltaTime * 5.0f);
    }

    private string GetInputNameForPlayer(string input)
    {
        return "Joystick " + playerId.ToString() + " " + input;
    }

    // Returned a clamp vector2 by checking if the velocity is within the maxVelocity range.
    private Vector2 ClampVelocity(Vector2 velocity)

    {
        Vector2 newVelocity = velocity;

        if (rb.velocity.y > maxJetpackVerticalVelocity)
        {
            newVelocity.y = maxJetpackVerticalVelocity;
        }
        else if (rb.velocity.y < -maxJetpackVerticalVelocity)
        {
            newVelocity.y = -maxJetpackVerticalVelocity;
        }

        if (rb.velocity.x > maxJetpackHorizontalVelocity)
        {
            newVelocity.x = maxJetpackHorizontalVelocity;
        }
        else if (rb.velocity.x < -maxJetpackHorizontalVelocity)
        {
            newVelocity.x = -maxJetpackHorizontalVelocity;
        }

        return newVelocity;
    }
}