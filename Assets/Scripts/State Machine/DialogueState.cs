using UnityEngine;

public class DialogueState : BaseState
{
    private PlayerConversant playerConversant;
    private AIConversant aiConversant;

    public DialogueState(StateMachine currentContext, StateFactory stateFactory) : base(currentContext, stateFactory)
    {
        InitializeSubState();
    }

    public override void EnterState()
    {
        SetRequiredComponents();
        StartDialogue();
        ctx.CanMove = false;
        ctx.LockCameraPosition = true;
        Cursor.lockState = CursorLockMode.None; //HardCoding
        Cursor.visible = true;
    }


    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        ctx.LockCameraPosition = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        ctx.AimUI.SetActive(true);
        if (aiConversant.IsRepeatableDialogue())
            aiConversant.SetInteractable(true);
    }

    public override void CheckSwitchStates()
    {
        if (!playerConversant.IsActive())
            SwitchState(factory.FreeState());
    }

    public override void InitializeSubState()
    {
    }

    private void StartDialogue()
    {
        playerConversant.StartDialogue(aiConversant.GetDialogue());
        aiConversant.SetInteractable(false);
        ctx.LockCameraPosition = true;
        Cursor.visible = true;
        ctx.AimUI.SetActive(false);
        playerConversant.ResetUI();
    }

    private void SetRequiredComponents()
    {
        playerConversant = ctx.GetComponent<PlayerConversant>();
        aiConversant = ctx.InteractableNpc.GetComponent<AIConversant>();
    }
}