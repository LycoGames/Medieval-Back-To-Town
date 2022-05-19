using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Ability", menuName = "Abilities/Ability", order = 0)]
public class Ability : ActionItem
{
    [SerializeField] TargetingStrategy targetingStrategy;
    [SerializeField] FilterStrategy[] filterStrategies;
    [SerializeField] EffectStrategy[] effectStrategies;
    [SerializeField] AudioClip audioClip = null;
    [SerializeField] float soundDelay;
    [SerializeField] float cooldownTime = 0;
    [SerializeField] float manaCost = 0;
    [SerializeField] bool isTargetRequired = false;
    AudioSource audioSource = null;

    public override void Use(GameObject user)
    {
        StateMachine stateMachine = user.GetComponent<StateMachine>();
        Transform target = null;

        if (stateMachine)
        {
            target = user.GetComponent<StateMachine>().Target;
        }

        if (isTargetRequired && target == null)
        {
            stateMachine.ShowNoTargetText();
            return;
        }

        Mana mana = user.GetComponent<Mana>();
        if (mana.GetMana() < manaCost)
        {
            stateMachine.ShowNotEnoughManaText();
            return;
        }

        CooldownStore cooldownStore = user.GetComponent<CooldownStore>();
        if (cooldownStore.GetTimeRemaining(this) > 0)
        {
            return;
        }


        if (audioClip != null)
        {
            PlaySoundEffect(user);
        }

        AbilityData data = new AbilityData(user);

        //    ActionScheduler actionScheduler = user.GetComponent<ActionScheduler>();
        //    actionScheduler.StartAction(data);

        targetingStrategy.StartTargeting(data,
            () => { TargetAquired(data); });
    }

    private void PlaySoundEffect(GameObject user)
    {
        audioSource = user.GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.PlayDelayed(soundDelay);
    }

    private void TargetAquired(AbilityData data)
    {
        if (data.IsCancelled()) return;
        Mana mana = data.GetUser().GetComponent<Mana>();
        if (!mana.UseMana(manaCost))
            return; //if my mana higher than cost it'll return true, if its false mymana doesnt enough so return directly.

        CooldownStore cooldownStore = data.GetUser().GetComponent<CooldownStore>();
        cooldownStore.StartCooldown(this, cooldownTime);

        foreach (var filterStrategy in filterStrategies)
        {
            data.SetTargets(filterStrategy.Filter(data.GetTargets()));
        }

        foreach (var effect in effectStrategies)
        {
            effect.StartEffect(data, EffectFinished);
        }
    }

    private void EffectFinished()
    {
    }
}