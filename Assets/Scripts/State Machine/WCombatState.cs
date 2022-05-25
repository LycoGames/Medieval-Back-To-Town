using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class WCombatState : WBaseState
{
    public WCombatState(WStateMachine currentContext, WStateFactory stateFactory) : base(currentContext, stateFactory)
    {
        InitializeSubState();
    }

    public override void EnterState()
    {
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        /*if (!(currentSubState is WCombatIdleState))
        {
            ctx.RotatePlayerToMoveDirection();
            ctx.Move();
        }*/
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        if (!ctx.IsAttacking)
            SwitchState(factory.WFreeState());
    }

    public override void InitializeSubState()
    {
        SetSubState(factory.WCombatIdleState());
    }
}