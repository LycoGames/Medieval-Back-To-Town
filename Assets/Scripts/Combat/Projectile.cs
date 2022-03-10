using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 100;
    [SerializeField] float maxLifeTime = 10f;
    [SerializeField] float lifeAfterImpact = 2f;
    [SerializeField] string targetTag = "Enemy";
    [SerializeField] float damage = 20;
    [SerializeField] GameObject hitEffect = null;
    [SerializeField] GameObject[] destroyOnHit = null;
    [SerializeField] UnityEvent onHit;
    [SerializeField] Rigidbody myRigidBody;

    GameObject instigator = null;

    /*
        private float torque = 5f;
        Archer archer;

      

        void Start()
        {
            archer = GetComponent<Archer>();
        }
        void Update()
        {
            var theForce = archer.getForce();
            print("" + theForce);
        }

        

        public void ShootArrow(Vector3 force)
    {
        myRigidBody.isKinematic = false;
        myRigidBody.AddForce(force, ForceMode.Impulse);
        myRigidBody.AddTorque(transform.right * torque);
        transform.SetParent(null);
    }

    public void shootArrow()
    {
        var theForce = archer.getForce();
        ShootArrow(theForce);
    }
    */

    void OnEnable()
    {
        StartCoroutine(SetTheObjectFalse());
    }

    private IEnumerator SetTheObjectFalse()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }

    public void AddForce()
    {
        myRigidBody.AddForce(myRigidBody.transform.forward * (speed * UnityEngine.Random.Range(2.5f, 2.8f)));
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision:" + collision.gameObject.name);
        /* if (collision.gameObject.tag != "Player")
         {
             myRigidBody.isKinematic = true;
             gameObject.SetActive(false);
             // Destroy(gameObject, lifeAfterImpact);
         }
         */
        Health target = collision.gameObject.GetComponent<Health>();
        if (target == null || target.tag != targetTag) return;
        if (target.IsDead()) return;

        if (hitEffect != null)
        {
            Instantiate(hitEffect, target.transform.position, transform.rotation);
        }

        foreach (GameObject toDestroy in destroyOnHit)
        {
            //Destroy(toDestroy);
        }

        target.ApplyDamage(instigator,damage);
        Debug.Log(target + " " + damage + "damage atıldı");
        Debug.Log(target.GetHealthPoints() + "canı kaldı");
        gameObject.SetActive(false);

    }

    /*private void OnTriggerEnter(Collider other)
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
        Destroy(gameObject);

    }*/
}
