using UnityEngine;

public class GroundedState : BaseState
{
    public GroundedState(StateMachine currentContext, StateFactory stateFactory) : base(currentContext, stateFactory)
    {
        InitializeSubState();
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
        if (ctx.Input.jump )
        {
            SwitchState(factory.InAirState());
        }
       /* else if (!ctx.Grounded)
        {
            SwitchState(factory.InAirState());
        }*/
    }

    public override void InitializeSubState()
    {
        SetSubState(factory.FreeState());
        factory.FreeState().EnterState();
    }

  
}