using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInstance : MonoBehaviour
{
    public float timeBeforeDestroy = 0.8f; //how long before this object is destroyed

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyObject", timeBeforeDestroy);
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}