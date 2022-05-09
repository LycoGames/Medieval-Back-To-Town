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
        Debug.Log("Ui State Enter");
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        Debug.Log("Ui State Update");
    }

    public override void ExitState()
    {
        Debug.Log("Ui State Exit");
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