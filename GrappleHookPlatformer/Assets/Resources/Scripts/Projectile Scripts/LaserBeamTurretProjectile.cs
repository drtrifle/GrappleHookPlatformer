using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamTurretProjectile : Projectile {

    private Rigidbody2D rb2d;
    public float moveSpeed;

    new void Start() {
        base.Start();
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update() {
        rb2d.velocity = -transform.right * moveSpeed;
    }
}
