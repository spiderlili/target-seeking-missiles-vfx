using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInstance : MonoBehaviour
{
    public float deadTime;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyObject", deadTime);
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}