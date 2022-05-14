using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menus;

    [SerializeField] private GameObject selectionUi;

    [SerializeField] private PlayableDirector playableDirector;

    private LazyValue<SavingWrapper> savingWrapper;

    private string newGameNameField;

    private bool isArcherSelected;

    [SerializeField] private Button selectArcherButton;
    [SerializeField] private Button selectWarriorButton;

    [SerializeField] private Animator archerAnim;
    [SerializeField] private Animator warriorAnim;


    private void Awake()
    {
        savingWrapper = new LazyValue<SavingWrapper>(GetSavingWrapper);
    }

    private void Start()
    {
        SetupUIs();
        playableDirector.stopped += StartSelectionState;
    }


    public void SwitchToCharacterSelection(string newGameNameField)
    {
        playableDirector.Play();
        this.newGameNameField = newGameNameField;
        CloseMenus();
    }

    public void NewGame()
    {
        if (newGameNameField == null)
            return;
        string characterPref = isArcherSelected ? "Archer" : "Warrior";
        PlayerPrefs.SetString("CharacterPref", characterPref);
        savingWrapper.value.NewGame(newGameNameField);
    }

    private SavingWrapper GetSavingWrapper()
    {
        return FindObjectOfType<SavingWrapper>();
    }

    private void CloseMenus()
    {
        if (menus.activeSelf)
            menus.SetActive(false);
    }

    private void StartSelectionState(PlayableDirector obj)
    {
        if (!selectionUi.activeSelf)
            selectionUi.SetActive(true);
        SelectRandomCharacter();
    }

    private void SelectRandomCharacter()
    {
        int randNumb = Random.Range(1, 100);
        if (randNumb >= 50)
        {
            SelectArcher();
            selectArcherButton.Select();
        }
        else
        {
            SelectWarrior();
            selectWarriorButton.Select();
        }
    }

    public void SelectArcher()
    {
        isArcherSelected = true;

        ResetTriggers();
        warriorAnim.SetTrigger("Deselect");
        archerAnim.SetTrigger("Select");
    }


    public void SelectWarrior()
    {
        isArcherSelected = false;

        ResetTriggers();
        archerAnim.SetTrigger("Deselect");
        warriorAnim.SetTrigger("Select");
    }

    private void ResetTriggers()
    {
        warriorAnim.ResetTrigger("Select");
        warriorAnim.ResetTrigger("Deselect");

        archerAnim.ResetTrigger("Select");
        archerAnim.ResetTrigger("Deselect");
    }

    private void SetupUIs()
    {
        menus.SetActive(true);
        selectionUi.SetActive(false);
    }
}