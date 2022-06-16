using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainInventory : MonoBehaviour
{
    [SerializeField] private UIMainItem itemPrefab;
    [SerializeField] private UIItemActionPanel itemActionPanel;
    [SerializeField] private UIConfirmAllPanel confirmationPanel;
    [SerializeField] private UIConfirmQuantityPanel confirmationQuantityPanel;
    [SerializeField] private UIConfirmQuickSlotPanel confirmationQuickSlotsPanel;
    [SerializeField] private RectTransform contentPanel;
    [SerializeField] private RectTransform quickSlotPanel;

    public Action<int>
        OnItemRMBClicked,
        OnQuickSlotItemRMBClicked,
        //OnItemLMBClicked,
        //OnQuickSlotItemLMBClicked,
        OnQuickSlotItemUnequipConfirmed,
        OnRemoveAllConfirmed;
    public Action<int, int>
        OnRemoveQuantityConfirmed,
        OnQuickSlotEquipConfirmed;

    private List<UIMainItem> uiItemsList = new List<UIMainItem>();
    private List<UIMainItem> uiQuickSlotsItems = new List<UIMainItem>();

    public void SetInventoryActive(bool value)
    {
        gameObject.SetActive(value);
    }
    public void InitializeInventoryData(List<InventoryItem> itemList)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].IsEmpty)
            {
                UIMainItem uiItem = CreateItem();
                uiItem.transform.SetParent(contentPanel);
                uiItem.SetData();
                uiItem.OnItemRMBClicked += HandleRMBClick;
                uiItem.OnItemLMBClicked += HandleLMBClick;
                uiItemsList.Add(uiItem);
            }
            else
            {
                UIMainItem uiItem = CreateItem();
                uiItem.transform.SetParent(contentPanel);
                uiItem.SetData(itemList[i].item.ItemImage, itemList[i].quantity, UIMainItem.SlotType.MAIN);
                uiItem.OnItemRMBClicked += HandleRMBClick;
                uiItem.OnItemLMBClicked += HandleLMBClick;
                uiItemsList.Add(uiItem);
            }
        }
    }
    public void InitializeQuickSlotsData(List<QuickSlotItem> quickSlotItemList)
    {
        for (int i = 0; i < quickSlotItemList.Count; i++)
        {
            if(quickSlotItemList[i].IsEmpty)
            {
                UIMainItem uiItem = CreateItem();
                uiItem.transform.SetParent(quickSlotPanel);
                uiItem.SetData();
                uiItem.OnItemRMBClicked += HandleRMBClick;
                uiItem.OnItemLMBClicked += HandleLMBClick;
                uiQuickSlotsItems.Add(uiItem);
            }
            else
            {
                UIMainItem uiItem = CreateItem();
                uiItem.transform.SetParent(quickSlotPanel);
                uiItem.SetData(quickSlotItemList[i].item.ItemImage, quickSlotItemList[i].quantity, UIMainItem.SlotType.QUICK_SLOT);
                uiItem.OnItemRMBClicked += HandleRMBClick;
                uiItem.OnItemLMBClicked += HandleLMBClick;
                uiQuickSlotsItems.Add(uiItem);
            }
        }
    }


    private UIMainItem CreateItem() => Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
    public void DeselectAllItems()
    {
        Debug.Log("deselect called");
        for (int i = 0; i < uiItemsList.Count; i++)
        {
            uiItemsList[i].DeselectItem();
        }
        for (int j = 0; j < uiQuickSlotsItems.Count; j++)
        {
            uiQuickSlotsItems[j].DeselectItem();
        }
    }
    public void UpdateInventoryUI(Dictionary<int, InventoryItem> newDictionary)
    {
        if (newDictionary.Count == uiItemsList.Count)
        {
            DeselectAllItems();
            for (int i = 0; i < newDictionary.Count; i++)
            {
                if (newDictionary[i].IsEmpty)
                {
                    uiItemsList[i].SetData();
                }
                else
                {
                    uiItemsList[i].SetData(newDictionary[i].item.ItemImage, newDictionary[i].quantity, UIMainItem.SlotType.MAIN);
                }
            }
            return;
        }
        else if (newDictionary.Count > uiItemsList.Count)
        {
            DeselectAllItems();
            int startIndex = uiItemsList.Count;
            for (int i = 0; i < uiItemsList.Count; i++)
            {
                if (newDictionary[i].IsEmpty)
                {
                    uiItemsList[i].SetData();
                }
                else
                {
                    uiItemsList[i].SetData(newDictionary[i].item.ItemImage, newDictionary[i].quantity, UIMainItem.SlotType.MAIN);
                }
            }
            for (int i = startIndex; i < newDictionary.Count; i++)
            {
                if (newDictionary[i].IsEmpty)
                {
                    UIMainItem newItem = CreateItem();
                    newItem.transform.SetParent(contentPanel);
                    newItem.SetData();
                    newItem.OnItemRMBClicked += HandleRMBClick;
                    newItem.OnItemLMBClicked += HandleLMBClick;
                    uiItemsList.Add(newItem);
                }
                else
                {
                    UIMainItem newItem = CreateItem();
                    newItem.transform.SetParent(contentPanel);
                    newItem.SetData(newDictionary[i].item.ItemImage, newDictionary[i].quantity, UIMainItem.SlotType.MAIN);
                    newItem.OnItemRMBClicked += HandleRMBClick;
                    newItem.OnItemLMBClicked += HandleLMBClick;
                    uiItemsList.Add(newItem);
                }
            }
            return;
        }
        else
        {
            DeselectAllItems();
            int reminder = newDictionary.Count - 1;
            int startPoint = uiItemsList.Count - 1;
            for (int i = 0; i < newDictionary.Count; i++)
            {
                if (newDictionary[i].IsEmpty)
                {
                    uiItemsList[i].SetData();
                }
                else
                {
                    uiItemsList[i].SetData(newDictionary[i].item.ItemImage, newDictionary[i].quantity, UIMainItem.SlotType.MAIN);
                }
            }
            for (int j = startPoint; j > reminder; j--)
            {
                uiItemsList[j].OnItemRMBClicked -= HandleRMBClick;
                uiItemsList[j].OnItemLMBClicked -= HandleLMBClick;
                uiItemsList[j].DeleteObject();
                uiItemsList.RemoveAt(j);
            }
        }
    }
    public void UpdateQuickSlotsUI(List<QuickSlotItem> quickSlotsList)
    {
        for (int i = 0; i < quickSlotsList.Count; i++) 
        {
            if (quickSlotsList[i].IsEmpty)
            {
                uiQuickSlotsItems[i].SetData();
            }
            else
            {
                uiQuickSlotsItems[i].SetData(quickSlotsList[i].item.ItemImage, quickSlotsList[i].quantity, UIMainItem.SlotType.QUICK_SLOT);
            }
        }
        DeselectAllItems();
    }
    private void HandleRMBClick(UIMainItem obj)
    {
        if (obj.mainSlotType == UIMainItem.SlotType.MAIN)
        {
            int index = uiItemsList.IndexOf(obj);
            if (index == -1)
                return;
            OnItemRMBClicked?.Invoke(index);
        }
        if(obj.mainSlotType == UIMainItem.SlotType.QUICK_SLOT)
        {
            int index = uiQuickSlotsItems.IndexOf(obj);
            if (index == -1)
                return;
            OnQuickSlotItemRMBClicked?.Invoke(index);
        }
    }
    private void HandleLMBClick(UIMainItem obj)
    {
        if (obj.mainSlotType == UIMainItem.SlotType.MAIN)
        {
            int index = uiItemsList.IndexOf(obj);
            if (index == -1)
                return;
            DeselectAllItems();
            obj.SelectItem();
            //OnItemLMBClicked?.Invoke(index);
        }
        if (obj.mainSlotType == UIMainItem.SlotType.QUICK_SLOT)
        {
            int index = uiQuickSlotsItems.IndexOf(obj);
            if (index == -1)
                return;
            DeselectAllItems();
            obj.SelectItem();
            //OnQuickSlotItemLMBClicked?.Invoke(index);
        }
    }
    public void ToggleActionPanel(bool value)
    {
        itemActionPanel.TogglePanel(value);
    }
    public void AddButton(string name, Action onClickAction)
    {
        itemActionPanel.AddButton(name, onClickAction);
    }


    public bool IsPanelActive()
    {
        return itemActionPanel.IsPanelActive();
    }
    public bool IsConfirmationPanelActive()
    {
        return confirmationPanel.IsConfirmationPanelActive();
    }
    public bool IsConfirmationQuantityPanelActive()
    {
        return confirmationQuantityPanel.IsConfirmationQuantityPanelActive();
    }
    public bool IsQuickSlotSelectionPanelActive()
    {
        return confirmationPanel.IsConfirmationPanelActive();
    }


    public void ToggleConfirmationPanel(bool value, int index)
    {
        confirmationPanel.ToggleConfirmationPanel(value, index);
    }
    public void ConfirmRemovingAll(int index)
    {
        confirmationPanel.ToggleConfirmationPanel(false, -1);
        OnRemoveAllConfirmed?.Invoke(index);
    }
    public void ToggleConfirmQuantityPanel(bool value, int index, int maxRemoveSize)
    {
        confirmationQuantityPanel.ToggleConfirmQuantityPanel(value, index, maxRemoveSize);
    }
    public void ConfirmRemovingQuantity(int index, int quantity)
    {
        confirmationQuantityPanel.ToggleConfirmQuantityPanel(false, -1, -1);
        if (quantity == 0)
            return;
        else
            OnRemoveQuantityConfirmed?.Invoke(index, quantity);
    }
    public void ToggleConfirmQuickSlotPanel(bool value, int index)
    {
        confirmationQuickSlotsPanel.ToggleConfirmQuantityPanel(value, index);
    }
    public void ConfirmQuickSlotEquip(int index, int slotIndex)
    {
        confirmationQuickSlotsPanel.ToggleConfirmQuantityPanel(false, -1);
        OnQuickSlotEquipConfirmed?.Invoke(index, slotIndex);
    }

    
    public void UnequipQuickSlot(int index)
    {
        OnQuickSlotItemUnequipConfirmed?.Invoke(index);
    }

    //public void ClearInventory()
    //{
    //    foreach(UIMainItem item in uiItemsList)
    //    {
    //        Destroy(item.gameObject);
    //    }
    //    uiItemsList.Clear();
    //}
}