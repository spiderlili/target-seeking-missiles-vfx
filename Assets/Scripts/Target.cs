using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject targetPosition;
    float rotSpeed = 0.8f;
    float speed = 8f;
    public Vector3 targetPositionVector;
    public bool stopFollowTargetPostLaunch = Drive.stopFollowTargetPostLaunch;

    private void Start()
    {
        if (targetPosition == null)
        {
            targetPosition = GameObject.FindGameObjectWithTag("Target");
        }
    }

    private void LateUpdate()
    {
        if (stopFollowTargetPostLaunch == false)
        {
            this.transform.LookAt(targetPosition.transform.position); //turn the bullet towards the target position
        }
        else
        {
            this.transform.LookAt(targetPositionVector); //LookAt just wants a vector3
        }

        this.transform.Translate(0, 0, speed * Time.deltaTime); //push the bullet forward along its z axis
    }
}