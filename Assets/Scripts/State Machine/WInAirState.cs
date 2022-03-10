using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WInAirState : WBaseState
{
    //timeout deltatime
    private float fallTimeoutDelta;

    public WInAirState(WStateMachine currentContext, WStateFactory stateFactory) : base(currentContext, stateFactory)
    {
        InitializeSubState();
    }

    public override void EnterState()
    {
        Debug.Log("In Air State Enter");
        // if we are not grounded, do not jump
        ctx.Input.jump = false;
    }

    public override void UpdateState()
    {
        Debug.Log("In Air State Update");
        ctx.Move();
        ctx.RotatePlayerToMoveDirection();

        // fall timeout
        if (fallTimeoutDelta >= 0.0f)
        {
            fallTimeoutDelta -= Time.deltaTime;
        }
        else
        {
            // update animator if using character
            if (ctx.HasAnimator)
            {
                ctx.Animator.SetBool(ctx.AnimIDFreeFall, true);
            }
        }

        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Debug.Log("In Air State Exit");
        // reset the fall timeout timer
        fallTimeoutDelta = ctx.FallTimeout;

        // update animator if using character
        if (ctx.HasAnimator)
        {
            ctx.Animator.SetBool(ctx.AnimIDJump, false);
            ctx.Animator.SetBool(ctx.AnimIDFreeFall, false);
        }
    }

    public override void CheckSwitchStates()
    {
        if (ctx.Grounded)
        {
            SwitchState(factory.WGroundedState());
        }
    }

    public override void InitializeSubState()
    {
    }
}