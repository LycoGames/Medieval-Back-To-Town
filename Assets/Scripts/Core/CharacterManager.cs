using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private GameObject CoreArcher;
    [SerializeField] private GameObject CoreWarrior;

    private void Awake()
    {
        string selectedCharacterString = PlayerPrefs.GetString("CharacterPref");
        switch (selectedCharacterString)
        {
            case "Archer":
                Instantiate(CoreArcher, Vector3.zero, Quaternion.identity);
                break;
            case "Warrior":
                Instantiate(CoreWarrior, Vector3.zero, Quaternion.identity);
                break;
        }
    }
}