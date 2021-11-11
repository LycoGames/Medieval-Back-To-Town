using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }

    public override void EnterState()
    {
        handleJump();
    }

    public override void UpdateState()
    {
        ApplyGravity();
        ChechSwitchStates();
    }

    public override void ExitState()
    {
        ctx.VelocityY = 0;
    }

    public override void InitializeSubState() { }

    public override void ChechSwitchStates()
    {
        if (ctx.IsFalling && ctx.IsGrounded)
        {
            SwitchState(factory.Grounded());
        }
    }

    void handleJump()
    {
        ctx.VelocityY = Mathf.Sqrt(ctx.JumpHeight * -2 * ctx.Gravity);
    }

    private void ApplyGravity()
    {
        ctx.IsFalling = ctx.VelocityY < 0 ? true : false;
        ctx.VelocityY += ctx.Gravity * Time.deltaTime;
        ctx.characterController.Move(ctx.Velocity * Time.deltaTime);
    }
}
