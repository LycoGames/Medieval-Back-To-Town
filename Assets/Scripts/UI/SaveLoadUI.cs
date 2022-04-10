using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadUI : MonoBehaviour
{
    [SerializeField] private Transform contentRoot;
    [SerializeField] private GameObject buttonPrefab;

    private void OnEnable()
    {
        SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
        if (!savingWrapper) return;
        foreach (Transform child in contentRoot)
        {
            Destroy(child.gameObject);
        }

        foreach (string save in savingWrapper.ListSaves())
        {
            GameObject buttonInstance = Instantiate(buttonPrefab, contentRoot);
            buttonInstance.GetComponentInChildren<TMP_Text>().text = save;
            Button button = buttonInstance.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => { savingWrapper.LoadGame(save); });
        }
    }
}