using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Orient To Target Effect", menuName = "Abilities/Effects/Orient To Target", order = 0)]
public class OrientToTargetEffect : EffectStrategy
{
    public override void StartEffect(AbilityData data, Action finished)
    {
        // data.GetUser().transform.LookAt(data.GetTargetedPoint());
        Transform target = data.GetUser().GetComponent<ICommonFunctions>().GetTarget();
        data.GetUser().transform.LookAt(target);
        finished();
    }
}