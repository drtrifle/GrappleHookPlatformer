using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserStreamTurretProjectile : Projectile {

	// Don't iniitate self destruct sequence
	new void Start () {
		
	}

    protected override void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Player")) {
            Player player = collider.gameObject.GetComponent<Player>();
            player.DamagePlayer(1);
        } 
    }

    protected override void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            Player player = collision.gameObject.GetComponent<Player>();
            player.DamagePlayer(1);
        } 
    }
}
