using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Weapon : MonoBehaviour {

    public float fireRate = 0f;
    public float Damage = 10;
    public LayerMask notToHit;

    //BulletTrail Effect Vars
    public Transform bulletTrailPrefab;
    private float timeToSpawnEffect = 0f;
    public float effectSpawnRate = 10f;

    private float timeToFire = 0f;
    private Transform firePoint;

	// Use this for initialization
	void Start () {
        firePoint = transform.Find("FirePoint");
        Assert.IsNotNull(firePoint,"transform firePoint is null");


	}
	
	// Update is called once per frame
	void Update () {
        //Check if Single Burst
		if(fireRate == 0) {
            if (Input.GetButtonDown("Fire1")) {
                Shoot();
            }
        } else {
            if (Input.GetButton("Fire1") && Time.time > timeToFire) {
                timeToFire = Time.time + 1 / fireRate;
                Shoot();
            }
        }
	}

    void Shoot() {
        Vector3 mousePositionV3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePositionV2 = new Vector2(mousePositionV3.x, mousePositionV3.y);
        Vector2 firePointPosition = firePoint.position;

        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePositionV2 - firePointPosition, 100, ~notToHit);


        if(Time.time >= timeToSpawnEffect) {
            SpawnEffect();
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }

        Debug.DrawLine(firePointPosition, mousePositionV2);
        if(hit.collider != null) {
            Debug.DrawLine(firePointPosition, hit.point, Color.red);
            Debug.Log("Hit " + hit.collider.name + " and did " + Damage + "damage");
        }
    }

    private void SpawnEffect() {
        Instantiate(bulletTrailPrefab, firePoint.position, firePoint.rotation);
    }
}
