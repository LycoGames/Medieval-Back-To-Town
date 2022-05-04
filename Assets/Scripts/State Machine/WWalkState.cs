using UnityEngine;
using UnityEngine.InputSystem;

public class WWalkState : WBaseState
{
    public WWalkState(WStateMachine currentContext, WStateFactory stateFactory) : base(currentContext, stateFactory)
    {
    }

    public override void EnterState()
    {
        ctx.SetSpeedToWalk();
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
        if (ctx.Input.move == Vector2.zero)
        {
            SwitchState(factory.WIdleState());
        }

        if (ctx.Input.sprint)
        {
            SwitchState(factory.WRunState());
        }
    }

    public override void InitializeSubState()
    {
    }
}