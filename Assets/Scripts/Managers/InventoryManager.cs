using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private UIInventory mainInventory;
    [SerializeField] private InventorySO mainInventorySO;
    [SerializeField] private UIItemDescriptionPanel itemDescriptionPanel;
    [SerializeField] private RectTransform contentUI;
    [SerializeField] private List<UIItem> initialItems = new List<UIItem>();

    private void Awake()
    {
        if(mainInventory.gameObject.activeSelf)
        {
            SetInventoryActive(false);
        }
        InitializeInventory();
    }
    private void InitializeInventory()
    {
        mainInventory.InitializeInventoryUI(mainInventorySO, contentUI);
        mainInventory.OnDescriptionRequest += HandleDescriptionRequest;
        mainInventory.OnDescriptionCloseRequest += HandleDescriptionCloseRequest;
    }

    private void HandleDescriptionCloseRequest()
    {
        if(itemDescriptionPanel.isActiveAndEnabled)
        {
            itemDescriptionPanel.ResetDescription();
            itemDescriptionPanel.gameObject.SetActive(false);
        }
    }

    private void HandleDescriptionRequest(int index)
    {
        InventoryItem item = mainInventorySO.GetItemAt(index);
        itemDescriptionPanel.SetUpData(item.item.Name, item.item.RarityState.ToString(), item.item.Description);
        itemDescriptionPanel.transform.SetPositionAndRotation(Input.mousePosition, Quaternion.identity);
        itemDescriptionPanel.gameObject.SetActive(true);
    }

    public void SetInventoryActive(bool state)
    {
        mainInventory.ToggleInventory(state);
    }
    public bool GetInventoryState()
    {
        return mainInventory.IsActive;
    }
}
