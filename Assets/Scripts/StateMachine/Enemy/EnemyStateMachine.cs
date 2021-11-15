using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour
{
    [SerializeField] PatrolPath patrolPath;
    [SerializeField] Transform target;
    [SerializeField] float waypointTolerance = 1f;
    [SerializeField] float maxSpeed = 1f;
    [SerializeField] float chaseDistance = 5f;
    [SerializeField] float suspicionTime = 4f;
    [SerializeField] float waitOnPointTime = 2f;
    [SerializeField] float agroCooldownTime = 4f;
    [SerializeField] float callArea = 3f;
    [SerializeField] float speedFraction = 1f;
    [SerializeField] float mobRange = 1f;
    [SerializeField] float enemyAttackCooldown = 1f;

    public GameManager gameManager;

    Vector3 guardPosition;
    GameObject targetPlayer;
    NavMeshAgent navMeshAgent;
    EnemyFighter enemyFighter;
    Health playerHealth;
    Health health;
    EnemyFOV enemyFOV;
    Transform enemyTransform;
    Animator animator;


    int currentWaypointIndex = 0;
    float timeSinceLastSawThePLayer = Mathf.Infinity;
    float timeSinceArrivedAtLastPoint = Mathf.Infinity;
    float timeSinceAggrevated = Mathf.Infinity;
    float timeSinceLastAttack = Mathf.Infinity;

    // state Variables
    EnemyBaseState currentState;
    EnemyStateFactory states;

    //getters and setters
    public EnemyBaseState CurrentState { get { return currentState; } set { currentState = value; } }
    public NavMeshAgent NavMeshAgent { get { return navMeshAgent; } }
    public EnemyStateFactory States { get { return states; } }
    public float MaxSpeed { get { return maxSpeed; } }
    public float WaypointTolerance { get { return waypointTolerance; } }
    public float TimeSinceArrivedAtLastPoint { get { return timeSinceArrivedAtLastPoint; } set { timeSinceArrivedAtLastPoint = value; } }
    public float TimeSinceLastSawThePLayer { get { return timeSinceLastSawThePLayer; } set { timeSinceLastSawThePLayer = value; } }
    public float TimeSinceAggrevated { get { return timeSinceAggrevated; } set { timeSinceAggrevated = value; } }
    public float TimeSinceLastAttack { get { return timeSinceLastAttack; } set { timeSinceLastAttack = value; } }
    public float EnemyAttackCooldown { get { return enemyAttackCooldown; } }
    public float SuspicionTime { get { return suspicionTime; } set { suspicionTime = value; } }
    public float WaitOnPointTime { get { return waitOnPointTime; } set { waitOnPointTime = value; } }
    public float SpeedFraction { get { return speedFraction; } }
    public int CurrentWaypointIndex { get { return currentWaypointIndex; } set { currentWaypointIndex = value; } }
    public PatrolPath PatrolPath { get { return patrolPath; } }
    public Vector3 GuardPosition { get { return guardPosition; } }
    public GameObject TargetPlayer { get { return targetPlayer; } }
    public Health PlayerHealth { get { return playerHealth; } }
    public Animator Animator { get { return animator; } }


    void Awake()
    {
        GetRequiredComponents();
    }

    void Start()
    {
        states = new EnemyStateFactory(this);
        guardPosition = transform.position;
        enemyFOV = GetComponent<EnemyFOV>();
        GetPlayer();
        GetPatrolInfos();
        currentState = states.Patrol();
        currentState.EnterState();
    }

    void Update()
    {
        currentState.UpdateState();
        UpdateTimers();
    }

    private void GetPlayer()
    {
        targetPlayer = GameObject.FindGameObjectWithTag("Player");
        playerHealth = targetPlayer.GetComponent<Health>();
    }

    private void GetPatrolInfos()
    {
        patrolPath = GameObject.FindObjectOfType<PatrolPath>();
        currentWaypointIndex = patrolPath.GetWaypointIndex(transform);
    }

    private void GetRequiredComponents()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyFighter = GetComponent<EnemyFighter>();
        health = GetComponent<Health>();
        animator = GetComponent<Animator>();
    }

    public bool IsAggrevated()
    {
        float distanceToPlayer = Vector3.Distance(targetPlayer.transform.position, transform.position);
        return (distanceToPlayer < chaseDistance || timeSinceAggrevated < agroCooldownTime);
    }

    public void MoveTo(Vector3 destination, float speedFraction)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.destination = destination;
        navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
    }

    public bool GetIsInAttackRange(Transform targetTransform)
    {
        return Vector3.Distance(transform.position, targetTransform.transform.position) < mobRange;
    }
    private void UpdateTimers()
    {
        timeSinceLastSawThePLayer += Time.deltaTime;
        timeSinceArrivedAtLastPoint += Time.deltaTime;
        timeSinceAggrevated += Time.deltaTime;
    }

    public void Hit()
    {
        playerHealth.ApplyDamage(GetDamage());
    }

    private float GetDamage()
    {
        return GetComponent<BaseStats>().GetBaseStat(Stat.Damage);
    }
}
