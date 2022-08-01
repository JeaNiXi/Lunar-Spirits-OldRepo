using Character;
using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.SO
{
    [CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Items/Equipment Item")]

    public class EquipmentSO : ItemSO, IEquipable
    {
        public void EquipItem(CharacterManager character, InventorySO mainInventory, int index, string containerType)
        {
            if (mainInventory.EquipItemHandler(index, containerType))
            {
                foreach (ModifierType modifier in ItemParameters.statModifiers)
                {
                    modifier.Modifier.ApplyModifier(character, modifier.Value);
                }
            }
        }

        public void EquipItem(CharacterManager character)
        {
            throw new System.NotImplementedException();
        }
    }
}