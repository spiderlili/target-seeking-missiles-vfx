using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject targetPosition;

    //need to increase rotationSpeed if increasing speed and vice versa - if moving faster in a straight line you'll also need faster rotation speed to get around the corner
    [Range(0.0f, 50.0f)] public float rotationSpeed = 50.0f; //can't be too small: distance gives a scalar value for rotation speed
    float originalRotationSpeed = 50.0f; //use this to update rotationSpeed without losing the original value
    public float speed = 10f;

    public Vector3 targetPositionVector;
    public bool stopFollowTargetPostLaunch = Drive.stopFollowTargetPostLaunch;
    public GameObject explosionVFX;

    Vector3 waypointLocation;
    bool isWaypointVisited = false; //don't want to continue to the goal location if haven't been to the waypoint
    float randomWaypointHeight = 10f;
    float randomWaypointInterpolator = 0.8f;

    public bool useMultipleWaypoints = true;
    int currentWaypoint = 0;
    Vector3[] waypoints;

    private void Start()
    {
        randomWaypointHeight = Random.Range(4f, 6f);
        randomWaypointInterpolator = Random.Range(0.5f, 0.7f);

        if (useMultipleWaypoints == true)
        {
            waypoints = new Vector3[3];
            //the longer the path, the further away the waypoints will be apart.
            waypoints[0] = Vector3.Lerp(this.transform.position, targetPositionVector, 0.4f);
            waypoints[0] += new Vector3(0, waypoints[currentWaypoint].y * randomWaypointHeight, 0); //lift bullet up
            waypoints[1] = Vector3.Lerp(this.transform.position, targetPositionVector, randomWaypointInterpolator);
            waypoints[1] += new Vector3(0, waypoints[currentWaypoint].y / 1.1f, 0); //lower bullet
            waypoints[2] = Vector3.Lerp(this.transform.position, targetPositionVector, 0.8f);
            waypoints[2] += new Vector3(Random.Range(-5, 5), 0, 0); //randomise x value of the bullet to keep things interesting
        }
        else //only use 1 waypoint
        {
            //intelligent tank: calculate appropriate rotation speed based on speed - the further it's away from the target, the less rotation speed it has
            rotationSpeed = 1 / Vector3.Distance(this.transform.position, targetPositionVector) * rotationSpeed;
            //create a waypoint positined halfway along the 2 vectors, increase interpolator for the waypoint to be closer to target
            waypointLocation = Vector3.Lerp(this.transform.position, targetPositionVector, 0.8f);
            waypointLocation += new Vector3(0, waypointLocation.y * randomWaypointHeight, 0); //lift waypoint up in the air
        }

        if (targetPosition == null)
        {
            targetPosition = GameObject.FindGameObjectWithTag("Target");
        }
    }

    //bullet collider is a trigger
    private void OnTriggerEnter(Collider other)
    {
        GameObject explosion = Instantiate(explosionVFX, this.transform.position, Quaternion.identity);
        Destroy(explosion, 2); //delete explosion object 2 secs after explosion
        Destroy(this.gameObject); //delete  bullet
    }

    private void LateUpdate()
    {
        //randomWaypointHeight = Random.Range(10f, 100f);
        //randomWaypointInterpolator = Random.Range(0.5f, 0.8f);

        if (stopFollowTargetPostLaunch == false)
        {
            this.transform.LookAt(targetPosition.transform.position); //turn the bullet towards the target position, even if the target position moves after launch
        }
        else
        {
            //travels to the goal with an arc
            //this.transform.LookAt(targetPositionVector); //LookAt just wants a vector3 - only follow the target's initial transform position & do not follow if it moves after launch
            //Vector3 lookAtGoal = new Vector3(targetPositionVector.x, targetPositionVector.y, targetPositionVector.z);
            //Vector3 directionToGoal = lookAtGoal - this.transform.position;
            //this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(directionToGoal), Time.deltaTime * rotationSpeed);
            //this.transform.Translate(0, 0, speed * Time.deltaTime);
            //this.transform.Translate(0, 0, speed * Time.deltaTime); //push the bullet forward along its z axis
            //travels to the goal with a middle trip to a randomised waypoint and then to the goal
            Vector3 lookAtGoal;
            Vector3 directionToGoal;

            if (useMultipleWaypoints == true)
            {
                if (currentWaypoint >= waypoints.Length - 1) //lookat the target position
                {
                    lookAtGoal = new Vector3(targetPositionVector.x, targetPositionVector.y, targetPositionVector.z);
                }
                else //lookat the waypoints
                {
                    lookAtGoal = new Vector3(waypoints[currentWaypoint].x, waypoints[currentWaypoint].y, waypoints[currentWaypoint].z);
                    if (Vector3.Distance(this.transform.position, waypoints[currentWaypoint]) < 2f) //reached the waypoint, go to the next one
                    {
                        currentWaypoint++;
                    }
                }
            }
            else //only use 1 waypoint
            {

                if (Vector3.Distance(this.transform.position, waypointLocation) < 1)
                {
                    isWaypointVisited = true;
                }

                if (isWaypointVisited = true)
                {
                    lookAtGoal = new Vector3(targetPositionVector.x, targetPositionVector.y, targetPositionVector.z);
                    //change the rotation inside the update each time: previously the rotation was just set in the start, now change it each time
                    //rotationSpeed = 1 / Vector3.Distance(this.transform.position, targetPositionVector) * originalRotationSpeed; //recalculate rotation speed for sharp turns
                }
                else
                {
                    lookAtGoal = new Vector3(waypointLocation.x, waypointLocation.y, waypointLocation.z);
                    //rotationSpeed = 1 / Vector3.Distance(this.transform.position, waypointLocation) * originalRotationSpeed; //recalculate rotation speed for sharp turns
                }
            }

            directionToGoal = lookAtGoal - this.transform.position;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(directionToGoal), Time.deltaTime * rotationSpeed);
            this.transform.Translate(0, 0, speed * Time.deltaTime);
        }
    }
}