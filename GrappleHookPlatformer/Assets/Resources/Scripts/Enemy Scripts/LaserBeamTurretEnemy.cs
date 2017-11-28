using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LaserBeamTurretEnemy : Enemy {

    //Projectile Firing Vars
    public float firingInterval;
    public GameObject projectilePrefab;
    public Transform projectilePool;

    //Animation Vars
    private Animator animator;

    // Use this for initialization
    void Start () {

        animator = GetComponent<Animator>();
        projectilePool = GameObject.Find("ProjectilePool").transform;

        StartCoroutine("FireProjectileSequence");
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

}
