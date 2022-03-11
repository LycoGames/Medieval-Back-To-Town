using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : ScriptableObject
{
    public new string name;
    public float cooldownTime;
    public float activeTime;

    public virtual void Active(GameObject parent){}
    
}
