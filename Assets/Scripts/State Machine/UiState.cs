using UnityEngine;
using UnityEngine.InputSystem;

public class UiState : BaseState
{
    //TODO Paneller kaydırılacak
    public UiState(StateMachine currentContext, StateFactory stateFactory) : base(currentContext, stateFactory)
    {
    }

    public override void EnterState()
    {
        ctx.CanMove = false;
        Cursor.lockState = CursorLockMode.None;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void CheckSwitchStates()
    {
        foreach (GameObject ui in ctx.MainUiArray)
        {
            if (ui.activeInHierarchy)
                return;
        }

        SwitchState(factory.FreeState());
    }

    public override void InitializeSubState()
    {
    }
}