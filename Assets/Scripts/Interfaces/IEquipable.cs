using Inventory.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interfaces
{
    public interface IEquipable
    {
        public void EquipItem(GameObject character, InventorySO mainInventory, int index, string containerType);
    }
}