using UnityEngine;
using UnityEngine.InputSystem;

public class CombatRunState : BaseState
{
    public CombatRunState(StateMachine currentContext, StateFactory stateFactory) : base(currentContext, stateFactory)
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
        if (!ctx.Input.sprint)
        {
            SwitchState(factory.CombatWalkState());
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