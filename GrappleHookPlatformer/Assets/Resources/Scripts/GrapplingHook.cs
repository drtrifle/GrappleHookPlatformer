using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GrapplingHook : MonoBehaviour {

    private LineRenderer line;
    private SpringJoint2D joint;
    Vector3 targetPos;
    RaycastHit2D hit;
    public float distance = 10f;
    public LayerMask mask;
    public float step = 0.02f;

    private bool isHooked = false;

    // Use this for initialization
    void Start() {
        joint = GetComponent<SpringJoint2D>();
        line = GetComponent<LineRenderer>();

        Assert.IsNotNull(joint, "DistanceJoint is null");
        Assert.IsNotNull(line, "LineRenderer is null");

        joint.enabled = false;
        line.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate() {

        //Break rope when distance too small
        if (joint.distance <= .5f) {
            DisableHook();
        }

        //Update LineRenderer
        if (isHooked) {
            line.SetPosition(0, transform.position);
        }

        //Fire GrappleHook
        if (Input.GetButtonDown("Fire1")) {
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = 0;

            hit = Physics2D.Raycast(transform.position, targetPos - transform.position, distance, mask);

            if (hit.collider != null && hit.collider.gameObject.GetComponent<Rigidbody2D>() != null) {
                joint.enabled = true;
                Vector2 connectPoint = hit.point - new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.y);
                connectPoint.x = connectPoint.x / hit.collider.transform.localScale.x;
                connectPoint.y = connectPoint.y / hit.collider.transform.localScale.y;
                Debug.Log(connectPoint);
                joint.connectedAnchor = connectPoint;

                joint.connectedBody = hit.collider.gameObject.GetComponent<Rigidbody2D>();
                joint.distance = Vector2.Distance(transform.position, hit.point);

                //LineRenderer
                line.enabled = true;
                line.SetPosition(0, transform.position);
                line.SetPosition(1, hit.point);
                isHooked = true;
            }else {
                DisableHook();
            }
        }

        //Allow Player to move up/down rope when hooked
        joint.distance = joint.distance - (step * Input.GetAxis("Vertical"));

    }

    private void DisableHook() {
        line.enabled = false;
        joint.enabled = false;
        isHooked = false;
    }
}
