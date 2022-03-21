using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroGroup : MonoBehaviour
{
    [SerializeField] private Fighter[] fighters;
    [SerializeField] private bool activateOnStart = false;

    private void Start()
    {
        Activate(activateOnStart);
    }

    public void Activate(bool shouldActivate)
    {
        foreach (Fighter fighter in fighters)
        {
            //Herkesin saldırma scriptlerini aç
        }
    }
}