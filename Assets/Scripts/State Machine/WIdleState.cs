using UnityEngine;

public class WIdleState : WBaseState
{
    public WIdleState(WStateMachine currentContext, WStateFactory stateFactory) : base(currentContext, stateFactory)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Idle State Enter");
        ctx.SetSpeedToIdle();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        Debug.Log("Idle State Update");
    }

    public override void ExitState()
    {
        Debug.Log("Idle State Exit");
    }

    public override void CheckSwitchStates()
    {
        if (ctx.Input.move != Vector2.zero)
        {
            SwitchState(factory.WWalkState());
        }
    }

    public override void InitializeSubState()
    {
    }
}