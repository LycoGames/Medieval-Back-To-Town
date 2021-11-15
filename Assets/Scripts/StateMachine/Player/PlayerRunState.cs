using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }

    public override void EnterState()
    {
        Debug.Log("Run");
    }

    public override void UpdateState()
    {
        if (Ctx.Animator.GetFloat("Speed") != Ctx.RunSpeed)
            Ctx.Animator.SetFloat("Speed", Ctx.RunSpeed, 0.1f, Time.deltaTime);

        Vector3 moveDir = Quaternion.Euler(0f, Ctx.RotationAngle, 0f) * Vector3.forward;
        Ctx.MoveSpeed = Ctx.RunSpeed;
        Ctx.CharacterController.Move(moveDir.normalized * Ctx.RunSpeed * Time.deltaTime);
        Ctx.PlayerTransform.rotation = Quaternion.Euler(0f, Ctx.RotationAngle, 0f);
        CheckSwitchStates();
    }

    public override void ExitState() { }

    public override void InitializeSubState() { }

    public override void CheckSwitchStates()
    {
        if (Ctx.IsAimPressed)
        {
            SwitchState(Factory.Aim());
        }
        else if (!Ctx.IsMovementPressed)
        {
            SwitchState(Factory.Idle());
        }
        else if (Ctx.IsMovementPressed && !Ctx.IsRunPressed)
        {
            SwitchState(Factory.Walk());
        }
    }
}
