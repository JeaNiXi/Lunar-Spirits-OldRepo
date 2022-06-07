using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    [SerializeField] private UIItem itemPrefab;
    [SerializeField] private UIMouseFollower mouseFollower;

    [SerializeField] private UIEquipSlots headSlot;
    [SerializeField] private UIEquipSlots medalionSlot;
    [SerializeField] private UIEquipSlots ringOneSlot;
    [SerializeField] private UIEquipSlots ringTwoSlot;
    [SerializeField] private UIEquipSlots armorSlot;
    [SerializeField] private UIEquipSlots bracesSlot;
    [SerializeField] private UIEquipSlots bootsSlot;
    [SerializeField] private UIEquipSlots weaponOneSlot;
    [SerializeField] private UIEquipSlots weaponTwoSlot;
    [SerializeField] private UIEquipSlots rangeWeaponOneSlot;
    [SerializeField] private UIEquipSlots rangeWeaponAmmo;
    [SerializeField] private UIEquipSlots quickSlotOne;
    [SerializeField] private UIEquipSlots quickSlotTwo;

    private List<UIItem> listOfUIItems = new List<UIItem>();
    public event Action OnDescriptionCloseRequest;
    public event Action<int> OnDescriptionRequest, OnStartItemDrag;
    public event Action<int, int> OnSwapRequest;
    public event Action<int, UIEquipSlots> OnEquipRequested;

    //[SerializeField] private 

    public bool IsActive { get; private set; }
    public int currentDraggedItemIndex = -1;

    public void ToggleInventory(bool state)
    {
        gameObject.SetActive(state);
        IsActive = state;
        ResetSelection();
    }

    public void InitializeInventoryUI(InventorySO mainInventorySO, RectTransform contentRect)
    {
        for (int i = 0; i < mainInventorySO.inventoryItems.Count; i++)
        {
            UIItem newItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            newItem.transform.SetParent(contentRect);
            newItem.OnDescriptionRequested += HandleDescriptionRequest;
            newItem.OnDescriptionClosed += HandleDescriptionClosingRequest;
            newItem.OnItemClicked += HandleItemClicked;
            newItem.OnItemRMBClicked += HandleItemRMBClicked;
            newItem.OnBeginDragging += HandleItemBeginDrag;
            newItem.OnEndDragging += HandleItemEndDrag;
            newItem.OnItemDropped += HandleItemSwap;
            //newItem.SetData(mainInventorySO.inventoryItems[i].item.ItemSprite, mainInventorySO.inventoryItems[i].quantity);
            listOfUIItems.Add(newItem);
        }
        UpdateInventoryUI(mainInventorySO);
        ListenToEquipSlots();
    }


    private void HandleItemDropOnSlot(UIEquipSlots obj)
    {
        OnEquipRequested?.Invoke(currentDraggedItemIndex, obj);

    }
    private void HandleItemSwap(UIItem obj)
    {
        
        int index = listOfUIItems.IndexOf(obj);
        if (index == -1)
            return;
        OnSwapRequest?.Invoke(currentDraggedItemIndex, index);
    }
    private void HandleItemEndDrag(UIItem obj)
    {
        ResetDraggedItem();
    }
    private void HandleItemBeginDrag(UIItem obj)
    {
        int index = listOfUIItems.IndexOf(obj);
        if (index == -1)
            return;
        HandleItemClicked(obj);
        currentDraggedItemIndex = index;
        OnStartItemDrag?.Invoke(index);
    }
    private void HandleItemRMBClicked(UIItem obj)
    {
        Debug.Log("RMB CLicked");
    }
    private void HandleItemClicked(UIItem obj)
    {
        foreach(UIItem item in listOfUIItems)
        {
            item.Deselect();
        }
        obj.Select();
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



    public void CreateMouseFollower(InventoryItem item)
    {
        mouseFollower.CreateMouseFollower(item.item.ItemSprite, item.quantity);
        mouseFollower.gameObject.SetActive(true);
    }
    private void ResetDraggedItem()
    {
        currentDraggedItemIndex = -1;
        mouseFollower.RestoreMouseFollower();
        mouseFollower.gameObject.SetActive(false);
    }


    public void ResetSelection()
    {
        for (int i = 0; i < listOfUIItems.Count; i++)
        {
            listOfUIItems[i].Deselect();
        }
    }
    public void UpdateInventoryList(int index, Sprite sprite, int quantity)
    {
        //if(listOfUIItems.Count>index)
        //{
        listOfUIItems[index].SetData(sprite, quantity);
        //}
    }
    public void UpdateInventoryUI(InventorySO mainInventorySO)
    {
        for (int i = 0; i < listOfUIItems.Count; i++)
        {
            listOfUIItems[i].SetData(mainInventorySO.inventoryItems[i].item.ItemSprite, mainInventorySO.inventoryItems[i].quantity);
        }
    }
    public void ListenToEquipSlots()
    {
        headSlot.OnItemDroppedOnSlot += HandleItemDropOnSlot;
        medalionSlot.OnItemDroppedOnSlot += HandleItemDropOnSlot;
        ringOneSlot.OnItemDroppedOnSlot += HandleItemDropOnSlot;
        ringTwoSlot.OnItemDroppedOnSlot += HandleItemDropOnSlot;
        armorSlot.OnItemDroppedOnSlot += HandleItemDropOnSlot;
        bracesSlot.OnItemDroppedOnSlot += HandleItemDropOnSlot;
        bootsSlot.OnItemDroppedOnSlot += HandleItemDropOnSlot;
        weaponOneSlot.OnItemDroppedOnSlot += HandleItemDropOnSlot;
        weaponTwoSlot.OnItemDroppedOnSlot += HandleItemDropOnSlot;
        rangeWeaponOneSlot.OnItemDroppedOnSlot += HandleItemDropOnSlot;
        rangeWeaponAmmo.OnItemDroppedOnSlot += HandleItemDropOnSlot;
        quickSlotOne.OnItemDroppedOnSlot += HandleItemDropOnSlot;
        quickSlotTwo.OnItemDroppedOnSlot += HandleItemDropOnSlot;
    }


}
