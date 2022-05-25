using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class WFreeState : WBaseState
{
    public WFreeState(WStateMachine currentContext, WStateFactory stateFactory) : base(currentContext, stateFactory)
    {
        InitializeSubState();
    }

    public override void EnterState()
    {
        ctx.CanMove = true;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        if (!(currentSubState is WIdleState))
        {
            ctx.RotatePlayerToMoveDirection();
            ctx.Move();
        }

    }

    public override void ExitState()
    {
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

        if (ctx.IsAttacking)
        {
            SwitchState(factory.WCombatState());
        }

        //TODO
        foreach (GameObject ui in ctx.MainUiArray)
        {
            if (ui.activeInHierarchy)
            {
                SwitchState(factory.WUiState());
            }
        }
    }


    public override void InitializeSubState()
    {
        SetSubState(factory.WIdleState());
    }
}