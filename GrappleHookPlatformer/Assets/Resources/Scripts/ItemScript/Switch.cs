﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {

    public bool startOn;

    public bool isOn;
    private bool isTriggered;

	// Use this for initialization
	void Start () {
        isTriggered = false;
        isOn = startOn;
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (!isTriggered) {
            isTriggered = true;
            isOn = !startOn;
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (!isTriggered) {
            isTriggered = true;
            isOn = !startOn;
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if (isTriggered) {
            isTriggered = false;
            isOn = startOn;
        }
    }

    //If button is On, return true, else false
    public bool GetOnState() {
        return isOn;
    }
}
