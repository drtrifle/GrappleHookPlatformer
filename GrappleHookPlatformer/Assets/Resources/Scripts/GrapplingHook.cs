using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GrapplingHook : MonoBehaviour {

    public GameObject hookPrefab;
    private GameObject hookObject;

    private LineRenderer line2D;
    private DistanceJoint2D joint2D;
    RaycastHit2D hit;
    public float distance = 10f;
    public LayerMask mask;
    public float step = 0.02f;

    private bool isHooked = false;

    public PlatformerCharacter2D platformerCharacterScript;

    // Use this for initialization
    void Start() {
        joint2D = GetComponent<DistanceJoint2D>();
        line2D = GetComponent<LineRenderer>();

        Assert.IsNotNull(joint2D, "DistanceJoint is null");
        Assert.IsNotNull(line2D, "LineRenderer is null");

        joint2D.enabled = false;
        line2D.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate() {

        //Break rope when distance too small
        if (joint2D.distance <= .5f) {
            DisableHook();
        }

        //Update LineRenderer
        if (isHooked) {
            line2D.SetPosition(0, transform.position);
            line2D.SetPosition(1, hookObject.transform.position);
        }

        //Fire GrappleHook
        if (Input.GetButtonDown("Fire1")) {
            ShootGrapple();
        } else if (Input.GetButtonDown("Fire2")) {
            DisableHook();
        }

        //Allow Player to move up/down rope when hooked
        joint2D.distance = joint2D.distance - (step * Input.GetAxis("Vertical"));

    }

    private void ShootGrapple() {
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPos.z = 0;

        hit = Physics2D.Raycast(transform.position, targetPos - transform.position, distance, mask);

        if (hit.collider != null && hit.collider.gameObject.GetComponent<Rigidbody2D>() != null) {
            joint2D.enabled = true;
            Vector2 connectPoint = hit.point - new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.y);
            connectPoint.x = connectPoint.x / hit.collider.transform.localScale.x;
            connectPoint.y = connectPoint.y / hit.collider.transform.localScale.y;
            Debug.Log(connectPoint);

            //Destroy hook if already exists
            if (hookObject != null) {
                Destroy(hookObject);
            }

            hookObject = Instantiate(hookPrefab, new Vector3(hit.point.x, hit.point.y, 0), Quaternion.identity, hit.transform);
            joint2D.connectedAnchor = connectPoint;

            joint2D.connectedBody = hit.collider.gameObject.GetComponent<Rigidbody2D>();
            joint2D.distance = Vector2.Distance(transform.position, hit.point);

            //LineRenderer
            line2D.enabled = true;
            line2D.SetPosition(0, transform.position);
            line2D.SetPosition(1, hookObject.transform.position);
            isHooked = true;

            //Affect Swinging
            platformerCharacterScript.SetSwingState(true);
            platformerCharacterScript.ropeHook = hit.point;

        } else {
            DisableHook();
        }
    }

    private void DisableHook() {
        line2D.enabled = false;
        joint2D.enabled = false;
        isHooked = false;

        //Destroy hook if alreay exists
        if (hookObject != null) {
            Destroy(hookObject);
        }

        platformerCharacterScript.SetSwingState(false);

    }
}
