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
    [SerializeField] GameObject hitEffect = null;
    [SerializeField] GameObject[] destroyOnHit = null;
    [SerializeField] UnityEvent onHit;
    [SerializeField] string targetTag = "Enemy";

    GameObject instigator = null;
    float damage = 0;

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
        speed = 0;
        onHit.Invoke();
    }
}
