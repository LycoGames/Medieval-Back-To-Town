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

    public virtual void Setup(InventoryItem item)
    {
        titleText.text = item.GetDisplayName();
        bodyText.text = item.GetDescription();

        StatsEquipableItem equipableItem = item as StatsEquipableItem;
        if (equipableItem == null)
            return;
        GetComponentInChildren<StatsListUI>().SetItem(equipableItem);
    }
}