using UnityEngine;
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
        if (currentSubState.GetType()
            != factory.WCombatIdleState().GetType())
        {
            ctx.RotatePlayerToMoveDirection();
            ctx.Move();
        }
    }

    public override void ExitState()
    {
        Debug.Log("Combat State Exit");
    }

    public override void CheckSwitchStates()
    {
        if (Keyboard.current.gKey.isPressed)
        {
            ctx.Animator.SetBool(ctx.AnimIDInCombat, false);
            SwitchState(factory.WFreeState());
        }
    }

    public override void InitializeSubState()
    {
        SetSubState(factory.WCombatIdleState());
    }
}