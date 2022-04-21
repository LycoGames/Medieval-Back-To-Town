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
    private float timeOfMeleeAttack360HighAnim;


    private bool canClick = true;
    private int noOfClicks = 0;

    private static readonly int ComboOne = Animator.StringToHash("ComboOne");

    protected override void AssignAnimationIDs()
    {
        animIDBasicAttack = Animator.StringToHash("FullBodyBasicAttack");
        animIDMeleeAttack360High = Animator.StringToHash("MeleeAttack360High");

        foreach (var animClip in animator.runtimeAnimatorController.animationClips)
        {
            if (animClip.name == "BasicAttack")
                timeOfBasicAttackAnim = animClip.length;
            if (animClip.name == "ComboOne")
                timeOfMeleeAttack360HighAnim = animClip.length;
        }
    }

    public override void BasicAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.started && canClick)
        {
            noOfClicks++;
        }

       /* if (ctx.started && noOfClicks == 1)
        {
            StartCoroutine(PerformAttack(animIDBasicAttack));
        }*/
    }

    public void ComboCheck()
    {
        canClick = false;
        if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == animIDBasicAttack && noOfClicks == 1)
        {
            canClick = true;
            noOfClicks = 0;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == animIDBasicAttack && noOfClicks >= 2)
        {
            StartCoroutine(PerformAttack(animIDMeleeAttack360High));
            canClick = true;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == animIDMeleeAttack360High)
        {
            canClick = true;
            noOfClicks = 0;
        }
    }

    private IEnumerator PerformAttack(int animID)
    {
        StartAttack(animID);
        yield return new WaitForSeconds(animID == animIDBasicAttack
            ? timeOfBasicAttackAnim
            : timeOfMeleeAttack360HighAnim);
        StopAttack(animID);
    }

    private void StartAttack(int animID)
    {
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