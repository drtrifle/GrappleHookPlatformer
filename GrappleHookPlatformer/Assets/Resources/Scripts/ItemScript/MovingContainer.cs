using UnityEngine;
using System.Collections;

public class MovingContainer : MonoBehaviour {
    public Transform[] Waypoints;
    public float speed = 2;

    public int CurrentPoint = 0;

    private int currentdirection = 1;                  //Either 1 or -1

    void Update() {
        if (transform.position != Waypoints[CurrentPoint].transform.position) {
            transform.position = Vector3.MoveTowards(transform.position, Waypoints[CurrentPoint].transform.position, speed * Time.deltaTime);
        }

        if (transform.position == Waypoints[CurrentPoint].transform.position) {
            CurrentPoint += currentdirection;
        }

        if (CurrentPoint >= Waypoints.Length) {
            currentdirection = -1;
            CurrentPoint = Waypoints.Length-1;
        }
        if (CurrentPoint < 0) {
            currentdirection = 1;
            CurrentPoint = 1;
        }
    }
}