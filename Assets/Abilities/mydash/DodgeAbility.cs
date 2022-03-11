using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DodgeAbility : Abilities
{
    Rigidbody myRigidBody;

    bool isDashing = false;

    public Transform cam;
    public LayerMask layerMask;
    Vector3 destination;
    public float distance, speed, destinationMultiplier;

    void Start()
    {
        cam = Camera.main.transform;
        Debug.Log("cam " + cam);
        //trail = parent.gameObject.GetComponentInChildren<ParticleSystem>();
        //trail = parent.transform.GetComponentInChildren<ParticleSystem>();
    }

    public override void Active(GameObject parent)
    {
        Start();
        parent.GetComponent<Controller>().StartCoroutine(HandleDash(parent));
        parent.GetComponent<Controller>().PlayTrail();
        isDashing = true;
        // myRigidBody = parent.GetComponent<Rigidbody>();
        // myRigidBody.AddForce(parent.transform.forward * dodgeMultiplier, ForceMode.Impulse);
        // isDashing = false;
    }

    private IEnumerator HandleDash(GameObject player)
    {
        Dash();
        while (true)
        {
            if (isDashing)
            {
                var dist = Vector3.Distance(player.transform.position, destination);
                if (dist > 0.5f)
                {
                    player.transform.position = Vector3.MoveTowards(player.transform.position, destination, Time.deltaTime * speed);
                }
                else { isDashing = false; }
            }
            yield return null;
        }
    }

    void Dash()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, distance, layerMask))
        {
            destination = hit.point * destinationMultiplier;
        }

        else
        {
            destination = (cam.position + cam.forward.normalized * distance) * destinationMultiplier;
        }

        isDashing = true;
    }

}
