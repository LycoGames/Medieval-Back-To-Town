using UnityEngine;
using UnityEngine.InputSystem;

public class CombatState : BaseState
{
    public CombatState(StateMachine currentContext, StateFactory stateFactory) : base(currentContext, stateFactory)
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
            != factory.CombatIdleState().GetType())
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
            SwitchState(factory.FreeState());
        }
    }

    public override void InitializeSubState()
    {
        SetSubState(factory.CombatIdleState());
    }
}