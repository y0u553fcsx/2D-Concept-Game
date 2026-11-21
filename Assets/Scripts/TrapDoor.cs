using System.Collections;
using UnityEngine;

// Put this on the swinging half of the trap door (RightTrap).
//
// How it works:
//  - The plank is Kinematic: solid, and nothing can push it around.
//  - When the player stands on top of it, it waits dropDelay seconds,
//    then the script itself rotates it DOWN around the hinge point.
//    (No physics hinge is used, so it can't jam or swing the wrong way.)
//  - If resetAfterDrop is on, it swings back up after resetDelay seconds.
public class TrapDoor : MonoBehaviour
{
    public float dropDelay = 0.3f;     // seconds between stepping on it and it opening
    public float openAngle = 90f;      // how far it swings down (degrees)
    public float swingSpeed = 270f;    // how fast it swings (degrees per second)
    public bool resetAfterDrop = true; // close again afterwards?
    public float resetDelay = 3f;      // seconds it stays open before closing

    private Rigidbody2D rb;
    private Vector2 hingePoint;     // world point the plank rotates around
    private Vector2 closedPosition; // world position when closed
    private float closedRotation;   // rotation when closed
    private bool triggered = false;
    private Rigidbody2D playerRb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        // Find the hinge point: use the HingeJoint2D's anchor if there is one,
        // otherwise the middle of the plank's left edge.
        HingeJoint2D hinge = GetComponent<HingeJoint2D>();
        if (hinge != null)
        {
            hingePoint = transform.TransformPoint(hinge.anchor);
            hinge.enabled = false; // the script does the swinging now
        }
        else
        {
            Bounds b = GetComponent<Collider2D>().bounds;
            hingePoint = new Vector2(b.min.x, b.center.y);
        }

        closedPosition = transform.position;
        closedRotation = transform.eulerAngles.z;
    }

    void OnCollisionStay2D(Collision2D other)
    {
        // only the player, only once, only when standing on TOP of the door,
        // and only once the player's center is actually over the door
        // (not still mostly standing on the solid half next to it)
        float doorRight = closedPosition.x + (closedPosition.x - hingePoint.x);
        float px = other.transform.position.x;

        if (!triggered && other.gameObject.tag == "Player"
            && other.transform.position.y > transform.position.y
            && px > hingePoint.x && px < doorRight)
        {
            triggered = true;
            playerRb = other.rigidbody;
            StartCoroutine(DropAndReset());
        }
    }

    IEnumerator DropAndReset()
    {
        yield return new WaitForSeconds(dropDelay);

        // swing down (negative angle = clockwise = free end goes down)
        yield return Swing(0f, -openAngle);

        if (resetAfterDrop)
        {
            yield return new WaitForSeconds(resetDelay);
            yield return Swing(-openAngle, 0f); // swing back up
            triggered = false;
        }
    }

    // Rotates the plank around the hinge point from one angle to another, step by step.
    IEnumerator Swing(float from, float to)
    {
        float angle = from;
        while (!Mathf.Approximately(angle, to))
        {
            angle = Mathf.MoveTowards(angle, to, swingSpeed * Time.fixedDeltaTime);
            SetAngle(angle);

            // A player standing still "falls asleep" in the physics engine and
            // won't notice the floor moving away. Wake them up so gravity pulls them down.
            if (playerRb != null)
            {
                playerRb.WakeUp();
            }

            yield return new WaitForFixedUpdate();
        }
    }

    void SetAngle(float angle)
    {
        // rotate the plank's closed position around the hinge by "angle" degrees
        Vector2 offset = closedPosition - hingePoint;
        Vector2 rotatedOffset = Quaternion.Euler(0, 0, angle) * offset;
        Vector2 newPosition = hingePoint + rotatedOffset;

        // move the Transform directly (the Rigidbody is Kinematic, so this is safe)
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        transform.rotation = Quaternion.Euler(0, 0, closedRotation + angle);
    }
}
