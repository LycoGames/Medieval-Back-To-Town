using UnityEngine;

public class CombatWalkState : BaseState
{
    public CombatWalkState(StateMachine currentContext, StateFactory stateFactory) : base(currentContext, stateFactory)
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
            SwitchState(factory.CombatIdleState());
        }

        if (ctx.Input.sprint)
        {
            SwitchState(factory.CombatRunState());
        }

        if (ctx.Input.roll)
        {
            SwitchState(factory.RollState());
        }
    }

    public override void InitializeSubState()
    {
    }
}