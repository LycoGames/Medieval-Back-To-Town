using UnityEngine;
using UnityEngine.InputSystem;

public class RunState : BaseState
{
    public RunState(StateMachine currentContext, StateFactory stateFactory) : base(currentContext, stateFactory)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Run State Enter");
        ctx.SetSpeedToRun();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        Debug.Log("Run State Update");
    }

    public override void ExitState()
    {
        Debug.Log("Run State Exit");
    }

    public override void CheckSwitchStates()
    {
        if (!ctx.Input.sprint)
        {
            SwitchState(factory.WalkState());
        }
    }

    public override void InitializeSubState()
    {
    }
}