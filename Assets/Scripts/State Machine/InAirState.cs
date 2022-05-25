using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InAirState : BaseState
{
    public InAirState(StateMachine currentContext, StateFactory stateFactory) : base(currentContext, stateFactory)
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
       /* if (ctx.Grounded)
        {
            SwitchState(factory.GroundedState());
        }*/
    }

    public override void InitializeSubState()
    {
    }
}