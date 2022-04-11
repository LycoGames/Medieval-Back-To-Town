using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ItemTooltip : MonoBehaviour
{
    // CONFIG DATA
    [SerializeField] TextMeshProUGUI titleText = null;
    [SerializeField] TextMeshProUGUI bodyText = null;

    // PUBLIC

    public void Setup(InventoryItem item)
    {
        titleText.text = item.GetDisplayName();
        bodyText.text = item.GetDescription();
    }
}