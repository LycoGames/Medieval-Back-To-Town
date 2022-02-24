using UnityEngine;

public class GroundedState : BaseState
{
    public GroundedState(StateMachine currentContext, StateFactory stateFactory) : base(currentContext, stateFactory)
    {
        InitializeSubState();
    }

    public override void EnterState()
    {
        Debug.Log("Grounded State Enter");

        // reset the fall timeout timer
        ctx.FallTimeoutDelta = ctx.FallTimeout;

        // update animator if using character
        if (ctx.HasAnimator)
        {
            ctx.Animator.SetBool(ctx.AnimIDJump, false);
            ctx.Animator.SetBool(ctx.AnimIDFreeFall, false);
        }
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        Timers();
        UpdateSpeed();
        UpdateAnimations();
        Debug.Log("Grounded State Update");
    }

    public override void ExitState()
    {
        Debug.Log("Grounded State Exit");
    }

    public override void CheckSwitchStates()
    {
        if (ctx.JumpTimeoutDelta <= 0.0f && ctx.Input.jump)
        {
            Jump();
            SwitchState(factory.InAirState());
        }
        else if (!ctx.Grounded)
        {
            SwitchState(factory.InAirState());
        }
    }

    public override void InitializeSubState()
    {
        SetSubState(factory.FreeState());
    }

    private void Timers()
    {
        // jump timeout
        if (ctx.JumpTimeoutDelta >= 0.0f)
        {
            ctx.JumpTimeoutDelta -= Time.deltaTime;
        }
    }

    private void Jump()
    {
        // Jump
        // the square root of H * -2 * G = how much velocity needed to reach desired height
        ctx.VerticalVelocity = Mathf.Sqrt(ctx.JumpHeight * -2f * ctx.Gravity);
        // update animator if using character
        if (ctx.HasAnimator)
        {
            ctx.Animator.SetBool(ctx.AnimIDJump, true);
        }

        ctx.JumpTimeoutDelta = ctx.JumpTimeout;
    }

    public void UpdateSpeed()
    {
        float currentHorizontalSpeed =
            new Vector3(ctx.Controller.velocity.x, 0.0f, ctx.Controller.velocity.z).magnitude;

        float speedOffset = 0.1f;

        //If player not reached target speed
        bool isReachedTargetSpeed = !(currentHorizontalSpeed < ctx.TargetSpeed - speedOffset ||
                                      currentHorizontalSpeed > ctx.TargetSpeed + speedOffset);
        if (!isReachedTargetSpeed)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            ctx.Speed = Mathf.Lerp(currentHorizontalSpeed, ctx.TargetSpeed, Time.deltaTime * ctx.SpeedChangeRate);

            // round speed to 3 decimal places
            ctx.Speed = Mathf.Round(ctx.Speed * 1000f) / 1000f;
        }
        else
        {
            ctx.Speed = ctx.TargetSpeed;
        }
    }

    public void UpdateAnimations()
    {
        ctx.AnimationBlend = Mathf.Lerp(ctx.AnimationBlend, ctx.TargetSpeed, Time.deltaTime * ctx.SpeedChangeRate);

        if (ctx.HasAnimator)
        {
            ctx.Animator.SetFloat(ctx.AnimIDSpeed, ctx.AnimationBlend);
            ctx.Animator.SetFloat(ctx.AnimIDMotionSpeed, 1f);
        }
    }
}