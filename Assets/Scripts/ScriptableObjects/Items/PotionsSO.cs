using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Inventory.SO
{
    [CreateAssetMenu(fileName = "New Potion", menuName = "Inventory/Items/Potion Item")]
    public class PotionsSO : ItemSO//, //IUsable, IRemovable, IRemovableQuantity, IQuickEquipable, IQuickUnequipable, IUsableQuickSlot
    {
        [SerializeField] [NonReorderable] public List<ModifierType> modifierTypes = new List<ModifierType>();

        //public void UseItem(GameObject character, InventorySO mainInventory, int index)
        //{
        //    foreach (ModifierType modifier in modifierTypes)
        //    {
        //        modifier.Modifier.ApplyModifier(character, modifier.Value);
        //    }
        //    DeleteUsedItem(mainInventory, index, 1);
        //}
        //public void DeleteUsedItem(InventorySO mainInventory, int index, int quantity)
        //{
        //    mainInventory.RemoveItem(index, quantity);
        //}
        //public void UseItemInQuickSlot(GameObject character, InventorySO mainInventory, int index)
        //{
        //    foreach (ModifierType modifier in modifierTypes)
        //    {
        //        modifier.Modifier.ApplyModifier(character, modifier.Value);
        //    }
        //    DeleteUsedQuickSlotItem(mainInventory, index, 1);
        //}
        //public void DeleteUsedQuickSlotItem(InventorySO mainInventory, int index, int quantity)
        //{
        //    mainInventory.RemoveQuickSlotItem(index, quantity);
        //}
    }
}