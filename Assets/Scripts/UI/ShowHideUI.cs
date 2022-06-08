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
                Cursor.lockState = CursorLockMode.None; //HardCoding
                Cursor.visible = true;
            }
            else
            {
                warrior.CanAttack = true;
                warrior.LockCameraPosition = false;
                Cursor.lockState = CursorLockMode.None; //HardCoding
            }
        }
        else
        {
            Cursor.visible = true;
            if (player != null)
            {
                player.LockCameraPosition = true;
                player.AimUI.SetActive(false);
                Cursor.lockState = CursorLockMode.Confined; //HardCoding
            }
            else
            {
                warrior.CanAttack = false;
                warrior.LockCameraPosition = true;
                Cursor.lockState = CursorLockMode.Confined; //HardCoding
            }
        }

        uiContainer.SetActive(!uiContainer.activeSelf);
    }
}