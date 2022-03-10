using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatState : BaseState
{
    private float fireCountdown = 0f;
    bool rotateState = false;


    public CombatState(StateMachine currentContext, StateFactory stateFactory) : base(currentContext, stateFactory)
    {
        InitializeSubState();
    }

    public override void EnterState()
    {
        Debug.Log("Combat State Enter");

        ctx.AimTimer = 2;
        ctx.Anim.SetBool(ctx.animIDAimMoving, true);
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        Timers();
        AimMoving();
        HandleAttack();

        if (ctx.AimTimer < 1)
        {
            ctx.SecondLayerWeight = Mathf.Lerp(ctx.SecondLayerWeight, 0, Time.deltaTime * 2);
        }


        ctx.ApplyGravity();

        Debug.Log("Combat State Update");
    }

    public override void ExitState()
    {
        Debug.Log("Combat State Exit");
        ctx.Anim.SetBool(ctx.animIDAimMoving, false);
    }

    public override void CheckSwitchStates()
    {
        if (ctx.AimTimer < 0)
        {
            ctx.Anim.SetBool(ctx.animIDAimMoving, false);
            SwitchState(factory.FreeState());
        }
    }

    public override void InitializeSubState()
    {
        SetSubState(factory.CombatIdleState());
    }

    private void AimMoving()
    {
        ctx.transform.rotation = Quaternion.Slerp(ctx.transform.rotation, Quaternion.LookRotation(ctx.Forward),
            ctx.DesiredRotationSpeed);
        //TODO rotation 2

        if (ctx.InputY < -0.3)
            ctx.Controller.Move(ctx.DesiredMoveDirection * Time.deltaTime * (ctx.Velocity / 2.4f));
        else if (ctx.InputX < -0.1 || ctx.InputX > 0.1)
            ctx.Controller.Move(ctx.DesiredMoveDirection * Time.deltaTime * (ctx.Velocity / 2.2f));
        else
            ctx.Controller.Move(ctx.DesiredMoveDirection * Time.deltaTime * (ctx.Velocity / 1.8f));
    }

    private void HandleAttack()
    {
        if (ctx.Input.basicAttack)
        {
            if (ctx.ActiveTarget)
            {
                if (fireCountdown <= 0f)
                {
                    if (!ctx.RotateState)
                    {
                        ctx.StartRotateCoroutine(ctx.FireRate, ctx.Target.position);
                        var lookPos = ctx.Target.position - ctx.transform.position;
                        lookPos.y = 0;
                        var rotation = Quaternion.LookRotation(lookPos);
                        var angle = Quaternion.Angle(ctx.transform.rotation, rotation);
                        if (angle > 20)
                        {
                            //turn animation
                            ctx.Anim.SetFloat(ctx.animIDInputX, 0.3f);
                        }
                    }

                    ctx.PerformBasicAttack();
                    fireCountdown = 0;
                    ctx.AimTimer = 1.5f;
                    fireCountdown += ctx.FireRate;
                }
            }
        }
        else if (ctx.AimTimer < 1)
        {
            ctx.SecondLayerWeight = Mathf.Lerp(ctx.SecondLayerWeight, 0, Time.deltaTime * 2);
        }
    }


    private void Timers()
    {
        if (ctx.AimTimer > 0)
        {
            ctx.AimTimer -= Time.deltaTime;
        }

        if (fireCountdown > 0)
        {
            fireCountdown -= Time.deltaTime;
        }
    }
}