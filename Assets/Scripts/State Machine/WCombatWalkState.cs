using UnityEngine;

public class WCombatWalkState : WBaseState
{
    public WCombatWalkState(WStateMachine currentContext, WStateFactory stateFactory) : base(currentContext, stateFactory)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Combat Walk State Enter");
        ctx.SetSpeedToWalk();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        Debug.Log("Combat Walk State Update");
    }

    public override void ExitState()
    {
        Debug.Log("Combat Walk State Exit");
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