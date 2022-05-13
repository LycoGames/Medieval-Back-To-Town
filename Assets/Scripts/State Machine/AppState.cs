using UnityEngine;
using UnityEngine.AI;
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
        ctx.InitializeWeapon();
    }

    public override void UpdateState()
    {
        UserInterface();
        GetTarget();
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
    }

    public override void InitializeSubState()
    {
        SetSubState(factory.GroundedState());
        factory.GroundedState().EnterState();
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
        Vector2 offSet = new Vector2(0, 2 * (ctx.Target.GetComponent<NavMeshAgent>().height) / 3);
        //  Debug.Log(ctx.Target.GetComponent<NavMeshAgent>().height);
        ctx.UiOffset = offSet;
        // Debug.Log("off: " + ctx.UiOffset);
        screenPos = ctx.Cam.WorldToScreenPoint(ctx.Target.position + (Vector3)ctx.UiOffset);
        cornerDistance = screenPos - screenCenter;
        absCornerDistance = new Vector3(Mathf.Abs(cornerDistance.x), Mathf.Abs(cornerDistance.y),
            Mathf.Abs(cornerDistance.z));

        if (absCornerDistance.x < screenCenter.x / ctx.TargetingSense &&
            absCornerDistance.y < screenCenter.y / ctx.TargetingSense && screenPos.x > 0 &&
            screenPos.y > 0 && screenPos.z > 0 //If target is in the middle of the screen
            && !Physics.Linecast(ctx.transform.position + (Vector3)ctx.UiOffset,
                ctx.Target.position + (Vector3)ctx.UiOffset,
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

    private void GetTarget()
    {
        if (ctx.ScreenTargets.Count == 0 && ctx.Target)
        {
            ctx.Target = null;
        }

        if (ctx.ScreenTargets.Count != 0)
        {
            if (ctx.ScreenTargets[ctx.targetIndex()].GetComponent<Health>().IsDead())
            {
                ctx.Target = null;
                return;
            }
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