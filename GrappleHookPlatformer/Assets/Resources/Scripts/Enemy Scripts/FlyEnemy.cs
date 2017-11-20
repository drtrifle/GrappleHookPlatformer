using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FlyEnemy : Enemy {

    //What to chase
    public Transform target;

    //Time per second that we will update path
    public float updateRate = 2f;

    //Caching
    private Seeker seeker;
    private Rigidbody2D rb2d;

    //Calculate path
    public Path path;

    //AI Speed per sec
    public float speed = 300f;
    public ForceMode2D fMode;

    [HideInInspector]
    public bool pathIsEnded = false;

    //Max distance from the Ai to a waypoint before it continues to the next waypoint
    public float nextWaypointDistance = 3f;

    //Waypoint we are currently moving towards
    private int currentWaypoint = 0;


    // Use this for initialization
    void Start () {
        seeker = GetComponent<Seeker>();
        rb2d = GetComponent<Rigidbody2D>();

        if(target == null) {
            //TODO: Insert a player search here
            Debug.LogError("Target Missing");
            return;
        }

        //Start a new path to the target position & return result to OnPathComplete method
        seeker.StartPath(transform.position,target.position, OnPathComplete);

	}

    public IEnumerator UpdatePath() {
        if (target == null) {
            //TODO: Insert a player search here
            yield return false;
        }

        //Start a new path to the target position & return result to OnPathComplete method
        seeker.StartPath(transform.position, target.position, OnPathComplete);

        yield return new WaitForSeconds(1f / updateRate);
        StartCoroutine("UpdatePath");
    }

    public void OnPathComplete(Path p) {
        Debug.Log("We got a path. Did it have an error?: " + p.error);

        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate () {
        if (target == null) {
            //TODO: Insert a player search here
            return;
        }

        //TODO: Always look at player

        if(path == null) {
            return;
        }

        if(currentWaypoint >= path.vectorPath.Count) {
            if (pathIsEnded) {
                return;
            }

            Debug.Log("End of path reached");
            pathIsEnded = true;
            return;
        }
        pathIsEnded = false;

        //direction to next waypoint
        Vector3 dir = path.vectorPath[currentWaypoint] - transform.position;
        dir *= speed * Time.fixedDeltaTime;

        //Move enemy
        rb2d.AddForce(dir, fMode);

        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (dist < nextWaypointDistance) {
            currentWaypoint++;
            return;
        }

    }

    protected override void OnTriggerEnter2D(Collider2D collider) {
        
    }
}
