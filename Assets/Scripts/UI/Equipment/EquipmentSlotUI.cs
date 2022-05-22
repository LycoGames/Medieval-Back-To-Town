using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
{
    // CONFIG DATA

    [SerializeField] InventoryItemIcon icon = null;
    [SerializeField] EquipLocation equipLocation = EquipLocation.PrimaryWeapon;
    [SerializeField] private CharacterClass[] allowedCharacterClasses;

    // CACHE
    Equipment playerEquipment;
    private GameObject player;

    // LIFECYCLE METHODS

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerEquipment = player.GetComponent<Equipment>();
        playerEquipment.equipmentUpdated += RedrawUI;
    }

    private void Start()
    {
        allowedCharacterClasses = new CharacterClass[1];
        if (player.GetComponent<StateMachine>() != null)
            allowedCharacterClasses[0] = CharacterClass.Archer;
        else
            allowedCharacterClasses[0] = CharacterClass.Warrior;
        RedrawUI();
    }

    // PUBLIC

    public int MaxAcceptable(InventoryItem item)
    {
        EquipableItem equipableItem = item as EquipableItem;
        if (equipableItem == null) return 0;
        if (equipableItem.GetAllowedEquipLocation() != equipLocation) return 0;
        if (GetItem() != null) return 0;
        //TODO 
        //if (!isSlotItemAllowedSpesificCharacterClasses) return 1;
        if (equipableItem.GetAllowedCharacterClasses().Length == 0)
            return 1;
        return equipableItem.GetAllowedCharacterClasses()
            .Any(characterClass => allowedCharacterClasses.Contains(characterClass))
            ? 1
            : 0;
    }

    public void AddItems(InventoryItem item, int number)
    {
        playerEquipment.AddItem(equipLocation, (EquipableItem) item);
    }

    public InventoryItem GetItem()
    {
        return playerEquipment.GetItemInSlot(equipLocation);
    }

    public int GetNumber()
    {
        if (GetItem() != null)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public void RemoveItems(int number)
    {
        playerEquipment.RemoveItem(equipLocation);
    }

    // PRIVATE

    void RedrawUI()
    {
        icon.SetItem(playerEquipment.GetItemInSlot(equipLocation));
    }
}