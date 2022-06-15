using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IUsable
{
    
    public void UseItem(GameObject character, InventorySO mainInventory, int index);
    public void DeleteUsedItem(InventorySO mainInventory, int index, int quantity);
}
