﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drive : MonoBehaviour
{

    public float speed = 10.0f; //forward speed
    public float rotationSpeed = 100.0f;
    public float turretSpeed = 0.2f;
    public GameObject turret;
    public GameObject shell;
    public GameObject shellSpawner;
    public GameObject target;

    void Update()
    {
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;
        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);

        if (Input.GetKey(KeyCode.R)) //pitch the turret back & forward
        {
            turret.transform.Rotate(-turretSpeed, 0, 0);
        }
        else if (Input.GetKey(KeyCode.F))
        {
            turret.transform.Rotate(turretSpeed, 0, 0);
        }

        //Determine the location of where the bullets should go through: based on raycasting from mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit; //project the ray into the environment, receive back any info about something that was hit
        if (Physics.Raycast(ray.origin, ray.direction, out hit)) //if something is hit
        {
            target.transform.position = hit.point;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            GameObject bullet = Instantiate(shell, shellSpawner.transform.position,
                shellSpawner.transform.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * 1000);
        }

    }
}