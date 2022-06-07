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

    //[SerializeField] private List<UIItem> initialItems = new List<UIItem>();

    private void Awake()
    {
        if (mainInventory.gameObject.activeSelf)
        {
            SetInventoryActive(false);
        }
        if (!mainInventorySO.IsSorted)
            InitSortInventory();
        InitializeInventory();
    }
    private void InitSortInventory()
    {
        mainInventorySO.StartInventoryInitSorting();
    }
    private void InitializeInventory()
    {
        mainInventory.InitializeInventoryUI(mainInventorySO, contentUI);
        mainInventorySO.OnInventoryUpdated += HandleInventoryUpdate;
        mainInventory.OnDescriptionRequest += HandleDescriptionRequest;
        mainInventory.OnDescriptionCloseRequest += HandleDescriptionCloseRequest;
        mainInventory.OnStartItemDrag += HandleItemStartDrag;
        mainInventory.OnSwapRequest += HandleItemSwaps;
        mainInventory.OnEquipRequested += HandleItemEquip;
    }

    private void HandleItemEquip(int index, UIEquipSlots equipSlot)
    {
        if(mainInventorySO.GetEquipSlotType(index)==equipSlot.slotType.SlotType.ToString())
        {
            Debug.Log("it fits");

        }
        else
        {
            Debug.Log("wrong item type");
        }
    }
    private void HandleInventoryUpdate(Dictionary<int, InventoryItem> obj)
    {
        UpdateInventoryUI();
        foreach (var item in obj)
        {
            mainInventory.UpdateInventoryList(item.Key, item.Value.item.ItemSprite, item.Value.quantity);
        }
    }
    private void UpdateInventoryUI()
    {
        mainInventory.ResetSelection();
    }



    private void HandleItemDropOnSlot(UIEquipSlots obj)
    {
        throw new NotImplementedException();
    }
    private void HandleItemSwaps(int index1, int index2)
    {
        mainInventorySO.SwapItems(index1, index2);
    }
    private void HandleItemStartDrag(int index)
    {
        InventoryItem item = mainInventorySO.GetItemAt(index);
        mainInventory.CreateMouseFollower(item);
    }
    private void HandleDescriptionCloseRequest()
    {
        if (itemDescriptionPanel.isActiveAndEnabled)
        {
            itemDescriptionPanel.ResetDescription();
            itemDescriptionPanel.gameObject.SetActive(false);
        }
    }
    private void HandleDescriptionRequest(int index)
    {
        if (mainInventory.currentDraggedItemIndex != -1)
            return;
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
