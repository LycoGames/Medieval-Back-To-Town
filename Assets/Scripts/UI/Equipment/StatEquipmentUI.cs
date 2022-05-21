using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StatEquipmentUI : MonoBehaviour
{
    [SerializeField] private Stat stat;
    [SerializeField] private Text value;

    private BaseStats stats;
    private UnityEvent updateEvent;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        stats = player.GetComponent<BaseStats>();
        updateEvent = player.GetComponent<Equipment>().updateEquipmentUiStatsEvent;
        updateEvent.AddListener(UpdateUi);
        UpdateUi();
    }

    private void UpdateUi()
    {
        value.text = stats.GetStat(stat).ToString(CultureInfo.InvariantCulture);
    }
}