using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WarriorFighter : Fighter
{
    //Animation IDs
    private int animIDBasicAttack;
    private float timeOfBasicAttackAnim;
    private int animIDMeleeAttack360High;
    private int animIDComboTwo;
    private float timeOfMeleeAttack360HighAnim;
    private float timeOfComboTwo;

    private bool canClick = true;
    private int noOfClicks = 0;
    private int currentCombo = 0;

    protected override void AssignAnimationIDs()
    {
        animIDBasicAttack = Animator.StringToHash("FullBodyBasicAttack");
        animIDMeleeAttack360High = Animator.StringToHash("MeleeAttack360High");
        animIDComboTwo = Animator.StringToHash("ComboTwo");

        foreach (var animClip in animator.runtimeAnimatorController.animationClips)
        {
            switch (animClip.name)
            {
                case "BasicAttack":
                    timeOfBasicAttackAnim = animClip.length;
                    break;
                case "ComboOne":
                    timeOfMeleeAttack360HighAnim = animClip.length;
                    break;
                case "ComboTwo":
                    timeOfComboTwo = animClip.length;
                    break;
            }
        }
    }

    public override void BasicAttack(InputAction.CallbackContext ctx)
    {
        if (GetComponent<WStateMachine>().CurrentState.currentSubState.currentSubState is WUiState ||
            GetComponent<WStateMachine>().CurrentState.currentSubState.currentSubState is WDialogueState)
            return;
        if (ctx.started && canClick)
        {
            noOfClicks++;
        }

        if (ctx.started && noOfClicks == 1)
        {
            StartCoroutine(PerformAttack(animIDBasicAttack));
            currentCombo = 1;
        }
    }

    public void ComboCheck()
    {
        canClick = false;
        if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == animIDBasicAttack && noOfClicks >= 2 &&
            currentCombo == 1)
        {
            canClick = true;
            currentCombo = 2;
            StartCoroutine(PerformAttack(animIDMeleeAttack360High));
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == animIDMeleeAttack360High && noOfClicks >= 3 &&
                 currentCombo == 2)
        {
            canClick = true;
            currentCombo = 3;
            StartCoroutine(PerformAttack(animIDComboTwo));
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == animIDBasicAttack ||
                 animator.GetCurrentAnimatorStateInfo(0).shortNameHash == animIDComboTwo ||
                 animator.GetCurrentAnimatorStateInfo(0).shortNameHash == animIDMeleeAttack360High)
        {
            canClick = true;
            noOfClicks = 0;
            currentCombo = 0;
            stateMachine.IsAttacking = false;
        }
    }

    private IEnumerator PerformAttack(int animID)
    {
        float animTime = 0;

        if (animID == animIDBasicAttack)
            animTime = timeOfBasicAttackAnim;
        else if (animID == animIDMeleeAttack360High)
            animTime = timeOfMeleeAttack360HighAnim;
        else if (animID == animIDComboTwo)
            animTime = timeOfComboTwo;

        StartAttack(animID);
        yield return new WaitForSeconds(animTime);
        StopAttack(animID);
    }

    private void StartAttack(int animID)
    {
        stateMachine.IsAttacking = true;
        animator.applyRootMotion = true;
        animator.SetTrigger(animID);
    }

    private void StopAttack(int animID)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == animID)
        {
            animator.applyRootMotion = false;
        }

        animator.ResetTrigger(animID);
    }
}