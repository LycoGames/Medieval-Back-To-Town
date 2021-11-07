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

    GameObject instigator = null;
    private void Start()
    {
        Destroy(gameObject, maxLifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
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

        Destroy(gameObject, lifeAfterImpact);

    }
}
