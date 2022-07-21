using Character;
using Inventory.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interfaces
{
    public interface IEquipable
    {
        public void EquipItem(CharacterManager character, InventorySO mainInventory, int index, string containerType);
        public void EquipItem(CharacterManager character);
    }
}