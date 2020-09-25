using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject targetPosition;

    //need to increase rotationSpeed if increasing speed and vice versa - if moving faster in a straight line you'll also need faster rotation speed to get around the corner
    [Range(0.0f, 50.0f)] public float rotationSpeed = 50.0f; //can't be too small: distance gives a scalar value for rotation speed
    public float speed = 10f;
    public Vector3 targetPositionVector;
    public bool stopFollowTargetPostLaunch = Drive.stopFollowTargetPostLaunch;
    public GameObject explosionVFX;

    private void Start()
    {
        //intelligent tank: calculate appropriate rotation speed based on speed - the further it's away from the target, the less rotation speed it has
        rotationSpeed = 1 / Vector3.Distance(this.transform.position, targetPositionVector) * rotationSpeed;

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
        if (stopFollowTargetPostLaunch == false)
        {
            this.transform.LookAt(targetPosition.transform.position); //turn the bullet towards the target position, even if the target position moves after launch
        }
        else
        {
            //this.transform.LookAt(targetPositionVector); //LookAt just wants a vector3 - only follow the target's initial transform position & do not follow if it moves after launch
            Vector3 lookAtGoal = new Vector3(targetPositionVector.x, targetPositionVector.y, targetPositionVector.z);
            Vector3 directionToGoal = lookAtGoal - this.transform.position;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(directionToGoal), Time.deltaTime * rotationSpeed);
            this.transform.Translate(0, 0, speed * Time.deltaTime);
        }

        this.transform.Translate(0, 0, speed * Time.deltaTime); //push the bullet forward along its z axis
    }
}