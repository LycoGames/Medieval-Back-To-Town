using UnityEngine;
using UnityEngine.InputSystem;

public class WRollState : WBaseState
{
    public WRollState(WStateMachine currentContext, WStateFactory stateFactory) : base(currentContext, stateFactory){}
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
        if (Keyboard.current.rKey.isPressed)
        {
            SwitchState(factory.WCombatIdleState());
        }
    }

    public override void InitializeSubState()
    {
    }
}
