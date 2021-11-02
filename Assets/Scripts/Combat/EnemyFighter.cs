using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFighter : MonoBehaviour
{

    [SerializeField] float enemyAttackCooldown = 1f;

    EnemyAIController enemyAIController;
    Animator animator;
    GameObject targetPlayer;
    Health health;
    BaseStats baseStats;


    float TimeSinceLastAttack = Mathf.Infinity;

    void Awake()
    {
        animator = GetComponent<Animator>();
        enemyAIController = GetComponent<EnemyAIController>();
        targetPlayer = GameObject.FindWithTag("Player");
        health = GetComponent<Health>();
        baseStats = GetComponent<BaseStats>();
    }

    void Start()
    {

    }

    void Update()
    {
        print("health : " + health.GetInitialHealth());
        print("base health : "+baseStats.GetBaseStat(Stat.Health));
        UpdateTimers();
    }

    private void UpdateTimers()
    {
        TimeSinceLastAttack += Time.deltaTime;
    }

    public void AttackBehaviour()
    {   //attack animasyonunu baslatıcak. Aynı zamanda animasyondaki Hit eventini baslatıcak.
        transform.LookAt(targetPlayer.transform.position);
        if (TimeSinceLastAttack > enemyAttackCooldown)
        {
            animator.SetTrigger("attack");
            TimeSinceLastAttack = 0;
        }
    }

    private void Hit()
    {
        float targetPlayerHealth = targetPlayer.GetComponent<Health>().GetHealthPoints();
        print("players health: " + targetPlayerHealth);
        targetPlayer.GetComponent<Health>().ApplyDamage(GetDamage());
        print("im hittin this float " +GetDamage());
    }

    private float GetDamage()
    {
        return GetComponent<BaseStats>().GetBaseStat(Stat.Damage);
    }
}
