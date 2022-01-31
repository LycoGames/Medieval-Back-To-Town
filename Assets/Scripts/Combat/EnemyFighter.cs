using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

public class EnemyFighter : MonoBehaviour, IAction
{

    [SerializeField] float enemyAttackCooldown = 1f;
    [SerializeField] float mobRange = 1f;

    EnemyAIController enemyAIController;
    Animator animator;
    Health targetPlayer;
    Health health;
    BaseStats baseStats;
    [Range(0, 1000)] [SerializeField] float speed = 10f;

    float TimeSinceLastAttack = Mathf.Infinity;


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
        if (targetPlayer == null) return;
        if (!GetIsInRange(targetPlayer.transform))
        {
            enemyAIController.MoveTo(targetPlayer.transform.position, 1f);
        }

        else
        {
            AttackBehaviour();
            enemyAIController.Cancel();
        }

        UpdateTimers();
    }

    public bool GetIsInRange(Transform targetTransform)
    {
        return Vector3.Distance(transform.position, targetTransform.transform.position) < mobRange;
    }

    private void UpdateTimers()
    {
        TimeSinceLastAttack += Time.deltaTime;
    }

    public void AttackBehaviour()
    {   //attack animasyonunu baslatıcak. Aynı zamanda animasyondaki Hit eventini baslatıcak.

        /* var targetRotation = Quaternion.LookRotation(targetPlayer.transform.position - transform.position);
        targetRotation.y = 0.0f;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed);
        */

        Vector3 TargetDir = targetPlayer.transform.position - transform.position;
        TargetDir.y = 0.0f;
        var step = Time.deltaTime * speed;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(TargetDir), step);


        // transform.LookAt(targetPlayer.transform.position);
        if (TimeSinceLastAttack > enemyAttackCooldown)
        {
            animator.SetTrigger("attack");
            TimeSinceLastAttack = 0;
        }
    }

    public void Attack(GameObject combatTarget)
    {
        GetComponent<ActionScheduler>().StartAction(this);
        targetPlayer = combatTarget.GetComponent<Health>(); //targetplayeri setledim.
    }

    public void Cancel()
    {
        targetPlayer = null;
        GetComponent<EnemyAIController>().Cancel();
    }

    public void Hit()
    {
        float targetPlayerHealth = targetPlayer.GetComponent<Health>().GetHealthPoints();
        targetPlayer.GetComponent<Health>().ApplyDamage(GetDamage());
    }

    private float GetDamage()
    {
        return GetComponent<BaseStats>().GetBaseStat(Stat.Damage);
    }
}
