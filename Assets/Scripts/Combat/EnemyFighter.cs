using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
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
    
    /*Network Variables*/

    void Awake()
    {
        animator = GetComponent<Animator>();
        enemyAIController = GetComponent<EnemyAIController>();
        health = GetComponent<Health>();
        baseStats = GetComponent<BaseStats>();
    }

    void Start()
    {

    }

    void Update()
    {
        UpdateTimers();
    }

    private void UpdateTimers()
    {
        TimeSinceLastAttack += Time.deltaTime;
    }

    public void AttackBehaviour(GameObject targetPlayer)
    {   //attack animasyonunu baslatıcak. Aynı zamanda animasyondaki Hit eventini baslatıcak.
        this.targetPlayer = targetPlayer;
        transform.LookAt(targetPlayer.transform.position);
        if (TimeSinceLastAttack > enemyAttackCooldown)
        {
            animator.SetTrigger("attack");
            TimeSinceLastAttack = 0;
        }
    }

    public void Hit()
    {
        /*float targetPlayerHealth = targetPlayer.GetComponent<Health>().GetHealthPoints();*/
        targetPlayer.GetComponent<Health>().ApplyDamage(GetDamage());
    }

    private float GetDamage()
    {
        return GetComponent<BaseStats>().GetBaseStat(Stat.Damage);
    }
}
