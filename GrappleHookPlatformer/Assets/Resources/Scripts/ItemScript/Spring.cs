using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Spring : MonoBehaviour {

    Animator springAnimator;

	// Use this for initialization
	void Start () {
        springAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        springAnimator.SetTrigger("isSprung");
    }
}
