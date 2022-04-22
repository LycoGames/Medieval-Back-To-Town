using UnityEngine;

public class WAppState : WBaseState
{
    public WAppState(WStateMachine currentContext, WStateFactory stateFactory) : base(currentContext, stateFactory)
    {
        isRootState = true;
        InitializeSubState();
    }

    public override void EnterState()
    {
        Debug.Log("App State Enter");
    }

    public override void UpdateState()
    {
        Debug.Log("App State Update");
        ApplyGravity();
        GroundedCheck();
    }

    public override void ExitState()
    {
        Debug.Log("App State Exit");
    }

    public override void CheckSwitchStates()
    {
    }

    public override void InitializeSubState()
    {
        SetSubState(factory.WGroundedState());
    }

    public void ApplyGravity()
    {
        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)

        if (currentSubState is WGroundedState && ctx.VerticalVelocity < 0)
        {
            ctx.VerticalVelocity = -2f;
        }

        else if (ctx.VerticalVelocity < ctx.TerminalVelocity)
        {
            ctx.VerticalVelocity += ctx.Gravity * Time.deltaTime;
        }
    }

    public void GroundedCheck()
    {
        // set sphere position, with offset
        var position = ctx.transform.position;

        Vector3 spherePosition = new Vector3(position.x, position.y - ctx.GroundedOffset,
            position.z);
        ctx.Grounded = Physics.CheckSphere(spherePosition, ctx.GroundedRadius, ctx.GroundLayers,
            QueryTriggerInteraction.Ignore);

        // update animator if using character
        if (ctx.HasAnimator)
        {
            ctx.Animator.SetBool(ctx.AnimIDGrounded, ctx.Grounded);
        }
    }
}