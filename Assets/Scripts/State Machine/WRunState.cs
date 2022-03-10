using UnityEngine;
using UnityEngine.InputSystem;

public class WRunState : WBaseState
{
    public WRunState(WStateMachine currentContext, WStateFactory stateFactory) : base(currentContext, stateFactory)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Run State Enter");
        ctx.SetSpeedToRun();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        Debug.Log("Run State Update");
    }

    public override void ExitState()
    {
        Debug.Log("Run State Exit");
    }

    public override void CheckSwitchStates()
    {
        if (!ctx.Input.sprint)
        {
            SwitchState(factory.WWalkState());
        }
    }

    public override void InitializeSubState()
    {
    }
}