using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EffectDamage : MonoBehaviour
{
    private float damage = 1f;
    private GameObject instigator;
    private List<GameObject> hitEnemies;

    private void OnEnable()
    {
        hitEnemies = new List<GameObject>();
        instigator = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        var health = other.GetComponent<Health>();
        if (other == null | !other.CompareTag("Enemy") | hitEnemies.Contains(other.gameObject) || health.IsDead() ||
            health == null)
            return;

        health.ApplyDamage(instigator, damage);
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
}