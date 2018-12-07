using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script to move the BulletTrailEffect
public class BulletTrail : MonoBehaviour {

    public int moveSpeed = 230;

    void Start() {
        Destroy(gameObject, 1);
    }

	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
	}
}
