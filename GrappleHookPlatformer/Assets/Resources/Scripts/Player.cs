using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [System.Serializable]
    public class PlayerStats {
        public int health = 3;
        public bool isInvincible = false;
    }

    public PlayerStats playerStats = new PlayerStats();

    public int fallBoundary = -20;

    private GameMaster gameMaster;

    //Invinsiblility vars
    public SpriteRenderer spriteBodyRenderer;
    public SpriteRenderer spriteArmRenderer;
    private bool toggleEnableSprite;

    void Start() {
        gameMaster = GameMaster.gameMaster;
    }

    void Update() {
        //Kill player if they fell outside the world
        if (transform.position.y <= fallBoundary) {
            DamagePlayer(9999999);
        }

        if (playerStats.isInvincible) {
            spriteBodyRenderer.enabled = toggleEnableSprite;
            spriteArmRenderer.enabled = toggleEnableSprite;
            toggleEnableSprite = !toggleEnableSprite;
        }
    }

    //Deals damage to player's health
    public void DamagePlayer(int damage) {

        //Do nothing if player is Invinicible
        if (playerStats.isInvincible) {
            return;
        }

        playerStats.health -= damage;
        gameMaster.DamagePlayer(damage);

        //Kill Player if no health left
        if(playerStats.health <= 0) {
            GameMaster.KillPlayer(this);
            return;
        }

        StartCoroutine("StartInvincibleSequence");
    }

    IEnumerator StartInvincibleSequence() {
        playerStats.isInvincible = true;

        yield return new WaitForSecondsRealtime(2f);
        spriteBodyRenderer.enabled = true;
        spriteArmRenderer.enabled = true;
        playerStats.isInvincible = false;
    }
}
