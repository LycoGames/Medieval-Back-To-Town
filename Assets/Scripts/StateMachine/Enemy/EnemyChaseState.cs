using System;
using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public EnemyChaseState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory) { }

    public override void EnterState()
    {
        Debug.Log("Chase State Entered");
        Ctx.TimeSinceLastSawThePLayer = 0;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        if (Ctx.IsAggrevated())
        {
            Chase();
        }
        else Ctx.NavMeshAgent.ResetPath();
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.GetIsInAttackRange(Ctx.TargetPlayer.transform))
        {
            Ctx.NavMeshAgent.isStopped = true;
            SwitchState(Ctx.States.Attack());

        }

        else if (Ctx.TimeSinceLastSawThePLayer > Ctx.SuspicionTime)
        {
            SwitchState(Ctx.States.Patrol());
        }
    }

    private void Chase()
    {
        Ctx.TimeSinceLastSawThePLayer = 0;
        Ctx.MoveTo(Ctx.TargetPlayer.transform.position, Ctx.SpeedFraction);
    }
}
