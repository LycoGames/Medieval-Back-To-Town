using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FreeState : BaseState
{
    private float rotationVelocity;

    public FreeState(StateMachine currentContext, StateFactory stateFactory) : base(currentContext, stateFactory)
    {
        InitializeSubState();
    }

    public override void EnterState()
    {
        ctx.CanMove = true;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        ctx.InputMagnitude();
        PlayerMoveAndRotation();
        ctx.ApplyGravity();
    }


    public override void ExitState()
    {
        ctx.SetAnimZero();
    }

    public override void CheckSwitchStates()
    {
        if (ctx.Input.basicAttack && ctx.Aim.enabled)
        {
            ctx.Anim.SetBool(ctx.animIDAimMoving, true);
            SwitchState(factory.CombatState());
        }

        if (ctx.InteractableNpc != null && ctx.Input.interaction)
        {
            SwitchState(factory.DialogueState());
        }

        //TODO
        foreach (GameObject ui in ctx.MainUiArray)
        {
            if (ui.activeInHierarchy)
            {
                SwitchState(factory.UiState());
            }
        }
    }

    public override void InitializeSubState()
    {
        SetSubState(factory.IdleState());
        factory.IdleState().EnterState();
    }

    private void PlayerMoveAndRotation()
    {
        if (currentSubState.GetType() != ctx.States.IdleState().GetType())
        {
            Vector3 inputDirection = new Vector3(ctx.Input.move.x, 0.0f, ctx.Input.move.y).normalized;

            ctx.TargetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                 ctx.Cam.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(ctx.transform.eulerAngles.y, ctx.TargetRotation,
                ref rotationVelocity,
                ctx.RotationSmoothTime);

            // rotate to face input direction relative to camera position
            ctx.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            /*    
                ctx.transform.rotation = Quaternion.Slerp(ctx.transform.rotation,
                    Quaternion.LookRotation(ctx.DesiredMoveDirection),
                    ctx.DesiredRotationSpeed);*/
        }

        ctx.Controller.Move(ctx.DesiredMoveDirection * Time.deltaTime * ctx.Velocity);
    }
}