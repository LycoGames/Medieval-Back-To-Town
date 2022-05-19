using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PathFollower : MonoBehaviour
{
    [SerializeField] int pathID;
    [SerializeField] private PathRouteType pathRouteType = PathRouteType.Circular;
    [SerializeField] private float speed = 2f;
    [SerializeField] float waypointTolerance = 1f;
    [SerializeField] float waitOnPointTime = 2f;

    enum PathRouteType
    {
        Circular,
        CircularReverse,
        RoundTrip,
        ReverseRoundTrip
    }


    private PatrolPath path;
    NavMeshAgent agent;
    private Animator anim;
    int currentWaypointIndex;
    float timeSinceArrivedAtLastPoint = Mathf.Infinity;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        GetPatrolInfos();
    }

    // Update is called once per frame
    void Update()
    {
        PatrolBehaviour();
        UpdateAnimator();
        UpdateTimers();
    }

    public void MoveTo(Vector3 destination)
    {
        agent.destination = destination;

        agent.speed = speed;
        agent.isStopped = false;
    }

    private void PatrolBehaviour()
    {
        Vector3 nextPosition = transform.position;

        if (path != null)
        {
            if (AtWaypoint())
            {
                timeSinceArrivedAtLastPoint = 0;
                DoPatrolOnPoints();
            }

            nextPosition = GetCurrentWaypoint();

            if (timeSinceArrivedAtLastPoint > waitOnPointTime)
            {
                MoveTo(nextPosition);
            }
        }

        if (path == null)
        {
            MoveTo(nextPosition); //ya patrol pathi yoksa ? eski yerine dönsün.
        }
    }

    private bool AtWaypoint() //patroldeki waypointlere yakın olup olmadıgımın sorgusu.
    {
        float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
        return distanceToWaypoint < waypointTolerance;
    }

    private Vector3
        GetCurrentWaypoint()
    {
        return path.GetWaypoint(currentWaypointIndex);
    }


    private void DoPatrolOnPoints() // waypointin nextini alıyorum. 
    {
        /*currentWaypointIndex = shouldPathReverse
            ? path.GetReverseNextIndex(currentWaypointIndex)
            : path.GetNextIndex(currentWaypointIndex);*/
        currentWaypointIndex = pathRouteType switch
        {
            PathRouteType.Circular => path.GetNextIndex(currentWaypointIndex),
            PathRouteType.CircularReverse => path.GetReverseNextIndex(currentWaypointIndex),
            PathRouteType.RoundTrip => path.GetRoundTripNextIndex(currentWaypointIndex, gameObject),
            PathRouteType.ReverseRoundTrip => path.GetReverseRoundTripNextIndex(currentWaypointIndex, gameObject),
            _ => currentWaypointIndex
        };
    }

    private void GetPatrolInfos()
    {
        foreach (var item in FindObjectsOfType<PatrolPath>())
        {
            if (item.GetPathID() != pathID) continue;
            path = item;
            break;
        }

        currentWaypointIndex = path.GetClosestIndex(transform.position);
    }

    private void UpdateAnimator()
    {
        Vector3 velocity = agent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);

        // Update animation parameters
        anim.SetFloat("speed", localVelocity.z);
    }

    private void UpdateTimers()
    {
        timeSinceArrivedAtLastPoint += Time.deltaTime;
    }
}