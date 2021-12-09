using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }

    public override void EnterState()
    {
        Debug.Log("Idle");
    }

    public override void UpdateState()
    {
        if (Ctx.Animator.GetFloat("Speed") != 0)
            Ctx.Animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);

        CheckSwitchStates();
    }

    public override void ExitState() { }

    public override void InitializeSubState() { }

    public override void CheckSwitchStates()
    {
        if (Ctx.IsMovementPressed && Ctx.IsRunPressed)
        {
            SwitchState(Factory.Run());
        }
        else if (Ctx.IsMovementPressed)
        {
            SwitchState(Factory.Walk());
        }
    }
}
