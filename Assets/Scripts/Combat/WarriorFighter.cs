using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WarriorFighter : Fighter
{
    //Animation IDs
    private int animIDComboOne;
    private int animIDComboThree;
    private int animIDComboTwo;
    private int currentComboAnimID;


    private bool canClick = true;
    private int noOfClicks;
    private int currentCombo;

    protected override void AssignAnimationIDs()
    {
        animIDComboOne = Animator.StringToHash("ComboOne");
        animIDComboTwo = Animator.StringToHash("ComboTwo");
        animIDComboThree = Animator.StringToHash("ComboThree");
    }

    public override void BasicAttack(InputAction.CallbackContext ctx)
    {
        if (GetCurrentWeaponConfig() == null || GetCurrentWeaponConfig().name == "Unarmed")
            return;

        if (GetComponent<WStateMachine>().CurrentState.currentSubState.currentSubState is WUiState ||
            GetComponent<WStateMachine>().CurrentState.currentSubState.currentSubState is WDialogueState)
            return;
        if (ctx.started && canClick)
        {
            noOfClicks++;
        }

        if (ctx.started && noOfClicks == 1)
        {
            currentComboAnimID = animIDComboOne;
            PerformAttack(animIDComboOne);
            currentCombo = 1;
        }
    }

    public void ComboCheck()
    {
        canClick = false;
        if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == animIDComboOne && noOfClicks >= 2 &&
            currentCombo == 1)
        {
            canClick = true;
            currentCombo = 2;
            currentComboAnimID = animIDComboTwo;
            PerformAttack(animIDComboTwo);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == animIDComboTwo && noOfClicks >= 3 &&
                 currentCombo == 2)
        {
            canClick = true;
            currentCombo = 3;
            currentComboAnimID = animIDComboThree;
            PerformAttack(animIDComboThree);
        }
        else if (InCombo())
        {
            canClick = true;
            noOfClicks = 0;
            currentCombo = 0;
        }
    }

    public void StopAttack()
    {
        if (!InCombo() || animator.GetCurrentAnimatorStateInfo(0).shortNameHash == currentComboAnimID)
            stateMachine.IsAttacking = false;
    }

    private bool InCombo()
    {
        return animator.GetCurrentAnimatorStateInfo(0).shortNameHash == animIDComboOne ||
               animator.GetCurrentAnimatorStateInfo(0).shortNameHash == animIDComboTwo ||
               animator.GetCurrentAnimatorStateInfo(0).shortNameHash == animIDComboThree;
    }


    private void PerformAttack(int animID)
    {
        StartAttack(animID);
    }

    private void StartAttack(int animID)
    {
        stateMachine.IsAttacking = true;
        animator.applyRootMotion = true;
        animator.SetTrigger(animID);
    }
}