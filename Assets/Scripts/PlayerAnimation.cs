using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {


    Animator PlayerAnimator;

    private void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown("x"))
        {
            StartCoroutine("HitAnim");
        }
    }

    IEnumerator HitAnim()
    {
        PlayerAnimator.SetBool("Hit", true);

        yield return new WaitForSeconds(0.5f);
            PlayerAnimator.SetBool("Hit", false);

    }
}
