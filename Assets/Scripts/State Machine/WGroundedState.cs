using UnityEngine;

public class WGroundedState : WBaseState
{
    public WGroundedState(WStateMachine currentContext, WStateFactory stateFactory) : base(currentContext, stateFactory)
    {
        InitializeSubState();
    }

    public override void EnterState()
    {
        Debug.Log("Grounded State Enter");

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
        if (!ctx.Grounded)
        {
            SwitchState(factory.WInAirState());
        }
    }

    public override void InitializeSubState()
    {
        SetSubState(factory.WFreeState());
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