using System;
using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    public void Save()
    {
        SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
        savingWrapper.Save();
    }

    public void SaveAndQuit()
    {
        Save();
        SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
        savingWrapper.LoadMenu();
    }
}