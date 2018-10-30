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
    public Transform jetpackFuel;

    /* Components */
    private Rigidbody2D rb;
    private CharacterController2D controller;
    //private new ParticleSystem particleSystem; // In children

    /* Private fields */
    private float horizontalInput = 0f;
    private float horizontalMovement = 0f;
    private float verticalMovement = 0f;
    private bool usingJetpack = false;
    private bool playerControlsEnabled = true;

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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "FuelTank")
        {
            if (jetpackFuel.localScale.y < 5)
            {
                jetpackFuel.localScale += new Vector3(0,
                (float)collision.gameObject.GetComponent<fuelTankScript>().fuelUnit / 100);
                Destroy(collision.gameObject);
                if (jetpackFuel.localScale.y > 5)
                    jetpackFuel.localScale = new Vector3(jetpackFuel.localScale.x, 5);

            }
        }
    }

    private void Update()
    {
        if (playerControlsEnabled)
        {
            horizontalInput = Input.GetAxis(GetInputNameForPlayer("Direction X"));
            horizontalMovement = horizontalInput;
            // TODO  Which is better ?
            //horizontalMovement = Input.GetAxisRaw("Horizontal");

            verticalMovement = Input.GetAxis(GetInputNameForPlayer("Direction Y"));
            usingJetpack = UsingJetpack();

            if (usingJetpack && jetpackFuel.localScale.y > 0)
            {
                jetpackFuel.localScale += new Vector3(0, -0.01f);
                if (jetpackFuel.localScale.y < 0)
                    jetpackFuel.localScale = new Vector3(jetpackFuel.localScale.x, 0);
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
    }

    private void FixedUpdate()
    {
        if (playerControlsEnabled)
        {
            if (usingJetpack && jetpackFuel.localScale.y > 0)
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

    private bool UsingJetpack()
    {
        return (Input.GetAxis(GetInputNameForPlayer("LT")) == 1.0f) || 
                (Input.GetAxis(GetInputNameForPlayer("RT")) == -1.0f) ||
                (Input.GetButton("DebugJetpackKeyboardP" + playerId) == true);
    }

    public void EnablePlayerControls(bool enable)
    {
        rb.velocity = new Vector3();
        jetpackAnimator.SetBool("useJetpack", false);
        playerControlsEnabled = enable;
    }
}