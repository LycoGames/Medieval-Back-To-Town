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
        Debug.Log("In Air State Enter");
     
    }

    public override void UpdateState()
    {
        Debug.Log("In Air State Update");

        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Debug.Log("In Air State Exit");
      
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