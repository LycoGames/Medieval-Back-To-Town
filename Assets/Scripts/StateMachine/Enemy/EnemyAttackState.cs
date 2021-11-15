using System;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory) { }

    public override void EnterState()
    {
        Debug.Log("Attack State Entered");

    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        Attack();
        UpdateTimers();
    }


    private void Attack()
    {
        Ctx.transform.LookAt(Ctx.TargetPlayer.transform.position);
        if (Ctx.TimeSinceLastAttack > Ctx.EnemyAttackCooldown)
        {
            Ctx.NavMeshAgent.isStopped = true;
            Ctx.Animator.SetTrigger("attack");
            Ctx.TimeSinceLastAttack = 0;
        }
    }

    public override void ExitState() { }

    public override void CheckSwitchStates()
    {
        if (!Ctx.GetIsInAttackRange(Ctx.TargetPlayer.transform))
        {
            SwitchState(Ctx.States.Chase());
        }
    }

    private void UpdateTimers()
    {
        Ctx.TimeSinceLastAttack += Time.deltaTime;
    }

}
