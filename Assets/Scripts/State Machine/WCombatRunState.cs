using UnityEngine;
using UnityEngine.InputSystem;

public class WCombatRunState : WBaseState
{
    public WCombatRunState(WStateMachine currentContext, WStateFactory stateFactory) : base(currentContext, stateFactory)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Combat Run State Enter");
        ctx.SetSpeedToRun();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        Debug.Log("Combat Run State Update");
    }

    public override void ExitState()
    {
        Debug.Log("Combat Run State Exit");
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