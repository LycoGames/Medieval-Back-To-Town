using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }

    public override void EnterState()
    {
        Debug.Log("Walk");
    }

    public override void UpdateState()
    {
        if (Ctx.Animator.GetFloat("Speed") != Ctx.WalkSpeed)
            Ctx.Animator.SetFloat("Speed", Ctx.WalkSpeed, 0.1f, Time.deltaTime);

        Vector3 moveDir = Quaternion.Euler(0f, Ctx.RotationAngle, 0f) * Vector3.forward;
        Ctx.MoveSpeed = Ctx.WalkSpeed;
        Ctx.CharacterController.Move(moveDir.normalized * Ctx.WalkSpeed * Time.deltaTime);
        Ctx.transform.rotation = Quaternion.Euler(0f, Ctx.RotationAngle, 0f);
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
        else if (Ctx.IsMovementPressed && Ctx.IsRunPressed)
        {
            SwitchState(Factory.Run());
        }
    }
}
