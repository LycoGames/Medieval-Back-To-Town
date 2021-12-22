using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

public class SavingWrapper : MonoBehaviour
{
    [SerializeField] KeyCode saveKey = KeyCode.K;
    [SerializeField] KeyCode loadKey = KeyCode.L;
    [SerializeField] KeyCode deleteKey = KeyCode.Delete;
    const string defaultSaveFile = "save";

    private void Awake()
    {
//        StartCoroutine(LoadLastScene());
    }

    private IEnumerator LoadLastScene()
    {
        yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(saveKey))
            Save();
        if (Input.GetKeyDown(loadKey))
            Load();
        if (Input.GetKeyDown(deleteKey))
            Delete();
    }

    private void Save()
    {
        GetComponent<SavingSystem>().Save(defaultSaveFile);
    }

    private void Load()
    {
        StartCoroutine(GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile));
    }

    private void Delete()
    {
        GetComponent<SavingSystem>().Delete(defaultSaveFile);
    }
}
