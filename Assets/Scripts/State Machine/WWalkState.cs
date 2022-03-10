using UnityEngine;
using UnityEngine.InputSystem;

public class WWalkState : WBaseState
{
    public WWalkState(WStateMachine currentContext, WStateFactory stateFactory) : base(currentContext, stateFactory)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Walk State Enter");
        ctx.SetSpeedToWalk();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        Debug.Log("Walk State Update");
    }

    public override void ExitState()
    {
        Debug.Log("Walk State Exit");
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