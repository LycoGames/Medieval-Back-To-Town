using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using System;

public class EnemyAIController : MonoBehaviour
{
    [SerializeField] PatrolPath patrolPath;
    [SerializeField] Transform target;
    [SerializeField] float waypointTolerance = 1f;
    [SerializeField] float maxSpeed = 1f;
    [SerializeField] float chaseDistance = 5f;
    [SerializeField] float suspicionTime = 4f;
    [SerializeField] float WaitOnPointTime = 2f;

    Vector3 guardPosition;
    GameObject targetPlayer;
    NavMeshAgent navMeshAgent;
    EnemyFighter enemyFighter;
    Health playerHealth;
    Health health;

    int currentWaypointIndex = 0;
    float timeSinceLastSawThePLayer = Mathf.Infinity;
    float timeSinceArrivedAtLastPoint = Mathf.Infinity;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyFighter = GetComponent<EnemyFighter>();
        targetPlayer = GameObject.FindWithTag("Player");
        playerHealth = targetPlayer.GetComponent<Health>();
        health = GetComponent<Health>();
    }

    void Start()
    {
        guardPosition = transform.position;
    }


    void Update()
    {
        if (health.IsDead()) return; // enemy ai öldüyse playeri takibi kes.
        if (playerHealth.IsDead()) return;

        if (IsAggrevated())
        {
            AttackBehaviour();
            MoveTo(targetPlayer.transform.position, 1f);

        }

        else if (timeSinceLastSawThePLayer < suspicionTime)
        {
            SuspicionBehaviour();
        }

        else
        {
            PatrolBehaviour();
        }

        UpdateTimers();

        // GetComponent<NavMeshAgent>().destination = target.position;
    }

    private void AttackBehaviour()
    {
        timeSinceLastSawThePLayer = 0f;
        enemyFighter.AttackBehaviour();
    }

    private void UpdateTimers()
    {
        timeSinceLastSawThePLayer += Time.deltaTime;
        timeSinceArrivedAtLastPoint += Time.deltaTime;
    }

    public void MoveTo(Vector3 destination, float speedFraction)
    {
        GetComponent<NavMeshAgent>().destination = destination;
        navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
        navMeshAgent.isStopped = false;
    }

    private void PatrolBehaviour()
    {
        Vector3 nextPosition = guardPosition; //mobun eski pozisyonunu tuttum.

        if (patrolPath != null) // patrol pathi yoksa eski yerine geri dönsün.
        {
            if (AtWaypoint())
            { //waypointe olan distancem yeterliyse patrol yap degilse sıradaki pozisyonu getcurrent waypointten al. Ardından nextposition'u degiştir o pointe git.
                timeSinceArrivedAtLastPoint = 0;
                DoPatrolOnPoints();
            }
            nextPosition = GetCurrentWaypoint();

            if (timeSinceArrivedAtLastPoint > WaitOnPointTime)
            {
                MoveTo(nextPosition, 1f);
            }

        }

        if (patrolPath == null)
        {
            MoveTo(nextPosition, 1f); //ya patrol pathi yoksa ? eski yerine dönsün.
        }

    }



    private bool AtWaypoint() //patroldeki waypointlere yakın olup olmadıgımın sorgusu.
    {
        float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
        return distanceToWaypoint < waypointTolerance;
    }

    private void DoPatrolOnPoints() // waypointin nextini alıyorum. 
    {
        currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
    }

    private Vector3 GetCurrentWaypoint() //waypointi alıyorumilk başta 0 gönderip. Aynı zamanda distancede kullanmak için waypointin tranformunu alıyorum.
    {
        return patrolPath.GetWaypoint(currentWaypointIndex);
    }

    private void SuspicionBehaviour()
    {
        GetComponent<ActionScheduler>().CancelCurrentAction();
    }

    public void Cancel()
    {
        navMeshAgent.isStopped = true;
    }

    public bool IsAggrevated()
    {
        float distanceToPlayer = Vector3.Distance(targetPlayer.transform.position, transform.position);
        return distanceToPlayer < chaseDistance;

    }






    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}
