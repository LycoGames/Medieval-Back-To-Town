using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFighter : MonoBehaviour
{

    [SerializeField] float enemyAttackCooldown = 1f;
    [SerializeField] float mobDamage = 1f;

    EnemyAIController enemyAIController;
    Animator animator;
    GameObject targetPlayer;


    float TimeSinceLastAttack = Mathf.Infinity;

    void Awake()
    {
        animator = GetComponent<Animator>();
        enemyAIController = GetComponent<EnemyAIController>();
        targetPlayer = GameObject.FindWithTag("Player");
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
        print("im hittin "+targetPlayerHealth);
        targetPlayer.GetComponent<Health>().ApplyDamage(mobDamage);
    }

}
