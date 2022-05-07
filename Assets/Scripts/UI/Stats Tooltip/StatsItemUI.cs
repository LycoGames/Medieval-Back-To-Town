using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class StatsItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statName;
    [SerializeField] private TextMeshProUGUI value;

    public void Setup(string stat, float value)
    {
        statName.text = stat;
        this.value.text = value.ToString(CultureInfo.CurrentCulture);
    }
}