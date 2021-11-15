using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    public EnemyPatrolState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory) { }

    public override void EnterState()
    {
        Debug.Log("Patrol state entered");
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        PatrolBehaviour();
    }

    public override void ExitState()
    {
        Ctx.NavMeshAgent.isStopped = true;
    }


    public override void CheckSwitchStates()
    {
        if (Ctx.IsAggrevated())
        {
            SwitchState(Factory.Chase());
        }
    }

    private void PatrolBehaviour()
    {
        Vector3 nextPosition = Ctx.GuardPosition;

        if (Ctx.PatrolPath == null)
        {
            Ctx.MoveTo(nextPosition, Ctx.SpeedFraction);
            return;
        }

        if (AtWaypoint())
        { //waypointe olan distancem yeterliyse patrol yap degilse sıradaki pozisyonu getcurrent waypointten al. Ardından nextposition'u degiştir o pointe git.
            Ctx.TimeSinceArrivedAtLastPoint = 0;
            DoPatrolOnPoints();
        }
        nextPosition = GetCurrentWaypoint();

        if (Ctx.TimeSinceArrivedAtLastPoint > Ctx.WaitOnPointTime)
        {
            //MoveTo(nextPosition, 1f);
            Ctx.MoveTo(nextPosition, Ctx.SpeedFraction);
        }
    }

    private bool AtWaypoint() //patroldeki waypointlere yakın olup olmadıgımın sorgusu.
    {
        float distanceToWaypoint = Vector3.Distance(Ctx.transform.position, GetCurrentWaypoint());
        return distanceToWaypoint < Ctx.WaypointTolerance;
    }

    private Vector3 GetCurrentWaypoint() //waypointi alıyorum ilk başta 0 gönderip. Aynı zamanda distancede kullanmak için waypointin tranformunu alıyorum.
    {
        return Ctx.PatrolPath.GetWaypoint(Ctx.CurrentWaypointIndex);
    }

    private void DoPatrolOnPoints() // waypointin nextini alıyorum. 
    {
        Ctx.CurrentWaypointIndex = Ctx.PatrolPath.GetNextIndex(Ctx.CurrentWaypointIndex);
    }
}
