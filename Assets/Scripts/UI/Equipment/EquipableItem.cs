using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Equipable Item"))]
public class EquipableItem : InventoryItem
{
    [Tooltip("Where are we allowed to put this item.")] [SerializeField]
    EquipLocation allowedEquipLocation = EquipLocation.PrimaryWeapon;

    [Tooltip("Select if only spesific classes use this item")] [SerializeField]
    CharacterClass[] allowedCharacterClasses;


    public EquipLocation GetAllowedEquipLocation()
    {
        return allowedEquipLocation;
    }

    public CharacterClass[] GetAllowedCharacterClasses()
    {
        return allowedCharacterClasses;
    }
}