using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIConversant : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue = null;

    private void Update()
    {
    }

    public bool StartConservation()
    {
        if (dialogue == null)
        {
            return false;
        }

        return true;
    }
}