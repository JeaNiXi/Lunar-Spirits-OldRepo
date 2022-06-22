using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Inventory.SO;

namespace Interfaces
{
    public interface IUsable
    {
        public void UseItem(GameObject character, InventorySO mainInventory, int index, string containerType);
        public void DeleteUsedItem(InventorySO mainInventory, int index, int quantity, string containerType);
    }
}