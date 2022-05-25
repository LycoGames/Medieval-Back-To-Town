using UnityEngine;

public class CombatIdleState : BaseState
{
    public CombatIdleState(StateMachine currentContext, StateFactory stateFactory) : base(currentContext, stateFactory)
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
            SwitchState(factory.CombatWalkState());
        }
    }

    public override void InitializeSubState()
    {
    }
}