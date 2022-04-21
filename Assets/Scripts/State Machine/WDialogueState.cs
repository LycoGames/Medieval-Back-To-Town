using UnityEngine;

public class WDialogueState : WBaseState
{
    private PlayerConversant playerConversant;
    private AIConversant aiConversant;

    public WDialogueState(WStateMachine currentContext, WStateFactory stateFactory) : base(currentContext, stateFactory)
    {
        InitializeSubState();
    }

    public override void EnterState()
    {
        Debug.Log("Dialogue State Enter");
        SetRequiredComponents();
        StartDialogue();
        Cursor.visible = true;
        ctx.LockCameraPosition = true;
        ctx.CanMove = false;
    }


    public override void UpdateState()
    {
        CheckSwitchStates();
        Debug.Log("Dialogue State Update");
    }

    public override void ExitState()
    {
        Debug.Log("Dialogue State Exit");
        ctx.LockCameraPosition = false;
        Cursor.visible = false;
        ctx.CanMove = true;
        if (aiConversant.IsRepeatableDialogue())
            aiConversant.SetInteractable(true);
    }

    public override void CheckSwitchStates()
    {
        if (!playerConversant.IsActive())
            SwitchState(factory.WFreeState());
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
        playerConversant.ResetUI();
    }

    private void SetRequiredComponents()
    {
        playerConversant = ctx.GetComponent<PlayerConversant>();
        aiConversant = ctx.InteractableNpc.GetComponent<AIConversant>();
    }
}