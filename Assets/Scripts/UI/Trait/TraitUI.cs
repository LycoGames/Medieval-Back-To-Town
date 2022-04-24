using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TraitUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI unassignedPointsText;
    [SerializeField] Button commitButton;

    TraitStore playerTraitStore = null;

    Mana mana;
    Health health;


    void Start()
    {
        playerTraitStore = GameObject.FindGameObjectWithTag("Player").GetComponent<TraitStore>();
        mana = GameObject.FindGameObjectWithTag("Player").GetComponent<Mana>();
        health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        commitButton.onClick.AddListener(playerTraitStore.Commit); //onConfirm 
        commitButton.onClick.AddListener(mana.SetNewMaxMana);
        commitButton.onClick.AddListener(health.SetNewMaxHealthOnHUD);
    }

    void Update()
    {
        unassignedPointsText.text = playerTraitStore.GetUnassignedPoints().ToString();
    }

}