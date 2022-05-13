using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using System;

public class EnemyAIController : MonoBehaviour, IAction
{
    [SerializeField] PatrolPath patrolPath;
    [SerializeField] int pathID;
    [SerializeField] Transform target;
    [SerializeField] float waypointTolerance = 1f;
    [SerializeField] float maxSpeed = 1f;
    [SerializeField] float chaseDistance = 5f;
    [SerializeField] float suspicionTime = 4f;
    [SerializeField] float waitOnPointTime = 2f;
    [SerializeField] float agroCooldownTime = 4f;
    [SerializeField] float callArea = 3f;

    public GameManager gameManager;

    LazyValue<Vector3> guardPosition;
    GameObject targetPlayer;
    NavMeshAgent navMeshAgent;
    EnemyFighter enemyFighter;
    Health playerHealth;
    Health health;
    EnemyFOV enemyFOV;
    Animator animator;
    CanvasDisabler canvasDisabler;
    int currentWaypointIndex = 0;
    float timeSinceLastSawThePLayer = Mathf.Infinity;
    float timeSinceArrivedAtLastPoint = Mathf.Infinity;
    float timeSinceAggrevated = Mathf.Infinity;

    void Awake()
    {
        GetRequiredComponents();
    }

    void Start()
    {
        canvasDisabler = GetComponent<CanvasDisabler>();
        enemyFOV = GetComponent<EnemyFOV>();
        GetPlayer();
        GetPatrolInfos();
        guardPosition.ForceInit();
    }


    void Update()
    {
        if (health.IsDead()) return; // enemy ai öldüyse playeri takibi kes.

        if (playerHealth.IsDead()) return;

        if (IsAggrevated() || enemyFOV.GetCanSeePlayer())
        {
            AttackBehaviour();
            //MoveTo(targetPlayer.transform.position, 1f);
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
        UpdateAnimator();

        // GetComponent<NavMeshAgent>().destination = target.position;
    }

    public void Reset()
    {
        NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.Warp(guardPosition.value);

        currentWaypointIndex = 0;
        timeSinceLastSawThePLayer = Mathf.Infinity;
        timeSinceArrivedAtLastPoint = Mathf.Infinity;
        timeSinceAggrevated = Mathf.Infinity;
    }


    public void Aggrevated() //event! damage aldıgında event tetikliycek.
    {
        timeSinceAggrevated = 0;
    }

    public void MoveTo(Vector3 destination, float speedFraction)
    {
        GetComponent<NavMeshAgent>().destination = destination;
        navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
        navMeshAgent.isStopped = false;
    }

    public bool IsAggrevated()
    {
        float distanceToPlayer = Vector3.Distance(targetPlayer.transform.position, transform.position);
        return (distanceToPlayer < chaseDistance || timeSinceAggrevated < agroCooldownTime);
    }

    private void GetRequiredComponents()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyFighter = GetComponent<EnemyFighter>();
        health = GetComponent<Health>();
        animator = GetComponent<Animator>();
        guardPosition = new LazyValue<Vector3>(GetGuardPosition);
        guardPosition.ForceInit();
    }

    private Vector3 GetGuardPosition()
    {
        return transform.position;
    }

    private void GetPlayer()
    {
        targetPlayer = GameObject.FindGameObjectWithTag("Player");
        playerHealth = targetPlayer.GetComponent<Health>();
    }

    private void GetPatrolInfos()
    {
        foreach (var item in GameObject.FindObjectsOfType<PatrolPath>())
        {
            if (item.GetPathID() == pathID)
            {
                patrolPath = item;
            }
        }
        //  patrolPath = GameObject.FindObjectsOfType<PatrolPath>();
        currentWaypointIndex = patrolPath.GetWaypointIndex(transform);
    }

    private void AttackBehaviour()
    {
        timeSinceLastSawThePLayer = 0f;
        enemyFighter
            .Attack(targetPlayer); //enemy fighter scriptinde targetPlayeri setlemek göndermek için.(enemy fighter da findwithtag="player" silindigi için)
        canvasDisabler.SetActiveCanvas();
    }

    public void CallNearbyEnemies()
    {
        //https://docs.unity3d.com/ScriptReference/Physics.SphereCastAll.html
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, callArea, Vector3.up, 0);
        foreach (RaycastHit hit in hits)
        {
            EnemyAIController
                ai = hit.collider
                    .GetComponent<EnemyAIController>(); //vuran her raycast hiti için(her bir mob için) bu scriptteki Aggrevated'i tetikle. 
            if (ai == null) continue;
            if (ai == this) continue;
            ai.Aggrevated();
        }
    }

    private void UpdateTimers()
    {
        timeSinceLastSawThePLayer += Time.deltaTime;
        timeSinceArrivedAtLastPoint += Time.deltaTime;
        timeSinceAggrevated += Time.deltaTime;
    }


    private void PatrolBehaviour()
    {
        Vector3 nextPosition = guardPosition.value; //mobun eski pozisyonunu tuttum.

        if (patrolPath != null) // patrol pathi yoksa eski yerine geri dönsün.
        {
            if (AtWaypoint())
            {
                //waypointe olan distancem yeterliyse patrol yap degilse sıradaki pozisyonu getcurrent waypointten al. Ardından nextposition'u degiştir o pointe git.
                timeSinceArrivedAtLastPoint = 0;
                DoPatrolOnPoints();
            }

            nextPosition = GetCurrentWaypoint();

            if (timeSinceArrivedAtLastPoint > waitOnPointTime)
            {
                //MoveTo(nextPosition, 1f);
                StartMoveAction(nextPosition, 1f);
            }
        }

        if (patrolPath == null)
        {
            MoveTo(nextPosition, 1f); //ya patrol pathi yoksa ? eski yerine dönsün.
        }
    }

    private void StartMoveAction(Vector3 destination, float speedFraction)
    {
        GetComponent<ActionScheduler>().StartAction(this);
        MoveTo(destination, speedFraction);
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

    private Vector3
        GetCurrentWaypoint() //waypointi alıyorum ilk başta 0 gönderip. Aynı zamanda distancede kullanmak için waypointin tranformunu alıyorum.
    {
        return patrolPath.GetWaypoint(currentWaypointIndex);
    }

    private void SuspicionBehaviour()
    {
        GetComponent<ActionScheduler>().CancelCurrentAction();
        canvasDisabler.SetDisableCanvas();
    }

    public void Cancel()
    {
        navMeshAgent.isStopped = true;
    }


    private void UpdateAnimator()
    {
        Vector3 velocity = navMeshAgent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        GetComponent<Animator>().SetFloat("forwardSpeed", speed);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}