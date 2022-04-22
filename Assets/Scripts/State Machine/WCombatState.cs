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
        Debug.Log("Combat State Enter");
    }

    public override void UpdateState()
    {
        Debug.Log("Combat State Update");
        CheckSwitchStates();
        /*if (!(currentSubState is WCombatIdleState))
        {
            ctx.RotatePlayerToMoveDirection();
            ctx.Move();
        }*/
    }

    public override void ExitState()
    {
        Debug.Log("Combat State Exit");
    }

    public override void CheckSwitchStates()
    {
        if(!ctx.IsAttacking)
            SwitchState(factory.WFreeState());
    }

    public override void InitializeSubState()
    {
        SetSubState(factory.WCombatIdleState());
    }
}