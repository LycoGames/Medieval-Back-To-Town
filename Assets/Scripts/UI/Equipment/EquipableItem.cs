using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Equipable Item"))]
public class EquipableItem : InventoryItem
{

    [Tooltip("Where are we allowed to put this item.")]
    [SerializeField] EquipLocation allowedEquipLocation = EquipLocation.PrimaryWeapon;


    public EquipLocation GetAllowedEquipLocation()
    {
        return allowedEquipLocation;
    }
}
