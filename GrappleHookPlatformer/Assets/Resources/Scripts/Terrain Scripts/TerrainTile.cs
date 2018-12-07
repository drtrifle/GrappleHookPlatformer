using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTile : MonoBehaviour {

    //Used by playerMovement to determine if player should slide on floor
    public bool isSlippery = false;

    //Used by playerMovement to determine if player transform should follow platform
    public bool isMovingPlatform = false;
}
