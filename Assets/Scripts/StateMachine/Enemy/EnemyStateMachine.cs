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

    [Header("Enemy Fov")]
    [SerializeField] float radius;
    [Range(0, 360)]
    [SerializeField] float angle;
    [SerializeField] float delayUpdateForOptimize = 0.2f;
    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstructionMask;



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
    private bool canSeePlayer;


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
        UpdateAnimator();
        UpdateTimers();
    }

    private void LateUpdate()
    {
        FOVChecker();
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

    public void Aggrevated() //event! damage aldıgında event tetikliycek.
    {
        timeSinceAggrevated = 0;
    }

    public void CallNearbyEnemies()
    { //https://docs.unity3d.com/ScriptReference/Physics.SphereCastAll.html
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, callArea, Vector3.up, 0);
        foreach (RaycastHit hit in hits)
        {
            EnemyAIController ai = hit.collider.GetComponent<EnemyAIController>(); //vuran her raycast hiti için(her bir mob için) bu scriptteki Aggrevated'i tetikle. 
            if (ai == null) continue;
            if (ai == this) continue;
            ai.Aggrevated();
        }
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

    private void UpdateAnimator()
    {
        Vector3 velocity = navMeshAgent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        animator.SetFloat("forwardSpeed", speed);
    }

    private void FOVChecker()
    { //https://docs.unity3d.com/ScriptReference/Physics.OverlapSphere.html
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, targetMask); // targetMask'e verdigimiz layer ile carpısanları alıyorum.
        if (hitColliders.Length != 0)
        {
            foreach (Collider hit in hitColliders)
            {
                Transform target = hit.transform; //vuran hitlerin transformu Transform olan target adlı degiskene attım.
                                                  // https://docs.unity3d.com/ScriptReference/Vector3-normalized.html
                Vector3 directionToTarget = (target.position - transform.position).normalized; //targetin transform posi. ile enemyinin pozisyonu çıkarıp çıkan noktayı (yönü) attım normalize ettim (yt).
                                                                                               //https://docs.unity3d.com/ScriptReference/Vector3.Angle.html
                if (Vector3.Angle(transform.forward, directionToTarget) < angle) //ileriye dogru olan açı ile direction target arasındaki açıyı derece cinsinden alıp verdigimiz angle degerinden kücüklük sorgusu.
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position); // enemy ile player arasındaki mesafe
                    //https://docs.unity3d.com/ScriptReference/Physics.Raycast.html
                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))//engel var mı sorgusu raycast obstructionMask'taki layer ile carpısmıyorsa.
                    {
                        canSeePlayer = true;
                    }

                    else canSeePlayer = false;

                }
                else canSeePlayer = false;

            }

        }
        else if (canSeePlayer) canSeePlayer = false; // artık fovda degilsem cıkmıssım fovdan false yap.

    }

    public bool GetCanSeePlayer()
    {
        return canSeePlayer;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius);

        Vector3 viewAngle01 = DirectionFromAngle(transform.eulerAngles.y, -angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(transform.eulerAngles.y, angle / 2);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + viewAngle01 * radius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngle02 * radius);
    }
    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
