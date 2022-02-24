using UnityEngine;
using UnityEngine.InputSystem;

public class CombatRunState : BaseState
{
    public CombatRunState(StateMachine currentContext, StateFactory stateFactory) : base(currentContext, stateFactory)
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