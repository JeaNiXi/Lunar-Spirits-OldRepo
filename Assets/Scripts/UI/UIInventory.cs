using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    [SerializeField] private UIItem itemPrefab;
    private List<UIItem> listOfUIItems = new List<UIItem>();
    public event Action OnDescriptionCloseRequest;
    public event Action<int> OnDescriptionRequest;

    public bool IsActive { get; private set; }

    public void ToggleInventory(bool state)
    {
        gameObject.SetActive(state);
        IsActive = state;
    }

    public void InitializeInventoryUI(InventorySO mainInventorySO, RectTransform contentRect)
    {
        for (int i = 0; i < mainInventorySO.inventoryItems.Count; i++)
        {
            UIItem newItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            newItem.transform.SetParent(contentRect);
            newItem.OnDescriptionRequested += HandleDescriptionRequest;
            newItem.OnDescriptionClosed += HandleDescriptionClosingRequest;
            //newItem.SetData(mainInventorySO.inventoryItems[i].item.ItemSprite, mainInventorySO.inventoryItems[i].quantity);
            listOfUIItems.Add(newItem);
        }
        UpdateInventoryUI(mainInventorySO);

    }

    private void HandleDescriptionClosingRequest(UIItem obj)
    {
        OnDescriptionCloseRequest?.Invoke();
    }

    private void HandleDescriptionRequest(UIItem obj)
    {
        int index = listOfUIItems.IndexOf(obj);
        if (index == -1)
            return;
        OnDescriptionRequest?.Invoke(index);
    }

    public void UpdateInventoryUI(InventorySO mainInventorySO)
    {
        for (int i = 0; i < listOfUIItems.Count; i++)
        {
            listOfUIItems[i].SetData(mainInventorySO.inventoryItems[i].item.ItemSprite, mainInventorySO.inventoryItems[i].quantity);
        }
    }
}
