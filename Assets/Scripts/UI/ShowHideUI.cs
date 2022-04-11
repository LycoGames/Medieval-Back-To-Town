using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class ShowHideUI : MonoBehaviour
{
    [SerializeField] GameObject uiContainer = null;
    private StateMachine player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<StateMachine>();
        uiContainer.SetActive(false);
        Cursor.visible = false;
    }

    public void HandleShowHide()
    {
        if (uiContainer.activeSelf)
        {
            Cursor.visible = false;
            player.LockCameraPosition = false;
            player.AimUI.SetActive(true);
        }
        else
        {
            Cursor.visible = true;
            player.LockCameraPosition = true;
            player.AimUI.SetActive(false);
        }

        uiContainer.SetActive(!uiContainer.activeSelf);
    }
}