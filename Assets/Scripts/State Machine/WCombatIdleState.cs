using UnityEngine;

public class WCombatIdleState : WBaseState
{
    public WCombatIdleState(WStateMachine currentContext, WStateFactory stateFactory) : base(currentContext, stateFactory)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Combat Idle State Enter");
        ctx.SetSpeedToIdle();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        Debug.Log("Combat Idle State Update");
    }

    public override void ExitState()
    {
        Debug.Log("Combat Idle State Exit");
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