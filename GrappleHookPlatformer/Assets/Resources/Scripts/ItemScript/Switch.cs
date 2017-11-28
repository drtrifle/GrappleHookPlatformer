using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {

    public bool startOn;

    private bool isOn;
    private bool isTriggered;

	// Use this for initialization
	void Start () {
        isTriggered = false;
        isOn = startOn;
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (!isTriggered) {
            isTriggered = true;
            isOn = !isOn;
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if (isTriggered) {
            isTriggered = false;
            isOn = !isOn;
        }
    }

    //If button is On, return true, else false
    public bool getOnState() {
        return isOn;
    }
}
