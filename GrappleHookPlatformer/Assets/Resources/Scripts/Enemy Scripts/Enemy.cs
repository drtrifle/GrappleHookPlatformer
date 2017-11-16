using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    protected virtual void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Player")) {
            Player player = collider.gameObject.GetComponent<Player>();
            GameMaster.DamagePlayer(player);
        }
    }
}
