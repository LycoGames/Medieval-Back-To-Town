using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarRotater : MonoBehaviour
{
    Transform cam;


    void Start()
    {
        cam = Camera.main.transform;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
