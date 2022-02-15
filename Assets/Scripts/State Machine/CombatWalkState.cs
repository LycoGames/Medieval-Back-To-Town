using UnityEngine;

public class CombatWalkState : BaseState
{
    public CombatWalkState(StateMachine currentContext, StateFactory stateFactory) : base(currentContext, stateFactory)
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
            SwitchState(factory.CombatIdleState());
        }

        if (ctx.Input.sprint)
        {
            SwitchState(factory.CombatRunState());
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