using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }

    public override void EnterState()
    {

        Ctx.MoveSpeed = Ctx.RunSpeed;
        Debug.Log("Run");
    }

    public override void UpdateState()
    {
        if (Ctx.Animator.GetFloat("Speed") != Ctx.RunSpeed)
            Ctx.Animator.SetFloat("Speed", Ctx.RunSpeed, 0.1f, Time.deltaTime);
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
        else if (Ctx.IsMovementPressed && !Ctx.IsRunPressed)
        {
            SwitchState(Factory.Walk());
        }
    }
}
