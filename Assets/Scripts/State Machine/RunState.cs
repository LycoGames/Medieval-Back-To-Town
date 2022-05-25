using UnityEngine;
using UnityEngine.InputSystem;

public class RunState : BaseState
{
    public RunState(StateMachine currentContext, StateFactory stateFactory) : base(currentContext, stateFactory)
    {
    }

    public override void EnterState()
    {
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
            SwitchState(factory.WalkState());
        }
    }

    public override void InitializeSubState()
    {
    }
}