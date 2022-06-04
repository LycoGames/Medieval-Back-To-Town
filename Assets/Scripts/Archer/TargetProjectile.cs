using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TargetProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private GameObject hit;
    [SerializeField] GameObject flash;
    [SerializeField] GameObject[] Detached;
    private bool LocalRotation = false;
    private Transform target;
    private Vector3 targetOffset;

    [Space][Header("Projectile Path")] private float randomUpAngle;
    private float randomSideAngle;
    [SerializeField] private float sideAngle = 25;
    [SerializeField] private float upAngle = 20;

    private string targetTag = "Enemy";
    private float damage;
    private GameObject insitigator;

    void Start()
    {
        FlashEffect();
        newRandom();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            foreach (var detachedPrefab in Detached)
            {
                if (detachedPrefab != null)
                {
                    detachedPrefab.transform.parent = null;
                }
            }

            gameObject.SetActive(false);
            return;
        }

        Vector3 forward = ((target.position + targetOffset) - transform.position);
        Vector3 crossDirection = Vector3.Cross(forward, Vector3.up);
        Quaternion randomDeltaRotation = Quaternion.Euler(0, randomSideAngle, 0) *
                                         Quaternion.AngleAxis(randomUpAngle, crossDirection);
        Vector3 direction = randomDeltaRotation * ((target.position + targetOffset) - transform.position);

        float distanceThisFrame = Time.deltaTime * speed;

        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void newRandom()
    {
        randomUpAngle = Random.Range(0, upAngle);
        randomSideAngle = Random.Range(0, sideAngle);
    }

    public void UpdateTarget(Transform target, GameObject instigator, float damage, Vector3 Offset)
    {
        this.target = target;
        targetOffset = Offset;
        this.damage = damage;
        this.insitigator = instigator;
    }

    private void FlashEffect()
    {
        if (flash != null)
        {
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;
            var flashPs = flashInstance.GetComponent<ParticleSystem>();
            if (flashPs != null)
            {
                Destroy(flashInstance, flashPs.main.duration);
            }
            else
            {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(flashInstance, flashPsParts.main.duration);
            }
        }
    }

    void HitTarget()
    {
        if (hit != null)
        {
            var hitRotation = transform.rotation;

            if (LocalRotation == true)
            {
                hitRotation = Quaternion.Euler(0, 0, 0);
            }

            var hitInstance = Instantiate(hit, target.position + targetOffset, hitRotation);
            var hitPs = hitInstance.GetComponent<ParticleSystem>();
            if (hitPs != null)
            {
                Destroy(hitInstance, hitPs.main.duration);
            }
            else
            {
                var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitInstance, hitPsParts.main.duration);
            }
        }

        foreach (var detachedPrefab in Detached)
        {
            if (detachedPrefab != null)
            {
                detachedPrefab.transform.parent = null;
            }
        }

        gameObject.SetActive(false);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(targetTag))
        {
            other.gameObject.GetComponent<Health>().ApplyDamage(insitigator, damage);
        }
    }
}