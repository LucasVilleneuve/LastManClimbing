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
    [SerializeField] [Range(1, 4)] private int playerId = 1;
    [SerializeField] private float climbingSpeed = 5.0f;
    [SerializeField] private float jetpackHSpeed = 25.0f;
    [SerializeField] private float maxJetpackVerticalVelocity = 10.0f; // Max vertical velocity while using jet pack.
    [SerializeField] private float maxJetpackHorizontalVelocity = 10.0f; // Max horizontal velocity while using jet pack.
    [SerializeField] private float gravityScale = 3f;
    [SerializeField] [Range(30, 100)] private readonly float jetpackForce = 40f;

    /* Components */
    private Rigidbody2D rb;
    private CharacterController2D controller;
    private new ParticleSystem particleSystem; // In children

    /* Private fields */
    private float horizontalInput = 0f;
    private float horizontalMovement = 0f;
    private float verticalMovement = 0f;
    private bool usingJetpack = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        particleSystem = GetComponentInChildren<ParticleSystem>();
        controller = GetComponent<CharacterController2D>();
        PlayJetpackEmission(false);

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
        horizontalInput = Input.GetAxis(GetInputNameForPlayer("Horizontal"));
        horizontalMovement = horizontalInput;
        // TODO  Which is better ?
        //horizontalMovement = Input.GetAxisRaw("Horizontal");

        verticalMovement = Input.GetAxis(GetInputNameForPlayer("Vertical"));
        usingJetpack = Input.GetButton(GetInputNameForPlayer("Jetpack"));

        if (usingJetpack)
        {
            horizontalMovement *= jetpackHSpeed;
            verticalMovement = 0;
            PlayJetpackEmission();
        }
        else
        {
            horizontalMovement *= climbingSpeed;
            verticalMovement *= climbingSpeed;
            PlayJetpackEmission(false);
        }
    }

    private void FixedUpdate()
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

        // Rotate jet pack emission by emitting in the opposite direction of the player velocity
        float newRotAngle = rb.velocity.x.Remap(-10.0f, 10.0f, 45.0f, 135.0f);
        Quaternion target = Quaternion.Euler(newRotAngle, 90.0f, 90.0f);
        particleSystem.transform.rotation = Quaternion.Slerp(particleSystem.transform.rotation, target, Time.deltaTime * 5.0f);
    }

    private void PlayJetpackEmission(bool play = true)
    {
        if (!play)
        {
            if (particleSystem.isPlaying)
            {
                particleSystem.Stop();
            }
        }
        else
        {
            if (!particleSystem.isEmitting)
            {
                particleSystem.Play();
            }
        }
    }

    private string GetInputNameForPlayer(string input)
    {
        return input + "_P" + playerId.ToString();
    }
}