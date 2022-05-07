using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IItemHolder))]
public class ItemTooltipSpawner : TooltipSpawner
{
    public override bool CanCreateTooltip()
    {
        var item = GetComponent<IItemHolder>().GetItem();
        if (!item) return false;

        return true;
    }

    public override void UpdateTooltip(GameObject tooltip)
    {
        var itemTooltip = tooltip.GetComponent<ItemTooltip>();
        if (!itemTooltip) return;

        var item = GetComponent<IItemHolder>().GetItem();
        var equipableItemTooltip = itemTooltip as EquipableItemTooltip;
        if (equipableItemTooltip != null) equipableItemTooltip.Setup(item);
        else
            itemTooltip.Setup(item);
    }
}