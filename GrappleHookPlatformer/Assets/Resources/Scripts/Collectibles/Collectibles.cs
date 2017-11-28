using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectibles : MonoBehaviour {

    public int value;
    private bool isCollected = false;

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Player") && !isCollected) {
            isCollected = true;
            Player player = collider.gameObject.GetComponent<Player>();
            GameMaster.AddCoins(value);
            Destroy(gameObject);
        }
    }
}
