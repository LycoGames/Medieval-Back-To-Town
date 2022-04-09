using UnityEngine;
using UnityEngine.InputSystem;

public class WalkState : BaseState
{
    public WalkState(StateMachine currentContext, StateFactory stateFactory) : base(currentContext, stateFactory)
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
        if (ctx.Input.move == Vector2.zero)
        {
            SwitchState(factory.IdleState());
        }

        if (ctx.Input.sprint)
        {
            SwitchState(factory.RunState());
        }
    }

    public override void InitializeSubState()
    {
    }
}