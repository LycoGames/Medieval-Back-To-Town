using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dashing : MonoBehaviour
{
    public float distance, speed, destinationMultiplier;
    public Transform cam;
    public LayerMask layerMask;
    public Transform arissaHead;
    bool blinking = false;
    Vector3 destination;
    ParticleSystem trail;
    // Start is called before the first frame update

    void Start()
    {
        trail = transform.Find("Trail").GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Blink();
        }
        if (arissaHead) { Debug.DrawRay(arissaHead.position, arissaHead.forward, Color.green); }
        Debug.Log("dest: "+destination);
        if (blinking)
        {
            var dist = Vector3.Distance(transform.position, destination);
            if (dist > 0.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);
            }
            else { blinking = false; }
        }
    }

    void Blink()
    {
        trail.Play();
        RaycastHit hit;

        if (Physics.Raycast(arissaHead.position, arissaHead.forward, out hit, distance, layerMask))
        {
            destination = hit.point * destinationMultiplier;
        }

        else
        {
            destination = (arissaHead.position + arissaHead.forward.normalized * distance) * destinationMultiplier;
        }
        blinking = true;
    }

    /*void Blink()
    {
        trail.Play();
        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, distance, layerMask))
        {
           destination = hit.point * destinationMultiplier;
        }

        else
        {
            destination = (cam.position + cam.forward.normalized * distance) * destinationMultiplier;
        }

        blinking = true;
    }*/

}
