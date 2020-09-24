using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drive : MonoBehaviour {

    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;
    public float turretSpeed = 0.2f;
    public GameObject turret;
    public GameObject shell;
    public GameObject shellSpawner;

    void Update()
    {
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;
        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);

        if(Input.GetKey(KeyCode.R))
        {
            turret.transform.Rotate(-turretSpeed, 0, 0);
        }
        else if(Input.GetKey(KeyCode.F))
        {
            turret.transform.Rotate(turretSpeed, 0, 0);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject bullet = Instantiate(shell, shellSpawner.transform.position, 
                                            shellSpawner.transform.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * 1000);
        }
            
    }
}
