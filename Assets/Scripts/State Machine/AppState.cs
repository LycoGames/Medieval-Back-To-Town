using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AppState : BaseState
{
    private const float Threshold = 0.01f;
    private const int CursorSwitchSpeed = 1000;


    //User interface variables
    private Vector3 screenCenter;
    private Vector3 screenPos;
    private Vector3 cornerDistance;
    private Vector3 absCornerDistance;
    private Vector3 worldViewField;

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
        ctx.InitializeWeapon();
    }

    public override void UpdateState()
    {
        Debug.Log("App State Update");
        UserInterface();
        ctx.InputMagnitude();
        GetTarget();

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

    private void UserInterface()
    {
        DetectEnemies();
        screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2;
        if (!ctx.Target)
        {
            if (ctx.Aim.transform.position == screenCenter)
                return;

            ctx.Aim.transform.position =
                Vector3.MoveTowards(ctx.Aim.transform.position, screenCenter, Time.deltaTime * CursorSwitchSpeed);
            if (ctx.ActiveTarget)
                ctx.ActiveTarget = false;
            return;
        }

        screenPos = ctx.Cam.WorldToScreenPoint(ctx.Target.position + (Vector3) ctx.UiOffset);
        cornerDistance = screenPos - screenCenter;
        absCornerDistance = new Vector3(Mathf.Abs(cornerDistance.x), Mathf.Abs(cornerDistance.y),
            Mathf.Abs(cornerDistance.z));

        if (absCornerDistance.x < screenCenter.x / ctx.TargetingSense &&
            absCornerDistance.y < screenCenter.y / ctx.TargetingSense && screenPos.x > 0 &&
            screenPos.y > 0 && screenPos.z > 0 //If target is in the middle of the screen
            && !Physics.Linecast(ctx.transform.position + (Vector3) ctx.UiOffset,
                ctx.Target.position + (Vector3) ctx.UiOffset * 2,
                ctx.CollidingLayer)) //If player can see the target
        {
            ctx.Aim.transform.position =
                Vector3.MoveTowards(ctx.Aim.transform.position, screenPos, Time.deltaTime * CursorSwitchSpeed);
            if (!ctx.ActiveTarget)
                ctx.ActiveTarget = true;
        }
        else
        {
            ctx.Aim.transform.position =
                Vector3.MoveTowards(ctx.Aim.transform.position, screenCenter, Time.deltaTime * CursorSwitchSpeed);
            if (ctx.ActiveTarget)
                ctx.ActiveTarget = false;
        }

        DetectEnemies();
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
        /* ctx.CinemachineCameraTarget.transform.rotation = Quaternion.Euler(
             cinemachineTargetPitch + ctx.CameraAngleOverride,
             cinemachineTargetYaw, 0.0f);*/

        ctx.CinemachineCameraTarget.transform.rotation = Quaternion.Euler(
            cinemachineTargetPitch,
            cinemachineTargetYaw, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    private void GetTarget()
    {
        if (ctx.ScreenTargets.Count == 0 && ctx.Target)
        {
            ctx.Target = null;
        }

        if (ctx.ScreenTargets.Count != 0)
        {
            ctx.Target = ctx.ScreenTargets[ctx.targetIndex()];
        }
    }

    void DetectEnemies()
    {
        Collider[] hitColliders =
            Physics.OverlapSphere(ctx.transform.position, ctx.CurrentWeaponConfig.GetRange(),
                LayerMask.GetMask("Enemy"));
        ctx.ScreenTargets.Clear();
        foreach (var target in hitColliders)
        {
            if (!ctx.ScreenTargets.Contains(target.gameObject.transform))
                ctx.ScreenTargets.Add(target.gameObject.transform);
        }
    }

    /*public void RemoveTargetFromTargetList(GameObject target)
    {
        if (ctx.ScreenTargets.Contains(target.transform))
        {
            ctx.ScreenTargets.Remove(target.transform);
        }
    }*/
}