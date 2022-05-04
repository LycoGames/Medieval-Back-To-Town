using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trigger Animation Effect", menuName = "Abilities/Effects/Trigger Animation", order = 0)]
public class TriggerAnimationEffect : EffectStrategy
{
    [SerializeField] string animationTrigger;
    [SerializeField] private bool shouldUseRootMotion;
    [SerializeField] private float rootMotionUseTime = -1;

    public override void StartEffect(AbilityData data, Action finished)
    {
        Animator animator = data.GetUser().GetComponent<Animator>();
        if (shouldUseRootMotion)
        {
            WStateMachine stateMachine = data.GetUser().GetComponent<WStateMachine>();
            if (stateMachine != null)
            {
                stateMachine.StartCoroutine(PerformAnimation(animator, stateMachine));
            }
            else
            {
                StateMachine stateMachineArcher = data.GetUser().GetComponent<StateMachine>();
                stateMachineArcher.StartCoroutine(PerformAnimationForArcher(animator, stateMachineArcher));
            }
        }
        else
            animator.SetTrigger(animationTrigger);

        finished();
    }

    private IEnumerator PerformAnimation(Animator animator, WStateMachine stateMachine)
    {
        float animTime = 0;
        if (rootMotionUseTime == -1)
        {
            foreach (var clip in animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == animationTrigger)
                    animTime = clip.length;
            }
        }
        else
            animTime = rootMotionUseTime;

        animator.applyRootMotion = true;
        animator.SetTrigger(animationTrigger);
        stateMachine.IsAttacking = true;
        yield return new WaitForSeconds(animTime);
        animator.ResetTrigger(animationTrigger);
        stateMachine.IsAttacking = false;
        animator.applyRootMotion = false;
    }

    private IEnumerator PerformAnimationForArcher(Animator animator, StateMachine stateMachine)
    {
        float animTime = 0;
        if (rootMotionUseTime == -1)
        {
            foreach (var clip in animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == animationTrigger)
                    animTime = clip.length;
            }
        }
        else
            animTime = rootMotionUseTime;

        //animator.applyRootMotion = true;
        stateMachine.SetAnimZero();
        stateMachine.CanMove = false;
        animator.SetTrigger(animationTrigger);
        stateMachine.IsAttacking = true;
        yield return new WaitForSeconds(animTime);
        animator.ResetTrigger(animationTrigger);
        stateMachine.IsAttacking = false;
        //  animator.applyRootMotion = false;
        stateMachine.CanMove = true;
    }
}