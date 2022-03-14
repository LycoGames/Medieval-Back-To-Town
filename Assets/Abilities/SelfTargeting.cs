using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Self Targeting", menuName = "Abilities/Targeting/Self", order = 0)]
public class SelfTargeting : TargetingStrategy
{
    public override void StartTargeting(AbilityData data, Action finished)
    {
        data.SetTargets(new GameObject[] { data.GetUser() });
        data.SetTargetedPoint(data.GetUser().transform.position);
        finished();
    }
}
