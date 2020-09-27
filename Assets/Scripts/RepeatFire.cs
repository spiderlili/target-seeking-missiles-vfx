using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatFire : MonoBehaviour
{
    public GameObject fireObject;
    public float timeInterval;
    private float timeIncrement;
    private bool isFiring = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isFiring = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isFiring = false;
        }

        if (isFiring)
        {
            timeIncrement += Time.deltaTime;
            if (timeIncrement >= timeInterval)
            {
                Instantiate(fireObject, transform.position, transform.rotation);
                timeInterval = 0f;
            }
        }
    }
}