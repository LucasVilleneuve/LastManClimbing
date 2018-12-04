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

public class SpeedModifier
{
    public SpeedModifier(float val, float time) { value = val; timeRemaining = time; }
    private float timeRemaining;

    public float value;

    public bool updateTimeRemaining()
    {
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0) { return false; }
        return true;
    }
}

public class PlayerMovement : MonoBehaviour
{
    /* Serialized fields */
    [SerializeField] [Range(1, 4)] public int playerId = 1;
    [SerializeField] [Range(0, 50)] public float climbingSpeed = 20.0f;
    [SerializeField] [Range(0, 100)] public float jetpackVAcceleration = 25.0f;
    [SerializeField] [Range(0, 100)] public float jetpackHAcceleration = 25.0f;
    [SerializeField] public List<SpeedModifier> speedModifiers = new List<SpeedModifier>();
    [SerializeField] public float maxJetpackVerticalVelocity = 10.0f; // Max vertical velocity while using jet pack.
    [SerializeField] public float maxJetpackHorizontalVelocity = 20.0f; // Max horizontal velocity while using jet pack.
    [SerializeField] public float xInitialPosition;
    [SerializeField] private float gravityScale = 3.0f;
    [SerializeField] private Animator jetpackAnimator;
    [SerializeField] private float fuelConsumedByTick = 0.0025f;
    [SerializeField] private float fuelRestoredByTick = 0.0025f;
    [SerializeField] private float cooldownDash = 3.0f;
    [SerializeField] private float dashForce = 1000;
    [SerializeField] private FuelGauge fuel;

    public bool Invicible { get; set; }

    /* Components */
    private Rigidbody2D rb;
    private CharacterController2D controller;
    private Animator animator;

    /* Private fields */
    private float horizontalInput = 0f;
    private float horizontalMovement = 0f;
    private float verticalMovement = 0f;
    private bool usingJetpack = false;
    private bool playerControlsEnabled = true;
    private int usingDash = 0;
    private bool allowDash = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController2D>();
        jetpackAnimator.SetBool("useJetpack", false);
        animator.SetBool("JetPack", false);
        rb.gravityScale = 0;
        this.Invicible = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
    }

    private void ManageSpeedModifiers()
    {
        for (int i = speedModifiers.Count - 1; i >= 0; i--)
        {
            if (speedModifiers[i].updateTimeRemaining() == false)
            {
                AddSpeedValue(-speedModifiers[i].value);
                speedModifiers.RemoveAt(i);
                Debug.Log("Removing speed modifier. size: " + climbingSpeed);
            }
        }

        foreach (SpeedModifier it in speedModifiers)
        {
            if (it.updateTimeRemaining() == false)
            {

            }
        }
    }

    private void Update()
    {
        ManageSpeedModifiers();
        if (!playerControlsEnabled)
            return;

        // Get horizontal Input
        horizontalInput = Input.GetAxis(GetInputNameForPlayer("Direction X"));
        horizontalMovement = horizontalInput;

        // Get vertical Input
        verticalMovement = Input.GetAxis(GetInputNameForPlayer("Direction Y"));

        usingJetpack = UsingJetpack();
        usingDash = UsingDash();

        if (usingJetpack)
        {
            horizontalMovement *= jetpackHAcceleration;
            verticalMovement = 0;
        }
        else
        {
            horizontalMovement *= climbingSpeed;
            verticalMovement *= climbingSpeed;
        }
    }

    private void FixedUpdate()
    {
        if (!playerControlsEnabled) return;

        if (usingJetpack)
        {
            // Player movement
            rb.AddForce(new Vector2(horizontalMovement, jetpackVAcceleration));

            // Clamp vertical velocity to no go too fast
            rb.velocity = ClampVelocity(rb.velocity);

            // Consume fuel
            fuel.ConsumeFuel(fuelConsumedByTick);

            // Animations
            jetpackAnimator.SetBool("useJetpack", true);
            animator.SetBool("JetPack", true);
        }
        else // Climbing
        {
            // Player movement
            controller.Move(horizontalMovement * Time.fixedDeltaTime,
                verticalMovement * Time.fixedDeltaTime);

            // Restore fuel
            fuel.RefillFuel(fuelRestoredByTick);

            // Animations
            jetpackAnimator.SetBool("useJetpack", false);
            animator.SetBool("JetPack", false);
            animator.SetBool("Moving", (verticalMovement != 0f || horizontalMovement != 0f));
        }

        // Dash
        if (usingDash != 0 && allowDash)
        {
            UseDash();
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

    private int UsingDash()
    {
        if (Input.GetButton(GetInputNameForPlayer("LB")))
            return (-1);
        if (Input.GetButton(GetInputNameForPlayer("RB")))
            return (1);
        return (0);
    }

    private bool UsingJetpack()
    {
        bool input = (Input.GetAxis(GetInputNameForPlayer("LT")) == 1.0f) ||
                (Input.GetAxis(GetInputNameForPlayer("RT")) == -1.0f) ||
                (Input.GetButton("DebugJetpackKeyboardP" + playerId) == true);
        bool enoughFuel = (fuel.GetFuelLeft() > 0.0f);

        return input && enoughFuel;
    }

    public void addSpeedModifier(SpeedModifier mod)
    {
        speedModifiers.Add(mod);
        AddSpeedValue(mod.value);
        Debug.Log("Adding speed modifier. size: " + +climbingSpeed);
    }

    public void EnablePlayerControls(bool enable)
    {
        rb.velocity = new Vector3();
        jetpackAnimator.SetBool("useJetpack", false);
        playerControlsEnabled = enable;
    }

    public void HitThePlayer()
    {
        if (!this.Invicible)
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

    public bool IsUsingJetpack()
    {
        return usingJetpack;
    }

    private void UseDash()
    {
        rb.AddForce(new Vector2(usingDash * dashForce, 0));
        allowDash = false;
        StartCoroutine(CoolDownDash());
    }

    private IEnumerator CoolDownDash()
    {
        yield return new WaitForSeconds(cooldownDash);
        allowDash = true;
    }
}