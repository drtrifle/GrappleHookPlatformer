using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {

    public static GameMaster gameMaster;  

    //Vars for spawning player;
    public Transform playerPrefab;
    public Transform spawnPoint;
    public int spawnDelay = 2;
    public GameObject spawnPrefab; //Prefab of particle effects when player spawns
    public GameObject levelClearedText;
    private bool isPlayerRespawning = false;
    AudioSource audioSource;

    //Vars for Timer
    public Timer timerText;

    //Vars for playerHealth
    private int playerHealth = 3;
    public HealthUIContainer healthUIScript;

    //Vars for playerCoins
    private int playerCoins = 0;
    public Text playerCoinText;

    private void Start() {
        if (gameMaster == null) {
            gameMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        }

        audioSource = GetComponent<AudioSource>();
    }

    public IEnumerator RespawnPlayer() {
        audioSource.Play();
        yield return new WaitForSeconds(spawnDelay);
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        GameObject particleClone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation);
        Destroy(particleClone, 3f);

        //ResetHealthUI
        healthUIScript.ResetHealth();

        isPlayerRespawning = false;
    }

    public void DamagePlayer(int damage) {
        healthUIScript.ReduceHealth(damage);
    }

    public static void KillPlayer(Player player) {
        GameMaster gm = GameMaster.gameMaster;

        //If player is not currently respawning
        if (!gm.isPlayerRespawning) {
            Destroy(player.gameObject);
            gameMaster.StartCoroutine(gameMaster.RespawnPlayer());
            gm.isPlayerRespawning = true;

            //Update Health UI
            gm.healthUIScript.ReduceHealth(3);
        }

    }

    public static void AddCoins(int value) {
        GameMaster gm = GameMaster.gameMaster;
        gm.playerCoins += value;

        //Update Coin UI
        gm.playerCoinText.text = "Coins: " + gm.playerCoins.ToString("D3");
    }


        public void WinLevel() {
        levelClearedText.SetActive(true);
        timerText.StopTimer();
    }

}
