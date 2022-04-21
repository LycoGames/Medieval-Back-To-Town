using UnityEngine;
using UnityEngine.InputSystem;

public class WFreeState : WBaseState
{
    public WFreeState(WStateMachine currentContext, WStateFactory stateFactory) : base(currentContext, stateFactory)
    {
        InitializeSubState();
    }

    public override void EnterState()
    {
        Debug.Log("Free State Enter");
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        if (currentSubState.GetType()
            != factory.WIdleState().GetType())
        {
            ctx.RotatePlayerToMoveDirection();
            ctx.Move();
        }

        Debug.Log("Free State Update");
    }

    public override void ExitState()
    {
        Debug.Log("Free State Exit");
    }

    public override void CheckSwitchStates()
    {
        /*if (Keyboard.current.cKey.isPressed)
        {
            ctx.Animator.SetBool(ctx.AnimIDInCombat, true);
            SwitchState(factory.CombatState());
        }*/
        if (ctx.InteractableNpc != null && ctx.Input.interaction)
        {
            SwitchState(factory.WDialogueState());
        }
    }

    public override void InitializeSubState()
    {
        SetSubState(factory.WIdleState());
    }
}