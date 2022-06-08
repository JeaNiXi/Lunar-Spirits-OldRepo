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

    public void ClearInventory()
    {
        foreach(UIMainItem item in uiItemsList)
        {
            Destroy(item.gameObject);
        }
        uiItemsList.Clear();
    }
}