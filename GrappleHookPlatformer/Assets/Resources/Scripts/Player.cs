using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [System.Serializable]
    public class PlayerStats {
        public int health = 100;
    }

    public PlayerStats playerStats = new PlayerStats();

    public int fallBoundary = -20;

    void Update() {
        //Kill player if they fell outside the world
        if (transform.position.y <= fallBoundary) {
            DamagePlayer(9999999);
        }
    }

    //Deals damage to player's health
    public void DamagePlayer(int damage) {
        playerStats.health -= damage;

        if(playerStats.health <= 0) {
            GameMaster.KillPlayer(this);
        }
    } 
}
