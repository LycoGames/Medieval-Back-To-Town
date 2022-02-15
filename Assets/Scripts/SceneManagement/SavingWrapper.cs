using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using UnityEngine.InputSystem;


public class SavingWrapper : MonoBehaviour
{
    const string defaultSaveFile = "save";
    [SerializeField] float fadeInTime = 0.2f;

    private void Awake()
    {
        LoadLastScene();
    }

    private void LoadLastScene()
    {
      //  GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
    }

    void Update()
    {

        if (Keyboard.current.lKey.isPressed)
        {
            Load();
        }
        if (Keyboard.current.kKey.isPressed)
        {
            Save();
        }
        if (Keyboard.current.deleteKey.isPressed)
        {
            Delete();
        }
    }

    public void Save()
    {
        GetComponent<SavingSystem>().Save(defaultSaveFile);
    }

    public void Load()
    {
        GetComponent<SavingSystem>().Load(defaultSaveFile);
    }

    public void Delete()
    {
        GetComponent<SavingSystem>().Delete(defaultSaveFile);
    }
}
