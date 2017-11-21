using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTile : MonoBehaviour {

    //Used by playerMovement to determine if player should slide on floor
    public bool isSlippery;

	// Use this for initialization
	void Start () {
        isSlippery = false;
    }
}
