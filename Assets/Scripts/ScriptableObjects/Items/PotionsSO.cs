using Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Inventory.SO
{
    [CreateAssetMenu(fileName = "New Potion", menuName = "Inventory/Items/Potion Item")]
    public class PotionsSO : ItemSO, IUsable, IQuickEquipable, IRemovable, IRemovableQuantity
    {
        [SerializeField] [NonReorderable] public List<ModifierType> modifierTypes = new List<ModifierType>();

        public void UseItem(GameObject character, InventorySO mainInventory, int index, string containerType)
        {
            foreach (ModifierType modifier in modifierTypes)
            {
                modifier.Modifier.ApplyModifier(character, modifier.Value);
            }
            DeleteUsedItem(mainInventory, index, 1, containerType);
        }
        public void DeleteUsedItem(InventorySO mainInventory, int index, int quantity, string containerType)
        {
            mainInventory.RemoveItem(index, quantity, containerType);
        }
    }
}