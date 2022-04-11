using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(StateMachine currentContext, StateFactory stateFactory) : base(currentContext, stateFactory)
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
        if (ctx.Input.move != Vector2.zero)
        {
            SwitchState(factory.WalkState());
        }
    }

    public override void InitializeSubState()
    {
    }
}