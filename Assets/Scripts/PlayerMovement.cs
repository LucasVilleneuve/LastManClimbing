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
    public int dashForce = 1000;
    public Transform jetpackFuel;
    private Animator animator;

    /* Components */
    private Rigidbody2D rb;
    private CharacterController2D controller;
    //private new ParticleSystem particleSystem; // In children

    /* Private fields */
    private float horizontalInput = 0f;
    private float horizontalMovement = 0f;
    private float verticalMovement = 0f;
    private int usingJetpack = 0;
    private bool playerControlsEnabled = true;
    private int usingDash = 0;
    private bool allowDash = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //particleSystem = GetComponentInChildren<ParticleSystem>();
        controller = GetComponent<CharacterController2D>();
        jetpackAnimator.SetBool("useJetpack", false);

        if (usingJetpack != 0)
        {
            animator.SetBool("JetPack", true);
            rb.gravityScale = gravityScale;
        }
        else
        {
            animator.SetBool("JetPack", false);
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
            usingDash = UsingDash();

            if (((allowDash && usingDash != 0) || usingJetpack != 0) && jetpackFuel.localScale.y > 0)
            {
                if (allowDash && usingDash != 0)
                {
                    jetpackFuel.localScale += new Vector3(0, -0.5f);
                    if (jetpackFuel.localScale.y < 0)
                        jetpackFuel.localScale = new Vector3(jetpackFuel.localScale.x, 0);
                    jetpackAnimator.SetBool("useJetpack", true);
                    animator.SetBool("JetPack", true);
                }
                else if (usingJetpack != 0)
                {
                    jetpackFuel.localScale += new Vector3(0, -0.015f);
                    if (jetpackFuel.localScale.y < 0)
                        jetpackFuel.localScale = new Vector3(jetpackFuel.localScale.x, 0);
                    horizontalMovement *= jetpackHAcceleration;
                    verticalMovement = 0;
                    jetpackAnimator.SetBool("useJetpack", true);
                    animator.SetBool("JetPack", true);
                }
            }
            else
            {
                if (jetpackFuel.localScale.y < 5)
                    jetpackFuel.localScale += new Vector3(0, 0.01f);
                horizontalMovement *= climbingSpeed;
                verticalMovement *= climbingSpeed;
                jetpackAnimator.SetBool("useJetpack", false);
                animator.SetBool("JetPack", false);
                animator.SetBool("Moving", (verticalMovement != 0f || horizontalMovement != 0f));
            }
        }
    }

    private IEnumerator CoolDownDash()
    {
        yield return new WaitForSeconds(1);
        allowDash = true;
    }

    private void FixedUpdate()
    {
        if (playerControlsEnabled)
        {
            if (((allowDash && usingDash != 0) || usingJetpack != 0) && jetpackFuel.localScale.y > 0)
            {
                if (allowDash && usingDash != 0)
                {
                    rb.AddForce(new Vector2(usingDash * dashForce, 0));

                    // Clamp vertical velocity to no go too fast
                    rb.velocity = ClampVelocity(rb.velocity);
                    animator.SetBool("JetPack", true);
                    allowDash = false;
                    StartCoroutine(CoolDownDash());
                }
                else if (usingJetpack != 0)
                {
                    // Player movement
                    rb.AddForce(new Vector2(horizontalMovement, usingJetpack * jetpackVAcceleration));

                    // Clamp vertical velocity to no go too fast
                    rb.velocity = ClampVelocity(rb.velocity);
                    animator.SetBool("JetPack", true);
                }
            }
            else // Climbing
            {
                animator.SetBool("JetPack", false);
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

    private int UsingDash()
    {
        if (Input.GetButton(GetInputNameForPlayer("LB")))
            return (-1);
        if (Input.GetButton(GetInputNameForPlayer("RB")))
            return (1);
        if (Input.GetButton(GetInputNameForPlayer("A")))
            return (2);
        return (0);
    }

    private int UsingJetpack()
    {
        if (Input.GetAxis(GetInputNameForPlayer("LT")) == 1.0f)
            return (-1);
        if ((Input.GetAxis(GetInputNameForPlayer("RT")) == -1.0f ||
             Input.GetButton("DebugJetpackKeyboardP" + playerId) == true))
            return (1);
        return (0);
        /*return (Input.GetAxis(GetInputNameForPlayer("LT")) == 1.0f) ||
                (Input.GetAxis(GetInputNameForPlayer("RT")) == -1.0f) ||
                (Input.GetButton("DebugJetpackKeyboardP" + playerId) == true);
                */
    }

    public void EnablePlayerControls(bool enable)
    {
        rb.velocity = new Vector3();
        jetpackAnimator.SetBool("useJetpack", false);
        playerControlsEnabled = enable;
    }

    public void HitThePlayer()
    {
        StartCoroutine(PlayerIsWounded());
    }

    private IEnumerator PlayerIsWounded()
    {
        float speedValue = -5;

        this.AddSpeedValue(speedValue);
        yield return (new WaitForSecondsRealtime(2));
        this.AddSpeedValue(-speedValue);
    }

    public void AddSpeedValue(float speedValue)
    {
        this.climbingSpeed += speedValue;
        this.jetpackVAcceleration += speedValue;
        this.maxJetpackVerticalVelocity += speedValue;
        this.jetpackHAcceleration += speedValue;
        this.maxJetpackHorizontalVelocity += speedValue;
    }
}