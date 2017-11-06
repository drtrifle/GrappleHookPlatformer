using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    public static GameMaster gameMaster;

    private void Start() {
        if(gameMaster == null) {
            gameMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        }

        audioSource = GetComponent<AudioSource>();
    }

    //Vars for spawning player;
    public Transform playerPrefab;
    public Transform spawnPoint;
    public int spawnDelay = 2;
    public GameObject spawnPrefab; //Prefab of particle effects when player spawns
    AudioSource audioSource;

    public IEnumerator RespawnPlayer() {
        audioSource.Play();
        yield return new WaitForSeconds(spawnDelay);
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        GameObject particleClone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation);
        Destroy(particleClone, 3f);

    }

    public static void KillPlayer(Player player) {
        Destroy(player.gameObject);
        gameMaster.StartCoroutine(gameMaster.RespawnPlayer());
    }

}
