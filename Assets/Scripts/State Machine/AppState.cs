using UnityEngine;

public class AppState : BaseState
{
    private const float Threshold = 0.01f;

    //cinemachine
    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;

    public AppState(StateMachine currentContext, StateFactory stateFactory) : base(currentContext, stateFactory)
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
        CameraRotation();
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
        SetSubState(factory.GroundedState());
    }


    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (ctx.Input.look.sqrMagnitude >= Threshold && !ctx.LockCameraPosition)
        {
            cinemachineTargetYaw += ctx.Input.look.x * Time.deltaTime;
            cinemachineTargetPitch += ctx.Input.look.y * Time.deltaTime;
        }

        // clamp our rotations so our values are limited 360 degrees
        cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
        cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, ctx.BottomClamp, ctx.TopClamp);

        // Cinemachine will follow this target
        ctx.CinemachineCameraTarget.transform.rotation = Quaternion.Euler(
            cinemachineTargetPitch + ctx.CameraAngleOverride,
            cinemachineTargetYaw, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    public void ApplyGravity()
    {
        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)

        if (currentSubState.GetType() == factory.GroundedState().GetType() && ctx.VerticalVelocity < 0)
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