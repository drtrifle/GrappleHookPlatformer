using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float speed = 1f;
    public float jumpSpeed = 3f;
    public bool isSwinging;
    private SpriteRenderer playerSprite;
    private Rigidbody2D rBody;
    private bool isJumping;
    private Animator animator;
    private float jumpInput;
    private float horizontalInput;

    //Rope Swing ability vars
    public Vector2 ropeHook;
    public float swingForce = 4f;

    // Vars to Check if Player is on ground
    [SerializeField]
    private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
    private Transform m_GroundCheck;                                    // A position marking where to check if the player is grounded.
    const float k_GroundedRadius = .2f;                                 // Radius of the overlap circle to determine if grounded
    public bool groundCheck;


    void Awake() {
        playerSprite = GetComponent<SpriteRenderer>();
        rBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        m_GroundCheck = transform.Find("GroundCheck");
    }

    void Update() {
        jumpInput = Input.GetAxis("Jump");
        horizontalInput = Input.GetAxis("Horizontal");
        var halfHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        groundCheck = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].gameObject != gameObject)
                groundCheck = true;
        }
    }

    void FixedUpdate() {
        if (horizontalInput < 0f || horizontalInput > 0f) {
            //animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
            playerSprite.flipX = horizontalInput < 0f;
            if (isSwinging) {
                //animator.SetBool("IsSwinging", true);

                // 1 - Get a normalized direction vector from the player to the hook point
                var playerToHookDirection = (ropeHook - (Vector2)transform.position).normalized;

                // 2 - Inverse the direction to get a perpendicular direction
                Vector2 perpendicularDirection;
                if (horizontalInput < 0) {
                    perpendicularDirection = new Vector2(-playerToHookDirection.y, playerToHookDirection.x);
                    var leftPerpPos = (Vector2)transform.position - perpendicularDirection * -2f;
                    Debug.DrawLine(transform.position, leftPerpPos, Color.green, 0f);
                } else {
                    perpendicularDirection = new Vector2(playerToHookDirection.y, -playerToHookDirection.x);
                    var rightPerpPos = (Vector2)transform.position + perpendicularDirection * 2f;
                    Debug.DrawLine(transform.position, rightPerpPos, Color.green, 0f);
                }

                var force = perpendicularDirection * swingForce;
                rBody.AddForce(force, ForceMode2D.Force);
            } else {
                //animator.SetBool("IsSwinging", false);
                if (groundCheck) {
                    var groundForce = speed * 2f;
                    rBody.AddForce(new Vector2((horizontalInput * groundForce - rBody.velocity.x) * groundForce, 0));
                    rBody.velocity = new Vector2(rBody.velocity.x, rBody.velocity.y);
                }
            }
        } else {
            //animator.SetBool("IsSwinging", false);
            //animator.SetFloat("Speed", 0f);

            //Makes Player Stop horizontaly when not swinging
            if (!isSwinging) {
                rBody.velocity = new Vector2(0, rBody.velocity.y);
            }

        }

        if (!isSwinging) {
            if (!groundCheck) return;

            isJumping = jumpInput > 0f;
            if (isJumping) {
                rBody.velocity = new Vector2(rBody.velocity.x, jumpSpeed);
            }
        }
    }
}
