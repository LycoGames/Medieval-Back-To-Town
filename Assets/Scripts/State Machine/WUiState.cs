using UnityEngine;
using UnityEngine.InputSystem;

public class WUiState : WBaseState
{
    //TODO Paneller kaydırılacak
    public WUiState(WStateMachine currentContext, WStateFactory stateFactory) : base(currentContext, stateFactory)
    {
    }

    public override void EnterState()
    {
        ctx.CanMove = false;
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
        foreach (GameObject ui in ctx.MainUiArray)
        {
            if (ui.activeInHierarchy)
                return;
        }

        SwitchState(factory.WFreeState());
    }

    public override void InitializeSubState()
    {
    }
}