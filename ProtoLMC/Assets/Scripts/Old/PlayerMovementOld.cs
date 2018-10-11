using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementOld : MonoBehaviour {

    public float climbingSpeed = 5.0f;
    public float jetpackVSpeed = 25.0f;
    public float jetpackHSpeed = 25.0f;
    public float maxJetpackVelocity = 10.0f;

    public bool usingJetpack = true;

    private Rigidbody2D rb;
    private ParticleSystem ps;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ps = GetComponent<ParticleSystem>();
        PlayJetpackEmission(false);
        rb.gravityScale = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            usingJetpack = !usingJetpack;
            rb.gravityScale = usingJetpack ? 1 : 0;
            
            if (!usingJetpack)
            {
                rb.velocity = Vector3.zero;
            }
        }
    }

    void FixedUpdate ()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if (usingJetpack) // Jetpack movement
        {
            if (moveVertical > 0.0f)
            {
                PlayJetpackEmission();

                var x = moveHorizontal * jetpackHSpeed;
                var y = moveVertical * jetpackVSpeed;

                rb.AddForce(new Vector2(x, y));

                rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxJetpackVelocity);

            }
            else
            {
                PlayJetpackEmission(false);
            }

        }
        else // Climbing movement
        {
            var x = moveHorizontal * Time.deltaTime * climbingSpeed;
            var y = moveVertical * Time.deltaTime * climbingSpeed;

            transform.Translate(x, y, 0);
        }
    }

    void    PlayJetpackEmission(bool play = true)
    {
        if (!play)
        {
            if (ps.isPlaying)
            {
                Debug.Log("Stopping emission");
                ps.Stop();
            }
        }
        else
        {
            if (!ps.isEmitting)
            {
                Debug.Log("Play emission");
                ps.Play();
            }
        }
    }
}
