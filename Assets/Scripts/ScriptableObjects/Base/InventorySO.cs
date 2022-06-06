using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MainInventory", menuName = "Base/Main Inventory")]

public class InventorySO : ScriptableObject
{
    [SerializeField] [NonReorderable] public List<InventoryItem> inventoryItems;

    public void LoadInventory()
    {
        
    }
    public InventoryItem GetItemAt(int index)
    {
        Debug.Log($"index is {index}");
        return inventoryItems[index];
    }
}



[Serializable]
public struct InventoryItem
{
    public int quantity;
    public ItemSO item;
}
