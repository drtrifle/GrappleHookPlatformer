using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        playerHealth = 3;

        isPlayerRespawning = false;
    }

    public static void DamagePlayer(Player player) {
        GameMaster gm = GameMaster.gameMaster;

        gm.healthUIScript.ReduceHealth();
        gm.playerHealth--;

        if (gm.playerHealth == 0) {
            GameMaster.KillPlayer(player);
        }
    }

    public static void KillPlayer(Player player) {
        GameMaster gm = GameMaster.gameMaster;
        if (!gm.isPlayerRespawning) {
            Destroy(player.gameObject);
            gameMaster.StartCoroutine(gameMaster.RespawnPlayer());
            gm.isPlayerRespawning = true;
        }
    }

    public void WinLevel() {
        levelClearedText.SetActive(true);
        timerText.StopTimer();
    }

}
