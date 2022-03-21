using System;
using System.Collections;
using System.Collections.Generic;
using InputSystem;
using UnityEngine;

public class AIConversant : MonoBehaviour
{
    [SerializeField] private string conversantName;
    [SerializeField] private Dialogue dialogue = null;
    [SerializeField] private bool repeatableDialogue = false;
    private bool interactable = true;

    //private GameObject player;
    //private Inputs input;

    //private PlayerConversant playerConversant;
    //private StateMachine stateMachine;

    /*private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        input = player.GetComponent<Inputs>();
        playerConversant = player.GetComponent<PlayerConversant>();
        stateMachine = player.GetComponent<StateMachine>();
    }*/

    /* private void Update()
     {
         //StartConservation();
     }*/

    /*public void StartConservation()
    {
        if (playerConversant.GetInteractableNPC() == this && input.interaction)
        {
            playerConversant.StartDialogue(dialogue);
            stateMachine.LockCameraPosition = true;
            stateMachine.CanMove = false;
            stateMachine.AimUI.SetActive(false);
        }
    }*/

    public Dialogue GetDialogue()
    {
        return dialogue;
    }

    public string GetConversantName()
    {
        return conversantName;
    }

    public bool CanInteractable()
    {
        return interactable;
    }

    public void SetInteractable(bool state)
    {
        interactable = state;
    }

    public bool IsRepeatableDialogue()
    {
        return repeatableDialogue;
    }
}