using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 1;
    [SerializeField] float maxLifeTime = 10f;
    [SerializeField] float lifeAfterImpact = 2f;
    [SerializeField] string targetTag = "Enemy";
    [SerializeField] float damage = 20;
    [SerializeField] GameObject hitEffect = null;
    [SerializeField] GameObject[] destroyOnHit = null;
    [SerializeField] UnityEvent onHit;
    [SerializeField] Rigidbody myRigidBody;


    GameObject instigator = null;
    private void Start()
    {
        Destroy(gameObject, maxLifeTime);
    }

    private void Update()
    {
        myRigidBody.AddForce(myRigidBody.transform.forward * (speed * UnityEngine.Random.Range(1.8f, 2.9f)));

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "Enemy")
        {
            myRigidBody.isKinematic = true;
            Destroy(gameObject, lifeAfterImpact);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //speed = 0;
        onHit.Invoke();
        Health target = other.GetComponent<Health>();
        if (target == null || target.tag != targetTag) return;
        if (target.IsDead()) return;

        if (hitEffect != null)
        {
            Instantiate(hitEffect, target.transform.position, transform.rotation);
        }

         foreach (GameObject toDestroy in destroyOnHit)
         {
             Destroy(toDestroy);
         }

        target.ApplyDamage(damage);
        Debug.Log(target + " " + damage + "damage atıldı");
        Debug.Log(target.GetHealthPoints() + "canı kaldı");
        Destroy(gameObject, lifeAfterImpact);

    }
}
