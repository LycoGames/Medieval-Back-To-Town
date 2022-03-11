using System.Collections.Generic;
using UnityEngine;

public class CooldownStore : MonoBehaviour
{
    Dictionary<Ability, float> cooldownTimers = new Dictionary<Ability, float>();
    Dictionary<Ability, float> initialCooldownTimes = new Dictionary<Ability, float>();

    void Update()
    {
        var keys = new List<Ability>(cooldownTimers.Keys);
        foreach (Ability ability in keys)
        {
            cooldownTimers[ability] -= Time.deltaTime;
            if (cooldownTimers[ability] < 0)
            {
                cooldownTimers.Remove(ability);
                initialCooldownTimes.Remove(ability);
            }
        }
    }

    public void StartCooldown(Ability ability, float cooldownTime)
    {
        cooldownTimers[ability] = cooldownTime;
        initialCooldownTimes[ability] = cooldownTime;
    }

    public float GetTimeRemaining(Ability ability)
    {
        if (!cooldownTimers.ContainsKey(ability))
        {
            return 0;
        }

        return cooldownTimers[ability];
    }

    public float GetFractionRemaining(Ability ability)
    {
        if (ability == null)
        {
            return 0;
        }

        if (!cooldownTimers.ContainsKey(ability))
        {
            return 0;
        }

        return cooldownTimers[ability] / initialCooldownTimes[ability];
    }
}
