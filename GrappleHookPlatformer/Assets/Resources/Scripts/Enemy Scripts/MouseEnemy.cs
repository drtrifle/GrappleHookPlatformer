using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class MouseEnemy : Enemy {


    //Mouse Movement Vars
    private Rigidbody2D rb2d;
    private Vector2 moveDirection;
    public float moveSpeed = 1f;

    //Raycast Vars
    public LayerMask hitLayers;

    //Sprite Vars
    private SpriteRenderer spriteRenderer;
    private bool flipSprite = true;

    // Use this for initialization
    void Start () {
        moveDirection = -transform.right;
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    
    void FixedUpdate () {

        //Move mouse enemy forward
        rb2d.velocity = (moveDirection * moveSpeed);

        //Raycast to detect if block in front
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position, moveDirection, 1f, hitLayers);
        Debug.DrawLine(transform.position, moveDirection + (Vector2)transform.position, Color.green, 0f);

        if (hit && hit.collider.CompareTag("Terrain") ){
            changeDirection();
        }
    }

    private void changeDirection() {
        moveDirection *= -1f;
        spriteRenderer.flipX = flipSprite;
        flipSprite = !flipSprite;
    }

}
