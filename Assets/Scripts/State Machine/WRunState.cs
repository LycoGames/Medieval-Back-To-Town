using UnityEngine;
using UnityEngine.InputSystem;

public class WRunState : WBaseState
{
    public WRunState(WStateMachine currentContext, WStateFactory stateFactory) : base(currentContext, stateFactory)
    {
    }

    public override void EnterState()
    {
        ctx.SetSpeedToRun();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
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