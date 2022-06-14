using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRemovable
{
    public void RemoveItem(InventorySO mainInventory, int index);
}
