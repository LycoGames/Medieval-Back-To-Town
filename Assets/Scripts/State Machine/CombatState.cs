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
        CheckSwitchStates();
        if (currentSubState.GetType()
            != factory.CombatIdleState().GetType())
        {
            ctx.RotatePlayerToMoveDirection();
        }

        ctx.Move();

        Debug.Log("Combat State Update");
    }

    public override void ExitState()
    {
        Debug.Log("Combat State Exit");
    }

    public override void CheckSwitchStates()
    {
        if (Keyboard.current.gKey.isPressed)
        {
            SwitchState(factory.FreeState());
        }
    }

    public override void InitializeSubState()
    {
        SetSubState(factory.CombatIdleState());
    }
}