using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LaserStreamTurretEnemy : Enemy {

    //Animation Vars
    private Animator animator;

    //Stream Turret Vars
    public GameObject projectilePrefab;
    public LayerMask hitLayers;
    public List<GameObject> laserStreamList;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        SetUpLaserStream();
    }

    //Don't damage player on contact
    protected override void OnCollisionEnter2D(Collision2D collision) {
        
    }

    void SetUpLaserStream() {
        //Raycast to determine distance
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position, transform.up, 50f, hitLayers);

        if (hit.collider != null) {
            Debug.Log(hit.point);
            Debug.Log(hit.distance);
            for(float i= 1f; i<= hit.distance; i += 1f) {
                Vector3 beamPosition = transform.position + new Vector3(0, i, 0);
                GameObject clone = Instantiate(projectilePrefab, beamPosition, transform.rotation, transform);
                laserStreamList.Add(clone);
            }
        }
    }

}
