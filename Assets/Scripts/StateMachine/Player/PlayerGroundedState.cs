using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }

    public override void EnterState()
    {
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        HandleAttack();
    }

    public override void ExitState()
    {
    }

    public override void InitializeSubState()
    {
        if (!Ctx.IsMovementPressed)
        {
            SetSubState(Factory.Idle());
        }
        else if (Ctx.IsMovementPressed && !Ctx.IsRunPressed)
        {
            SetSubState(Factory.Walk());
        }
        else
        {
            SetSubState(Factory.Run());
        }
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.CurrentState.GetCurrentSubState().ToString().Trim() != Ctx.States.Aim().ToString().Trim() && Ctx.IsJumpPressed)
        {
            SwitchState(Factory.Jump());
        }
    }

    private void HandleAttack()
    {
        if (Ctx.IsBasicAttackPressed)
        {
            Ctx.Fighter.AttackBehaviour();
        }
    }
}
