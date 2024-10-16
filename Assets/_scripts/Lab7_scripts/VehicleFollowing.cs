using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleFollowing : MonoBehaviour
{
    public Path path;
    public float speed = 10.0f;
    [Range(1.0f, 1000.0f)]
    public float steeringInertia = 100.0f;
    public float waypointRadius = 1.0f;

    //Actual speed of the vehicle
    private float curSpeed;
    private int curPathIndex = 0;
    private int pathLength;
    private Vector3 targetPoint;
    private Vector3 velocity;

    private bool movingForward = true; // Track direction of movement

    void Start()
    {
        pathLength = path.Length;
        velocity = transform.forward;
    }

    void Update()
    {
        // Unify the speed
        curSpeed = speed * Time.deltaTime;

        // Get the current target waypoint
        targetPoint = path.GetPoint(curPathIndex);

        // If the vehicle reaches the waypoint, move to the next one
        if (Vector3.Distance(transform.position, targetPoint) < waypointRadius)
        {
            if (movingForward)
            {
                // Move to the next waypoint
                if (curPathIndex < pathLength - 1)
                {
                    curPathIndex++;
                }
                else
                {
                    // If reached the last waypoint, start moving backward
                    movingForward = false;
                    curPathIndex--; // Move to the previous waypoint
                }
            }
            else
            {
                // Move to the previous waypoint
                if (curPathIndex > 0)
                {
                    curPathIndex--;
                }
                else
                {
                    // If reached the first waypoint, start moving forward
                    movingForward = true;
                    curPathIndex++; // Move to the next waypoint
                }
            }
        }

        // Calculate the next velocity towards the path
        velocity += Steer(targetPoint);

        // Move the vehicle according to the velocity
        transform.position += velocity;

        // Rotate the vehicle towards the desired velocity
        transform.rotation = Quaternion.LookRotation(velocity);
    }

    public Vector3 Steer(Vector3 target)
    {
        // Calculate the directional vector from the current position towards the target point
        Vector3 desiredVelocity = (target - transform.position);
        float dist = desiredVelocity.magnitude;

        // Normalize the desired velocity
        desiredVelocity.Normalize();
        desiredVelocity *= curSpeed;

        // Calculate the force vector
        Vector3 steeringForce = desiredVelocity - velocity;
        return steeringForce / steeringInertia;
    }
}
