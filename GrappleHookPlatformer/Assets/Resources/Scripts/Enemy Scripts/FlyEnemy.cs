using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class FlyEnemy : Enemy {

    public Transform playerTransform;

    // Movement Vars
    private Rigidbody2D rb2d;
    public float moveSpeed = 1f;

    //Raycast Vars
    public LayerMask hitLayers;

    // Sprite Vars
    private SpriteRenderer spriteRenderer;

    //Coroutine Vars
    private bool isSearching = false;

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        if(playerTransform == null) {
            if (!isSearching) {
                StartCoroutine("SearchPlayer");
            }
            return;
        }

        UpdateSprite();

        //Get directional vector of player
        Vector2 playerDirection = (playerTransform.position - transform.position);
        playerDirection.Normalize();

        //Raycast to detect if player can be seen
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position, playerDirection, 30f, hitLayers);
        Debug.DrawLine(transform.position, playerDirection + (Vector2)transform.position, Color.green, 0f);

        if (hit && hit.collider.CompareTag("Player")) {
            ChasePlayer(playerDirection);
        } else {
            rb2d.velocity = Vector2.zero;
        }
    }

    void UpdateSprite() {
        if (playerTransform.position.x > transform.position.x) {
            spriteRenderer.flipX = true;
        } else {
            spriteRenderer.flipX = false;
        }
    }

    void ChasePlayer(Vector2 playerDirection) {
        rb2d.AddForce(playerDirection * moveSpeed);
    }

    IEnumerator SearchPlayer() {
        isSearching = true;
        yield return new WaitForSecondsRealtime(4f);

        GameObject player = GameObject.FindWithTag("Player");

        if(player != null) {
            playerTransform = player.transform;
            isSearching = false;
        } else {
            StartCoroutine("SearchPlayer");
        }
    }
}
