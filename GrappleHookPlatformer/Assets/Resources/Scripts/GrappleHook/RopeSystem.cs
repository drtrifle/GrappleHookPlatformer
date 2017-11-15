using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RopeSystem : MonoBehaviour {

    public DistanceJoint2D ropeJoint;
    public Transform crosshair;
    public SpriteRenderer crosshairSprite;
    public PlayerMovement playerMovement;
    private bool ropeAttached;
    private Vector2 playerPosition;                                    // Stores player's world coords

    // RopeHingeAnchor Vars
    public GameObject ropeHingeAnchor;
    private Rigidbody2D ropeHingeAnchorRb;
    private SpriteRenderer ropeHingeAnchorSprite;

    private Rigidbody2D grappleObjectRb;

    // RayCast vars 
    public LineRenderer ropeRenderer;
    public LayerMask ropeLayerMask;
    private float ropeMaxCastDistance = 20f;
    private List<Vector2> ropePositions = new List<Vector2>();         // Contains all rope positions for wrapping around objects

    private bool distanceSet;                                          // Flag to let the script know that the rope's distance has been set correctly

    private Dictionary<Vector2, int> wrapPointsLookup = new Dictionary<Vector2, int>();   // Stores all points of polygon collider that hook is curently attached to

    // Rappeling Vars
    public float climbSpeed = 3f;
    private bool isColliding;

    //Vars to brach behaviour for different type of objects hit by hook
    private string currentlyAttachedTag;

    void Awake() {
        ropeJoint.enabled = false;
        playerPosition = transform.position;
        ropeHingeAnchorRb = ropeHingeAnchor.GetComponent<Rigidbody2D>();
        ropeHingeAnchorSprite = ropeHingeAnchor.GetComponent<SpriteRenderer>();
    }

    void Update() {

        float aimAngle = CalculateAimAngle();

        // Convert angle from player to cursor to degrees. This will be used later for Raycasting from plyer to cursor
        Vector2 aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;

        //keep tracking player world coords
        playerPosition = transform.position;

        //Determine if the rope is attached to an anchor point
        if (ropeAttached) {
            crosshairSprite.enabled = false;

            switch (currentlyAttachedTag) {
                case ("Terrain"):
                    playerMovement.isSwinging = true;
                    playerMovement.ropeHook = ropePositions.Last();
                    HandleRopeWrapping();
                    break;
                case ("GrappleObject"):
                    playerMovement.isSwinging = true;
                    playerMovement.ropeHook = grappleObjectRb.transform.position;
                    break;
            }
            
        } else {
            playerMovement.isSwinging = false;

            SetCrosshairPosition(aimAngle);
        }

        UpdateRopePositions();
        HandlePlayerInput(aimDirection);
        HandleRopeLength();
    }

    #region Rope Wrapping Methods

    //Wraps Rope around edges of polygoncolliders that touch the rope
    private void HandleRopeWrapping() {
        //Check if ropePositions list has any positions stored
        if (ropePositions.Count > 0) {
            // Fire a raycast out from the player's position, in the direction of the player looking at the last rope position in the list 
            var lastRopePoint = ropePositions.Last();
            var playerToCurrentNextHit = Physics2D.Raycast(playerPosition, (lastRopePoint - playerPosition).normalized, Vector2.Distance(playerPosition, lastRopePoint) - 0.1f, ropeLayerMask);

            // If the raycast hits something, then that hit object's collider is safe cast to a PolygonCollider2D. 
            // As long as it's a real PolygonCollider2D, then the closest vertex position on that collider is returned as a Vector2, 
            if (playerToCurrentNextHit) {
                var colliderWithVertices = playerToCurrentNextHit.collider as PolygonCollider2D;
                if (colliderWithVertices != null) {
                    var closestPointToHit = GetClosestColliderPointFromRaycastHit(playerToCurrentNextHit, colliderWithVertices);

                    // The wrapPointsLookup is checked to make sure the same position is not being wrapped again.
                    // If it is, then it'll reset the rope and cut it, dropping the player.
                    if (wrapPointsLookup.ContainsKey(closestPointToHit)) {
                        ResetRope();
                        return;
                    }

                    // The wrapPointsLookup dictionar and ropePositions list is now updated, adding the position the rope should wrap around
                    // DistanceSet flag is disabled, so that UpdateRopePositions() method can re-configure the rope's distances to take into account the new rope length and segments.
                    ropePositions.Add(closestPointToHit);
                    wrapPointsLookup.Add(closestPointToHit, 0);
                    distanceSet = false;
                }
            }
        }
    }

    //Update the LineRenderer for Rope Effect
    private void UpdateRopePositions() {
        //Ignore if no rope
        if (!ropeAttached) {
            return;
        }

        switch (currentlyAttachedTag) {
            case ("Terrain"):
                //Count number of vertexs(including player)
                ropeRenderer.positionCount = ropePositions.Count + 1;

                //Set LineRenderer positions to be same as list of rope positions
                for (var i = ropeRenderer.positionCount - 1; i >= 0; i--) {
                    if (i != ropeRenderer.positionCount - 1) // if not the Last point of line renderer
                    {
                        ropeRenderer.SetPosition(i, ropePositions[i]);

                        //Set the rope anchor to the second-to-last rope position where the current hinge/anchor should be
                        if (i == ropePositions.Count - 1 || ropePositions.Count == 1) {
                            var ropePosition = ropePositions[ropePositions.Count - 1];
                            if (ropePositions.Count == 1) {
                                ropeHingeAnchorRb.transform.position = ropePosition;
                                if (!distanceSet) {
                                    ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
                                    distanceSet = true;
                                }
                            } else {
                                ropeHingeAnchorRb.transform.position = ropePosition;
                                if (!distanceSet) {
                                    ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
                                    distanceSet = true;
                                }
                            }
                        }
                        //Rope position being looped over is the second-to-last one
                        else if (i - 1 == ropePositions.IndexOf(ropePositions.Last())) {
                            var ropePosition = ropePositions.Last();
                            ropeHingeAnchorRb.transform.position = ropePosition;
                            if (!distanceSet) {
                                ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
                                distanceSet = true;
                            }
                        }
                    } else {
                        //Set the rope's last vertex position to the player's current position.
                        ropeRenderer.SetPosition(i, transform.position);
                    }
                }
                break;
            case ("GrappleObject"):
                ropeRenderer.SetPosition(0, transform.position);
                ropeRenderer.SetPosition(1, grappleObjectRb.transform.position);
                break;
        }

        
    }

    //WARNING: Requires all objects to be hooked to have a polygon collider
    private Vector2 GetClosestColliderPointFromRaycastHit(RaycastHit2D hit, PolygonCollider2D polyCollider) {
        //Converts the polygon collider's collection of points, into a dictionary of Vector2 positions 
        //The key of each entry, is set to the distance that this point is to the player's position 
        var distanceDictionary = polyCollider.points.ToDictionary<Vector2, float, Vector2>(
            position => Vector2.Distance(hit.point, polyCollider.transform.TransformPoint(position)),
            position => polyCollider.transform.TransformPoint(position));

        //Dictionary ordered by distance closest to the player's current position, and the closest one is returned
        var orderedDictionary = distanceDictionary.OrderBy(e => e.Key);
        return orderedDictionary.Any() ? orderedDictionary.First().Value : Vector2.zero;
    }

    # endregion

    //Returns the angle from the player to the cursor as a radian
    private float CalculateAimAngle() {
        var worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        var facingDirection = worldMousePosition - transform.position;
        var aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
        if (aimAngle < 0f) {
            aimAngle = Mathf.PI * 2 + aimAngle;
        }
        return aimAngle;
    }

    //Set the crosshair sprite 1.5 units in between player & cursor
    private void SetCrosshairPosition(float aimAngle) {
        if (!crosshairSprite.enabled) {
            crosshairSprite.enabled = true;
        }

        var x = transform.position.x + 1.5f * Mathf.Cos(aimAngle);
        var y = transform.position.y + 1.5f * Mathf.Sin(aimAngle);

        var crossHairPosition = new Vector3(x, y, 0);
        crosshair.transform.position = crossHairPosition;
    }

    //Handle Input from player playing game
    private void HandlePlayerInput(Vector2 aimDirection) {
        //On Player Left Click
        if (Input.GetMouseButton(0)) {
            ShootGrapple(aimDirection);
        }

        //On Player Right Click
        if (Input.GetMouseButton(1)) {
            ResetRope();
        }
    }

    //Fire Grapple Hook at player mouse cursor direction
    private void ShootGrapple(Vector2 aimDirection) {
        // Ignore if rope already attached
        if (ropeAttached) return;
        ropeRenderer.enabled = true;

        RaycastHit2D hit = Physics2D.Raycast(playerPosition, aimDirection, ropeMaxCastDistance, ropeLayerMask);
        
        // Raycast hit something
        if (hit.collider != null) {

            currentlyAttachedTag = hit.collider.tag;

            switch (currentlyAttachedTag) {
                case ("Terrain"):
                    ropeAttached = true;

                    //Check if raycast hit position is new
                    if (!ropePositions.Contains(hit.point)) {
                        // Jump slightly to distance the player a little from the ground after grappling to something.
                        transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 2f), ForceMode2D.Impulse);
                        ropePositions.Add(hit.point);
                        ropeJoint.connectedBody = ropeHingeAnchorRb;
                        ropeJoint.distance = Vector2.Distance(playerPosition, hit.point);
                        ropeJoint.enabled = true;
                        ropeHingeAnchorSprite.enabled = true;
                    }
                    break;
                case ("GrappleObject"):
                    ropeAttached = true;

                    ropeHingeAnchor.transform.position = hit.point;
                    ropeHingeAnchor.transform.parent = hit.collider.transform;
                    grappleObjectRb = hit.collider.GetComponent<Rigidbody2D>();
                    ropeJoint.connectedBody = grappleObjectRb;
                    ropeJoint.distance = Vector2.Distance(playerPosition, hit.collider.transform.position);
                    ropeJoint.enabled = true;
                    break;
            }
            
        }
        // Disable Rope Vars & Visuals
        else {
            ropeRenderer.enabled = false;
            ropeAttached = false;
            ropeJoint.enabled = false;
        }
    }

    //Reset Rope Vars & Visuals
    private void ResetRope() {
        ropeJoint.enabled = false;
        ropeAttached = false;
        playerMovement.isSwinging = false;
        ropeRenderer.positionCount = 2;
        ropeRenderer.SetPosition(0, transform.position);
        ropeRenderer.SetPosition(1, transform.position);
        ropePositions.Clear();
        ropeHingeAnchorSprite.enabled = false;
        wrapPointsLookup.Clear();
        currentlyAttachedTag = "None";
    }

    //Shortens/Lengthens Rope Length depending on player input
    private void HandleRopeLength() {
        if (Input.GetAxis("Vertical") >= 1f && ropeAttached && !isColliding) {
            ropeJoint.distance -= Time.deltaTime * climbSpeed;
        } else if (Input.GetAxis("Vertical") < 0f && ropeAttached) {
            ropeJoint.distance += Time.deltaTime * climbSpeed;
        }
    }

    #region Collision Checking Methods

    void OnTriggerStay2D(Collider2D colliderStay) {
        isColliding = true;
    }

    private void OnTriggerExit2D(Collider2D colliderOnExit) {
        isColliding = false;
    }

    #endregion


}
