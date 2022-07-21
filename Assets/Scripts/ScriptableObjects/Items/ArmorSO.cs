using Character;
using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.SO
{
    [CreateAssetMenu(fileName = "New Armor", menuName = "Inventory/Items/Armor Item")]

    public class ArmorSO : ItemSO, IEquipable
    {
        [SerializeField] [NonReorderable] public List<ModifierType> modifierTypes = new List<ModifierType>();

        public void EquipItem(CharacterManager character, InventorySO mainInventory, int index, string containerType)
        {
            if (mainInventory.EquipItemHandler(index, containerType))
            {
                foreach (ModifierType modifier in modifierTypes)
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