using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PotionEffect : MonoBehaviour, IModifierProvider
{
    [SerializeField] private List<PotionItem.Modifier> additiveModifierList;
    [SerializeField] private List<PotionItem.Modifier> percentageModifierList;

    private Dictionary<Modifiers, float> cooldownTimers;
    private Health health;
    private Mana mana;


    private struct Modifiers
    {
        public PotionItem.Modifier[] additiveModifiers;
        public PotionItem.Modifier[] percentageModifiers;
    }

    private void Start()
    {
        cooldownTimers = new Dictionary<Modifiers, float>();
        health = GetComponent<Health>();
        mana = GetComponent<Mana>();
    }

    private void Update()
    {
        var keys = new List<Modifiers>(cooldownTimers.Keys);
        foreach (Modifiers modifier in keys)
        {
            cooldownTimers[modifier] -= Time.deltaTime;
            if (!(cooldownTimers[modifier] < 0)) continue;
            StopPotionEffect(modifier);
            cooldownTimers.Remove(modifier);
        }
    }

    private void StopPotionEffect(Modifiers modifiers)
    {
        foreach (var activeAdditiveModifierItem in modifiers.additiveModifiers)
        {
            additiveModifierList.Remove(activeAdditiveModifierItem);
            switch (activeAdditiveModifierItem.stat)
            {
                case Stat.Health:
                    health.SetNewMaxHealthOnHUD();
                    break;
                case Stat.Mana:
                    mana.SetNewMaxMana();
                    break;
            }
        }

        foreach (var activePercentageModifierItem in modifiers.percentageModifiers)
        {
            additiveModifierList.Remove(activePercentageModifierItem);
            switch (activePercentageModifierItem.stat)
            {
                case Stat.Health:
                    health.SetNewMaxHealthOnHUD();
                    break;
                case Stat.Mana:
                    mana.SetNewMaxMana();
                    break;
            }
        }
    }

    public void Setup(float impactTime, PotionItem.Modifier[] additiveModifiers,
        PotionItem.Modifier[] percentageModifiers)
    {
        additiveModifierList.AddRange(additiveModifiers);
        percentageModifierList.AddRange(percentageModifiers);
        foreach (var modifier in additiveModifiers)
        {
            switch (modifier.stat)
            {
                case Stat.Health:
                    health.SetNewMaxHealthOnHUD();
                    break;
                case Stat.Mana:
                    mana.SetNewMaxMana();
                    break;
            }
        }

        foreach (var modifier in percentageModifiers)
        {
            switch (modifier.stat)
            {
                case Stat.Health:
                    health.SetNewMaxHealthOnHUD();
                    break;
                case Stat.Mana:
                    mana.SetNewMaxMana();
                    break;
            }
        }

        var modifiers = new Modifiers
        {
            additiveModifiers = additiveModifiers,
            percentageModifiers = percentageModifiers
        };
        cooldownTimers[modifiers] = impactTime;
    }

    public IEnumerable<float> GetAdditiveModifiers(Stat stat)
    {
        foreach (var modifier in additiveModifierList)
        {
            if (modifier.stat == stat)
            {
                yield return modifier.value;
            }
        }
    }

    public IEnumerable<float> GetPercentageModifiers(Stat stat)
    {
        foreach (var modifier in percentageModifierList)
        {
            if (modifier.stat == stat)
            {
                yield return modifier.value;
            }
        }
    }
}