using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurretProjectile : Projectile {

    private Rigidbody2D rb2d;
    public float moveSpeed;

    void Start() {
        base.Start();
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update() {
        //rb2d.velocity = new Vector2(-transform.right * moveSpeed, 0);
        rb2d.velocity = -transform.right * moveSpeed;
    }
}
