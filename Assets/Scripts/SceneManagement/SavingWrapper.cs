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
        StartCoroutine(LoadLastScene());
    }

    private IEnumerator LoadLastScene()
    {
        yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
    }

    void Update()
    {
        if (Keyboard.current.bKey.isPressed)
        {
            Load();
        }

        if (Keyboard.current.nKey.isPressed)
        {
            Save();
        }

        if (Keyboard.current.deleteKey.isPressed)
        {
            Delete();
        }
    }

    public void Load()
    {
        StartCoroutine(GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile));
    }
    public void Save()
    {
        GetComponent<SavingSystem>().Save(defaultSaveFile);
    }
    public void Delete()
    {
        GetComponent<SavingSystem>().Delete(defaultSaveFile);
    }
}