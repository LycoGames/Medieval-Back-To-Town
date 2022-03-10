using UnityEngine;
using UnityEngine.InputSystem;

public class WRollState : WBaseState
{
    public WRollState(WStateMachine currentContext, WStateFactory stateFactory) : base(currentContext, stateFactory){}
    public override void EnterState()
    {
        Debug.Log("Roll State Enter");
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        Debug.Log("Roll State Update");

    }

    public override void ExitState()
    {
        Debug.Log("Roll State Exit");
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
