using UnityEngine;
using UnityEngine.InputSystem;

public class RollState : BaseState
{
    public RollState(StateMachine currentContext, StateFactory stateFactory) : base(currentContext, stateFactory){}
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
            SwitchState(factory.CombatIdleState());
        }
    }

    public override void InitializeSubState()
    {
    }
}
