using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject targetPosition;
    float rotSpeed = 0.8f;
    float speed = 8f;

    private void LateUpdate()
    {
        this.transform.LookAt(targetPosition.transform.position); //turn the bullet towards the target position
        this.transform.Translate(0, 0, speed * Time.deltaTime); //push the bullet forward along its z axis
    }
}