using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Base/Inventory")]
public class InventorySO : ScriptableObject
{
    public event Action OnInventoryUpdated;
    [SerializeField] [NonReorderable] private List<InventoryItem> Container = new List<InventoryItem>(24);
    private int Rows { get => Container.Count / 6; }
    private const int MAX_ITEM_SLOTS = 42;
    public int GetInventorySize() => Container.Count;
    public List<InventoryItem> GetItemList() => Container;

    public void AddItem(ItemSO item, int quantity)
    {
        if (item.MaxStackSize > 1)
        {
            int index;
            int sizeToAdd = quantity;
            while (sizeToAdd > 0)
            {
                if (SearchForItemStackable(item, out index))
                {
                    if (Container[index].quantity + quantity > Container[index].item.MaxStackSize)
                    {
                        int maxStackSize = Container[index].item.MaxStackSize;
                        sizeToAdd = quantity - (Container[index].item.MaxStackSize - Container[index].quantity);
                        Container[index] = new InventoryItem(item, maxStackSize);
                    }
                    else
                    {
                        int addSize = Container[index].quantity + sizeToAdd;
                        Container[index] = new InventoryItem(item, addSize);
                        sizeToAdd = 0;
                    }
                }
                else
                {
                    if (SearchForEmptySlot(out index))
                    {
                        if (sizeToAdd > item.MaxStackSize)
                        {
                            Container[index] = new InventoryItem(item, item.MaxStackSize);
                            sizeToAdd -= item.MaxStackSize;
                        }
                        else
                        {
                            Container[index] = new InventoryItem(item, sizeToAdd);
                            sizeToAdd = 0;
                        }
                    }
                    else
                    {
                        //Should be remade. No Empty Slots, nowhere to stack. Debug Inventory Full!
                        if (sizeToAdd > item.MaxStackSize)
                        {
                            Container.Add(new InventoryItem(item, item.MaxStackSize));
                            sizeToAdd -= item.MaxStackSize;
                        }
                        else
                        {
                            Container.Add(new InventoryItem(item, sizeToAdd));
                            sizeToAdd = 0;
                        }
                    }
                }
            }
        }
        else
        {
            int index;
            int sizeToAdd = quantity;
            while (sizeToAdd > 0)
            {
                if (SearchForEmptySlot(out index))
                {
                    Container[index] = new InventoryItem(item, item.MaxStackSize);
                    sizeToAdd -= item.MaxStackSize;
                }
                else
                {
                    //Should be remade. No Empty Slots, nowhere add. Debug Inventory Full!
                    Container.Add(new InventoryItem(item, item.MaxStackSize));
                    sizeToAdd -= item.MaxStackSize;
                }
            }
        }
        InformUI();
    }
    public void RemoveItem(ItemSO item, int quantity, int index)
    {

    }
    public void RemoveItem(ItemSO item, int quantity)
    {

    }
    public void RemoveItem(ItemSO item)
    {

    }
    public void RemoveItem(int index, int quantity)
    {

    }
    public void RemoveItem(int index)
    {

    }
    public void RemoveItem(string itemType, string itemName)
    {

    }
    public void RemoveItem(string itemType)
    {
        if (SearchForItem(itemType))
        {
            for (int i = 0; i < Container.Count; i++)
            {
                if (Container[i].IsEmpty)
                    continue;
                if (Container[i].item.ItemType.ToString() == itemType)
                {
                    Container[i] = InventoryItem.GetEmptyItem();
                }
            }
            InformUI();
        }
        else
        {
            Debug.LogError("NO ITEM FOUND");
        }
    }
    private bool SearchForItem(string itemType)
    {
        for (int i = 0; i < Container.Count; i++)
        {
            if (!Container[i].IsEmpty && Container[i].item.ItemType.ToString() == itemType)
            {
                return true;
            }
        }
        return false;
    }
    private bool SearchForItemStackable(ItemSO item, out int index)
    {
        for (int i = 0; i < Container.Count; i++)
        {
            if (!Container[i].IsEmpty && Container[i].item == item && Container[i].quantity < Container[i].item.MaxStackSize)
            {
                index = i;
                return true;
            }
        }
        index = -1;
        return false;
    }
    private bool SearchForEmptySlot(out int index)
    {
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].IsEmpty)
            {
                index = i;
                return true;
            }
        }
        index = -1;
        return false;
    }
    private void InformUI()
    {
        CheckForInventoryGridEnd();
        OnInventoryUpdated?.Invoke();
    }
    public void CorrectQuantity()
    {
        if (Container.Count > MAX_ITEM_SLOTS)
        {
            int startIndex = Container.Count - 1;
            int reminder = Container.Count - (Container.Count - MAX_ITEM_SLOTS);
            for (int i = startIndex; i >= reminder; i--)
            {
                Container.RemoveAt(i);
            }
        }
        for (int i = 0; i < Container.Count; i++)
        {
            if (!Container[i].IsEmpty && Container[i].quantity > Container[i].item.MaxStackSize)
            {
                ItemSO tempItem = Container[i].item;
                Container[i] = new InventoryItem(tempItem, tempItem.MaxStackSize);
            }
        }
    }
    public void CheckForInventoryGridEnd()
    {
        // Making grid end perfectly.
        if (Container.Count % 6 != 0)
        {
            int iterations = (((Container.Count / 6) + 1) * 6) - Container.Count;
            for (int i = 0; i < iterations; i++)
            {
                Container.Add(InventoryItem.GetEmptyItem());
            }
        }
        // Cheking if rows number should be increased.
        for (int i = Container.Count - 1; i > Container.Count - 7; i--)
        {
            if (Rows < 7)
            {
                if (!Container[i].IsEmpty)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        Container.Add(InventoryItem.GetEmptyItem());
                    }
                }
            }
            else
            {
                break;
            }
        }
        // Cheking if rows number should be decreased. (Maybe should disable if increase was true?)
        bool stopSearch = false;
        do
        {
            Debug.Log(Rows);
            //Rows = Container.Count / 6;
            if (Rows > 4)
            {
                for (int i = Container.Count - 1; i > Container.Count - 13; i--)
                {
                    if (!Container[i].IsEmpty)
                    {
                        return;
                    }
                }
                int startPoint = Container.Count - 1;
                for (int j = startPoint; j > startPoint - 6; j--)
                {
                    Container.RemoveAt(Container.Count - 1);
                }
            }
            else
            {
                stopSearch = true;
            }
        }
        while (!stopSearch);
    }
    public Dictionary<int, InventoryItem> GetCurrentInventoryState()
    {
        Dictionary<int, InventoryItem> dictionary = new Dictionary<int, InventoryItem>();
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].IsEmpty)
                dictionary[i] = InventoryItem.GetEmptyItem();
            else
                dictionary[i] = Container[i];
        }
        return dictionary;
    }
}


[Serializable]
public struct InventoryItem
{
    // Is a property. Returning True if item==null, or False if not.
    public bool IsEmpty => item == null || quantity <= 0;

    public ItemSO item;
    public int quantity;

    public InventoryItem(ItemSO item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
    public static InventoryItem GetEmptyItem()
    {
        return new InventoryItem
        {
            item = null,
            quantity = 0,
        };
    }
}