using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Player")) {
            Player player = collider.gameObject.GetComponent<Player>();
            player.DamagePlayer(1);
            //GameMaster.KillPlayer(player);
        }
    }
}
