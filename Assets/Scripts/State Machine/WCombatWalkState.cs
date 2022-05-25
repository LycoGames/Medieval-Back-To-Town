using UnityEngine;

public class WCombatWalkState : WBaseState
{
    public WCombatWalkState(WStateMachine currentContext, WStateFactory stateFactory) : base(currentContext, stateFactory)
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
            SwitchState(factory.WCombatIdleState());
        }

        if (ctx.Input.sprint)
        {
            SwitchState(factory.WCombatRunState());
        }

        if (ctx.Input.roll)
        {
            SwitchState(factory.WRollState());
        }
    }

    public override void InitializeSubState()
    {
    }
}