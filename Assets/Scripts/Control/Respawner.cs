using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    [SerializeField] Transform respawnLocation;
    [SerializeField] float respawnDelay = 3;
    [SerializeField] float fadeTime = 0.2f;
    [SerializeField] float healthRegenPercentage = 100;
    [SerializeField] float enemyHealthRegenPercentage = 30;


    private void Awake()
    {
        GetComponent<Health>().onDie.AddListener(Respawn);
    }

    private void Respawn()
    {
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
        savingWrapper.Save();
        StateMachine stateMachine = GetComponent<StateMachine>();
        WStateMachine wStateMachine = GetComponent<WStateMachine>();
        if (stateMachine == null)
            wStateMachine.enabled = false;
        else
            stateMachine.enabled = false;
        yield return new WaitForSeconds(respawnDelay);
        Fader fader = FindObjectOfType<Fader>();
        yield return fader.FadeOut(fadeTime);
        RespawnPlayer();
        ResetEnemies();
        yield return fader.FadeIn(fadeTime);
        if (stateMachine == null)
            wStateMachine.enabled = true;
        else
            stateMachine.enabled = true;
    }

    private void ResetEnemies()
    {
        foreach (EnemyAIController enemyController in FindObjectsOfType<EnemyAIController>())
        {
            Health health = enemyController.GetComponent<Health>();
            if (health && !health.IsDead())
            {
                enemyController.Reset();
                health.Heal(health.GetMaxHealthPoints() * enemyHealthRegenPercentage / 100);
            }
        }
    }

    private void RespawnPlayer()
    {
        transform.position = respawnLocation.position;
        Health health = GetComponent<Health>();
        health.Heal(health.GetMaxHealthPoints() * healthRegenPercentage / 100);
        Animator animator = GetComponent<Animator>();
        animator.Rebind();
        animator.applyRootMotion = false;
    }
}