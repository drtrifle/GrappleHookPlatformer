using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float speed = 1f;
    public float jumpSpeed = 3f;
    public bool isSwinging;
    private SpriteRenderer playerSprite;
    private Rigidbody2D rBody;
    private Animator animator;
    private float jumpInput;

    //Rope Swing ability vars
    public Vector2 ropeHook;
    public float swingForce = 4f;
    public bool isJumping = false;                                             // To differentiate if reson not touching ground due to jump or swing

    // Vars to Check if Player is on the ground
    [SerializeField]
    private LayerMask m_WhatIsGround;                                   // A mask determining what is ground to the character
    private Transform m_GroundCheck;                                    // A position marking where to check if the player is grounded.
    const float k_GroundedRadius = .2f;                                 // Radius of the overlap circle to determine if grounded
    public bool isPlayerGrounded;


    void Awake() {
        playerSprite = GetComponent<SpriteRenderer>();
        rBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        m_GroundCheck = transform.Find("GroundCheck");
    }

    void FixedUpdate() {
        CheckIfPlayerGrounded();
        HandleHorizontalInput();
        HandleVerticalInput();
    }

    #region Vertical Movement Methods
    private void HandleVerticalInput() {
        jumpInput = Input.GetAxis("Jump");

        // Don't Jump if player swinging or in the air already
        if (!isPlayerGrounded) {
            return;
        }

        if (jumpInput > 0f && !isSwinging) {
            animator.SetBool("Ground", false);
            //isJumping = true;
            rBody.velocity = new Vector2(rBody.velocity.x, jumpSpeed);
        }

    }

    #endregion

    #region Horizontal Movement Methods

    private void HandleHorizontalInput() {
        float horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput != 0f) {
            animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
            playerSprite.flipX = horizontalInput < 0f;
            if (isSwinging && !isPlayerGrounded) {
                HandleSwinging(horizontalInput);
            } else {
                //animator.SetBool("IsSwinging", false);
                HandleGroundMovement(horizontalInput);
            }
        } else {
            //animator.SetBool("IsSwinging", false);
            //animator.SetFloat("Speed", 0f);

            //Makes Player Stop horizontaly when not swinging & when no player horizontal input
            if (!isSwinging) {
                rBody.velocity = new Vector2(0, rBody.velocity.y);
            }

        }
    }

    //Handles Movement When Player is attached to rope
    private void HandleSwinging(float horizontalInput) {
        //animator.SetBool("IsSwinging", true);

        // 1 - Get a normalized direction vector from the player to the hook point
        Vector2 playerToHookDirection = (ropeHook - (Vector2)transform.position).normalized;

        // 2 - Inverse the direction to get a perpendicular direction
        Vector2 perpendicularDirection;
        if (horizontalInput < 0) {
            perpendicularDirection = new Vector2(-playerToHookDirection.y, playerToHookDirection.x);
            Vector2 leftPerpPos = (Vector2)transform.position - perpendicularDirection * -2f;
            Debug.DrawLine(transform.position, leftPerpPos, Color.green, 0f);
        } else {
            perpendicularDirection = new Vector2(playerToHookDirection.y, -playerToHookDirection.x);
            Vector2 rightPerpPos = (Vector2)transform.position + perpendicularDirection * 2f;
            Debug.DrawLine(transform.position, rightPerpPos, Color.green, 0f);
        }

        Vector2 swingForce = perpendicularDirection * this.swingForce;
        rBody.AddForce(swingForce, ForceMode2D.Force);
    }

    //Handles Movement When Player is on the ground
    private void HandleGroundMovement(float horizontalInput) {
        // Player GroundMovement
        var groundForce = speed * 2f;
        rBody.AddForce(new Vector2((horizontalInput * groundForce - rBody.velocity.x) * groundForce, 0));
        rBody.velocity = new Vector2(rBody.velocity.x, rBody.velocity.y);
    }

    #endregion

    //Checks if Player is touching the ground
    private void CheckIfPlayerGrounded() {
        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].gameObject != gameObject) {
                animator.SetBool("Ground", true);
                isPlayerGrounded = true;
                isJumping = false;
                return;
            }
        }
        isPlayerGrounded = false;
    }
}
