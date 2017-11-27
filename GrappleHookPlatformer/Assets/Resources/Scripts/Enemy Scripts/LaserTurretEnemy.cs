using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LaserTurretEnemy : Enemy {

    // Define possible states for enemy using an enum 
    public enum TurretType { Beam, Stream };

    // The current state of enemy
    public TurretType turretType;

    //Projectile Firing Vars
    public float firingInterval;
    public GameObject projectilePrefab;
    public Transform projectilePool;

    //Animation Vars
    private Animator animator;

    //Stream Turret Vars
    public LayerMask hitLayers;
    public List<GameObject> laserStreamList;

    // Use this for initialization
    void Start () {

        animator = GetComponent<Animator>();
        projectilePool = GameObject.Find("ProjectilePool").transform;

        switch (turretType) {
            case TurretType.Beam:
                StartCoroutine("FireProjectileSequence");
                break;
            case TurretType.Stream:
                SetUpLaserStream();
                break;
        }   
    }

    //Don't damage player on contact
    protected override void OnCollisionEnter2D(Collision2D collision) {
        
    }

    IEnumerator FireProjectileSequence() {
        while (true) {
            yield return new WaitForSeconds(firingInterval);
            animator.SetTrigger("isShooting");
            Instantiate(projectilePrefab,transform.position, transform.rotation ,projectilePool);
        }
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
                GameObject clone = Instantiate(projectilePrefab, beamPosition, transform.rotation, projectilePool);
                laserStreamList.Add(clone);
            }
        }
    }

}
