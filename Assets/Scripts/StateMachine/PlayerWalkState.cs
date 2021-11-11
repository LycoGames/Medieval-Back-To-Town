using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }

    public override void EnterState()
    {
        Ctx.MoveSpeed = Ctx.WalkSpeed;
        Debug.Log("Walk");
    }

    public override void UpdateState()
    {
        if (Ctx.Animator.GetFloat("Speed") != Ctx.WalkSpeed)
            Ctx.Animator.SetFloat("Speed", Ctx.WalkSpeed, 0.1f, Time.deltaTime);
        CheckSwitchStates();
    }

    public override void ExitState() { }

    public override void InitializeSubState() { }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsMovementPressed)
        {
            SwitchState(Factory.Idle());
        }
        else if (Ctx.IsMovementPressed && Ctx.IsRunPressed)
        {
            SwitchState(Factory.Run());
        }
    }
}
