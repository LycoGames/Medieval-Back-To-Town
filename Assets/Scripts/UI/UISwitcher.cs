using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISwitcher : MonoBehaviour
{
    [SerializeField] private GameObject entryPoint;

    private void Start()
    {
        SwitchTo(entryPoint);
    }

    public void SwitchTo(GameObject toDisplay)
    {
        if (toDisplay.transform.parent != transform) return;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(child.gameObject == toDisplay);
        }
    }
}