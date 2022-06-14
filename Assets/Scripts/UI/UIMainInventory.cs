using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainInventory : MonoBehaviour
{
    [SerializeField] private UIMainItem itemPrefab;
    [SerializeField] private UIItemActionPanel itemActionPanel;
    [SerializeField] private RectTransform contentPanel;

    public Action<int> 
        OnItemRMBClicked;

    private List<UIMainItem> uiItemsList = new List<UIMainItem>();

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
                uiItemsList.Add(uiItem);
            }
            else
            {
                UIMainItem uiItem = CreateItem();
                uiItem.transform.SetParent(contentPanel);
                uiItem.SetData(itemList[i].item.ItemImage, itemList[i].quantity);
                uiItem.OnItemRMBClicked += HandleRMBClick;
                uiItemsList.Add(uiItem);
            }
        }
    }



    private UIMainItem CreateItem() => Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
    public void UpdateInventoryUI(Dictionary<int, InventoryItem> newDictionary)
    {
        if (newDictionary.Count == uiItemsList.Count)
        {
            for (int i = 0; i < newDictionary.Count; i++)
            {
                if (newDictionary[i].IsEmpty)
                {
                    uiItemsList[i].SetData();
                }
                else
                {
                    uiItemsList[i].SetData(newDictionary[i].item.ItemImage, newDictionary[i].quantity);
                }
            }
            return;
        }
        else if (newDictionary.Count > uiItemsList.Count)
        {
            int startIndex = uiItemsList.Count;
            for (int i = 0; i < uiItemsList.Count; i++)
            {
                if (newDictionary[i].IsEmpty)
                {
                    uiItemsList[i].SetData();
                }
                else
                {
                    uiItemsList[i].SetData(newDictionary[i].item.ItemImage, newDictionary[i].quantity);
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
                    uiItemsList.Add(newItem);
                }
                else
                {
                    UIMainItem newItem = CreateItem();
                    newItem.transform.SetParent(contentPanel);
                    newItem.SetData(newDictionary[i].item.ItemImage, newDictionary[i].quantity);
                    newItem.OnItemRMBClicked += HandleRMBClick;
                    uiItemsList.Add(newItem);
                }
            }
            return;
        }
        else
        {
            int reminder = (uiItemsList.Count - newDictionary.Count) - 1;
            int startPoint = uiItemsList.Count - 1;
            for (int i = 0; i < newDictionary.Count; i++)
            {
                if (newDictionary[i].IsEmpty)
                {
                    uiItemsList[i].SetData();
                }
                else
                {
                    uiItemsList[i].SetData(newDictionary[i].item.ItemImage, newDictionary[i].quantity);
                }
            }
            for (int j = startPoint; j > reminder; j--)
            {
                uiItemsList[j].OnItemRMBClicked -= HandleRMBClick;
                uiItemsList[j].DeleteObject();
                uiItemsList.RemoveAt(j);
            }
        }
    }
    private void HandleRMBClick(UIMainItem obj)
    {
        int index = uiItemsList.IndexOf(obj);
        if (index == -1)
            return;
        OnItemRMBClicked?.Invoke(index);
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

    //public void ClearInventory()
    //{
    //    foreach(UIMainItem item in uiItemsList)
    //    {
    //        Destroy(item.gameObject);
    //    }
    //    uiItemsList.Clear();
    //}
}