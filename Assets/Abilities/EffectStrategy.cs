using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectStrategy : ScriptableObject
{
    public abstract void StartEffect(AbilityData data, Action finished);
}