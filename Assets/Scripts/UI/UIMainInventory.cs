using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainInventory : MonoBehaviour
{
    [SerializeField] private UIMainItem itemPrefab;
    [SerializeField] private RectTransform contentPanel;

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
                uiItemsList.Add(uiItem);
            }
            else
            {
                UIMainItem uiItem = CreateItem();
                uiItem.transform.SetParent(contentPanel);
                uiItem.SetData(itemList[i].item.ItemImage, itemList[i].quantity);
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
                    uiItemsList.Add(newItem);
                }
                else
                {
                    UIMainItem newItem = CreateItem();
                    newItem.transform.SetParent(contentPanel);
                    newItem.SetData(newDictionary[i].item.ItemImage, newDictionary[i].quantity);
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
                uiItemsList[j].DeleteObject();
                uiItemsList.RemoveAt(j);
            }
        }
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