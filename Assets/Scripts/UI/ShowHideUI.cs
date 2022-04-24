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
    private WStateMachine warrior;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<StateMachine>();
        if (player == null)
            warrior = GameObject.FindGameObjectWithTag("Player").GetComponent<WStateMachine>();
        uiContainer.SetActive(false);
        Cursor.visible = false;
    }

    public void HandleShowHide()
    {
        if (uiContainer.activeSelf)
        {
            Cursor.visible = false;
            if (player != null)
            {
                player.LockCameraPosition = false;
                player.AimUI.SetActive(true);
            }
            else
            {
                warrior.LockCameraPosition = false;
            }
        }
        else
        {
            Cursor.visible = true;
            if (player != null)
            {
                player.LockCameraPosition = true;
                player.AimUI.SetActive(false);
            }
            else
            {
                warrior.LockCameraPosition = true;
            }
        }

        uiContainer.SetActive(!uiContainer.activeSelf);
    }
}