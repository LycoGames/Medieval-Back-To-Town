using UnityEngine;
using UnityEngine.InputSystem;

public class RollState : BaseState
{
    public RollState(StateMachine currentContext, StateFactory stateFactory) : base(currentContext, stateFactory){}
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
            SwitchState(factory.CombatIdleState());
        }
    }

    public override void InitializeSubState()
    {
    }
}
