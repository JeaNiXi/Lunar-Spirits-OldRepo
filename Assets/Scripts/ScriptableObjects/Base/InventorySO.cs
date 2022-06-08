using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Base/Inventory")]
public class InventorySO : ScriptableObject
{
    public event Action OnInventoryUpdated;

    [SerializeField] [NonReorderable] private List<InventoryItem> Container = new List<InventoryItem>();
    public int GetInventorySize() => Container.Count;
    public List<InventoryItem> GetItemList() => Container;

    public void AddItem(ItemSO item, int quantity)
    {
        bool hasItem = false;
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == item)
            {
                Debug.Log(Container[i].item);
                Debug.Log(item);
                int toAdd = Container[i].quantity + quantity;
                Container[i] = new InventoryItem(item, toAdd);
                hasItem = true;
                break;
            }
        }
        if (!hasItem)
        {
            Container.Add(new InventoryItem(item, quantity));
        }
        Debug.Log(hasItem);
        InformUI();
    }
    private void InformUI()
    {
        OnInventoryUpdated?.Invoke();
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