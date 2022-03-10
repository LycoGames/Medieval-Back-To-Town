using UnityEngine;

public class GroundedState : BaseState
{
    public GroundedState(StateMachine currentContext, StateFactory stateFactory) : base(currentContext, stateFactory)
    {
        InitializeSubState();
    }

    public override void EnterState()
    {
        Debug.Log("Grounded State Enter");
        
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        Debug.Log("Grounded State Update");
    }

    public override void ExitState()
    {
        Debug.Log("Grounded State Exit");
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
    }

  
}