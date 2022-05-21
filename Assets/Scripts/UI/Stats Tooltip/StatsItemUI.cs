using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class StatsItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statName;
    [SerializeField] private TextMeshProUGUI value;

    public void Setup(string stat, float value, bool isPercentage)
    {
        statName.text = stat;
        if (isPercentage)
            this.value.text = "+" + value.ToString(CultureInfo.CurrentCulture) + "%";
        else
        {
            this.value.text = value.ToString(CultureInfo.CurrentCulture);
        }

    }
}