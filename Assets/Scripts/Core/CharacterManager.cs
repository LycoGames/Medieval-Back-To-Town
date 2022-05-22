using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private GameObject CoreArcher;
    [SerializeField] private GameObject CoreWarrior;
    [SerializeField] private Transform spawnTransfrom;

    private void Awake()
    {
        string selectedCharacterString = PlayerPrefs.GetString("CharacterPref");
        switch (selectedCharacterString)
        {
            case "Archer":
                Instantiate(CoreArcher, spawnTransfrom.position, spawnTransfrom.rotation);
                break;
            case "Warrior":
                Instantiate(CoreWarrior, spawnTransfrom.position, spawnTransfrom.rotation);
                break;
        }
    }
}