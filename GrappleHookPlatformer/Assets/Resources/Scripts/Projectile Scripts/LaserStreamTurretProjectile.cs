using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserStreamTurretProjectile : Projectile {

	// Don't initiate self destruct sequence
	new void Start () {
		
	}

    protected override void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Player")) {
            Player player = collider.gameObject.GetComponent<Player>();
            player.DamagePlayer(3);
        }

        if (collider.gameObject.CompareTag("Enemy")) {
            Enemy enemyScript = collider.gameObject.GetComponent<Enemy>();
            enemyScript.KillSelf();
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            Player player = collision.gameObject.GetComponent<Player>();
            player.DamagePlayer(3);
        }

        if (collision.gameObject.CompareTag("Enemy")) {
            Enemy enemyScript = collision.gameObject.GetComponent<Enemy>();
            enemyScript.KillSelf();
        }
    }
}
