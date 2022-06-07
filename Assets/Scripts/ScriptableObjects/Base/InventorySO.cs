using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MainInventory", menuName = "Base/Main Inventory")]

public class InventorySO : ScriptableObject
{
    [field: SerializeField] public bool IsSorted { get; set; }

    [SerializeField] [NonReorderable] public List<InventoryItem> inventoryItems;
    public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;
    public void StartInventoryInitSorting()
    {
        if (IsSorted)
            return;
        Debug.Log("STARTED INVENTORY FIRST SORTING");
        List<InventoryItem> collapsedList = new List<InventoryItem>();
        List<InventoryItem> sortedList = new List<InventoryItem>();
        int overStackItems;
        int toAddItemsStackable;
 
        foreach (InventoryItem baseItem in inventoryItems)
        {
            if (collapsedList.Count == 0)
            {
                collapsedList.Add(baseItem);
            }
            else
            {
                toAddItemsStackable = baseItem.quantity;
                int i = 0;
                while (toAddItemsStackable > 0 && i < collapsedList.Count)
                {
                    if (collapsedList[i].item.Name == baseItem.item.Name)
                    {
                        collapsedList[i] = new InventoryItem((collapsedList[i].quantity + toAddItemsStackable), collapsedList[i].item);
                        toAddItemsStackable = 0;
                    }
                    else
                    {
                        i++;
                    }
                }
                if (toAddItemsStackable > 0)
                {
                    collapsedList.Add(baseItem);
                }
            }
        }
        foreach (InventoryItem baseItem in collapsedList)
        {
            if (baseItem.quantity <= baseItem.item.MaxStackSize)
            {
                sortedList.Add(baseItem);
            }
            else
            {
                overStackItems = baseItem.quantity;
                while (overStackItems > baseItem.item.MaxStackSize)
                {
                    InventoryItem tmpItem1 = new InventoryItem(baseItem.item.MaxStackSize, baseItem.item);
                    sortedList.Add(tmpItem1);
                    overStackItems -= baseItem.item.MaxStackSize;
                }
                InventoryItem tmpItem2 = new InventoryItem(overStackItems, baseItem.item);
                sortedList.Add(tmpItem2);
            }
        }
        inventoryItems = sortedList;
        IsSorted = true;
    }
    public InventoryItem GetItemAt(int index)
    {
        Debug.Log($"index is {index}");
        return inventoryItems[index];
    }
    public string GetEquipSlotType(int index)
    {
        return inventoryItems[index].item.EquipSlot.ToString();
    }
    public void SwapItems(int index1, int index2)
    {
        InventoryItem tempItem = inventoryItems[index1];
        inventoryItems[index1] = inventoryItems[index2];
        inventoryItems[index2] = tempItem;
        InformAboutInventoryChange();
    }
    public void InformAboutInventoryChange()
    {
        OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
    }
    private Dictionary<int, InventoryItem> GetCurrentInventoryState()
    {
        Dictionary<int, InventoryItem> returnedDictionary = new Dictionary<int, InventoryItem>();
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            returnedDictionary[i] = inventoryItems[i];
        }
        return returnedDictionary;
    }
}



[Serializable]
public struct InventoryItem
{
    public int quantity;
    public ItemSO item;

    public InventoryItem(int quantity, ItemSO item)
    {
        this.quantity = quantity;
        this.item = item;
    }
}
