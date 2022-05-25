using UnityEngine;

public class WCombatIdleState : WBaseState
{
    public WCombatIdleState(WStateMachine currentContext, WStateFactory stateFactory) : base(currentContext, stateFactory)
    {
    }

    public override void EnterState()
    {
        ctx.SetSpeedToIdle();
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
            SwitchState(factory.WCombatWalkState());
        }
    }

    public override void InitializeSubState()
    {
    }
}