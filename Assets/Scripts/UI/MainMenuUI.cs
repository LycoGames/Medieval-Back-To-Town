using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    private LazyValue<SavingWrapper> savingWrapper;

    [SerializeField] private TMP_InputField newGameNameField;

    private void Awake()
    {
        savingWrapper = new LazyValue<SavingWrapper>(GetSavingWrapper);
    }

    private SavingWrapper GetSavingWrapper()
    {
        return FindObjectOfType<SavingWrapper>();
    }

    public void ContinueGame()
    {
        savingWrapper.value.ContinueGame();
    }

    public void NewGame()
    {
        if (newGameNameField.text == String.Empty)
        {
            newGameNameField.text = "This field is required...";
            return;
        }

        FindObjectOfType<MainMenuManager>().SwitchToCharacterSelection(newGameNameField.text);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
    }
}