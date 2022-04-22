using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WInAirState : WBaseState
{
    public WInAirState(WStateMachine currentContext, WStateFactory stateFactory) : base(currentContext, stateFactory)
    {
        InitializeSubState();
    }

    public override void EnterState()
    {
        Debug.Log("In Air State Enter");
        // if we are not grounded, do not jump
    }

    public override void UpdateState()
    {
        Debug.Log("In Air State Update");
        ctx.Move();
        ctx.RotatePlayerToMoveDirection();

        // fall timeout
        if (ctx.FallTimeoutDelta >= 0.0f)
        {
            ctx.FallTimeoutDelta -= Time.deltaTime;
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
        ctx.FallTimeoutDelta = ctx.FallTimeout;

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