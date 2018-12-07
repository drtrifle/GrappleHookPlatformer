using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class FrogEnemy : Enemy {

    //Frog Movement Vars
    private Rigidbody2D rb2d;
    private Vector2 jumpDirection;
    public float jumpSpeed = 1f;
    public float jumpInterval;

    //Animation Vars
    private Animator animator;

    // Use this for initialization
    void Start () {
        jumpDirection = new Vector2(-1, 1.75f);
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        StartCoroutine("JumpSequence");
    }

    IEnumerator JumpSequence() {
        while (true) {
            yield return new WaitForSecondsRealtime(jumpInterval);
            Jump();
        }
    }

    private void Jump() {
        animator.SetTrigger("isJumping");
        Vector2 velocity = jumpDirection * jumpSpeed;
        rb2d.AddRelativeForce(velocity);
    }
}
