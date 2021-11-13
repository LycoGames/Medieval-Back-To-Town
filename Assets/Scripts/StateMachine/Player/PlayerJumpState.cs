using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }

    public override void EnterState()
    {
        handleJump();
    }

    public override void UpdateState()
    {
        ApplyGravity();
        CheckSwitchStates();
    }

    public override void ExitState()
    {
    }

    public override void InitializeSubState() { }

    public override void CheckSwitchStates()
    {
        if (Ctx.IsFalling && Ctx.IsGrounded)
        {
            SwitchState(Factory.Grounded());
        }
    }

    void handleJump()
    {
        Ctx.VelocityY = Mathf.Sqrt(Ctx.JumpHeight * -2 * Ctx.Gravity);
    }

    private void ApplyGravity()
    {
        Ctx.IsFalling = Ctx.VelocityY < 0 ? true : false;
        Ctx.VelocityY += Ctx.Gravity * Time.deltaTime;
        Ctx.CharacterController.Move(Ctx.Velocity * Time.deltaTime);
    }
}
