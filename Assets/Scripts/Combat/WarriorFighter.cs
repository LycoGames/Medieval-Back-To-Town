using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WarriorFighter : Fighter
{
    //Animation IDs
    private int animIDBasicAttack;

    private float timeOfAnimation;
    private bool isAttacking;
    private bool isContiniousAttacking;

    private void Update()
    {
        if (isContiniousAttacking && !isAttacking)
        {
            StartCoroutine(PerformBasicAttack());
        }
    }

    protected override void AssignAnimationIDs()
    {
        animIDBasicAttack = Animator.StringToHash("FullBodyBasicAttack");
        timeOfAnimation = animator.runtimeAnimatorController.animationClips[9].length;
    }


    public override void BasicAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            isContiniousAttacking = true;
        }

        else if (ctx.canceled)
        {
            isContiniousAttacking = false;
        }
    }

    private IEnumerator PerformBasicAttack()
    {
        StartAttack();
        yield return new WaitForSeconds(timeOfAnimation);
        StopAttack();
    }

    private void StartAttack()
    {
        animator.applyRootMotion = true;
        animator.SetTrigger(animIDBasicAttack);
        isAttacking = true;
    }

    private void StopAttack()
    {
        animator.ResetTrigger(animIDBasicAttack);
        isAttacking = false;
        animator.applyRootMotion = false;
    }
}