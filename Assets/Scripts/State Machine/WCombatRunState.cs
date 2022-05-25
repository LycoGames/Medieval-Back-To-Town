using UnityEngine;
using UnityEngine.InputSystem;

public class WCombatRunState : WBaseState
{
    public WCombatRunState(WStateMachine currentContext, WStateFactory stateFactory) : base(currentContext, stateFactory)
    {
    }

    public override void EnterState()
    {
        ctx.SetSpeedToRun();
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
            SwitchState(factory.WCombatWalkState());
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