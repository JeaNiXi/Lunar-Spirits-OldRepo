using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Inventory.SO;

namespace Inventory.UI
{
    public class UIMainInventory : MonoBehaviour
    {
        [SerializeField] private UIMainItem itemPrefab;
        [SerializeField] private UIItemActionPanel itemActionPanel;
        [SerializeField] private UIConfirmAllPanel confirmationPanel;
        [SerializeField] private UIConfirmQuantityPanel confirmationQuantityPanel;
        [SerializeField] private UIConfirmQuickSlotPanel confirmationQuickSlotsPanel;
        [SerializeField] private RectTransform contentPanel;
        [SerializeField] private RectTransform quickSlotPanel;
        [SerializeField] private UIMouseFollower mouseFollower;
        [SerializeField] private RectTransform leftEquipmentPanel;
        [SerializeField] private RectTransform rightEquipmentPanel;
        [SerializeField] private UINotifications uiNotifications;

        private enum SlotTypeTemplates
        {
            DEFAULT,
            MAIN,
            QUICK_SLOT,
            EQUIP_SLOT,
        }


        public Action<int>
            OnItemRMBClicked,
            OnQuickSlotItemRMBClicked,
            //OnItemLMBClicked,
            //OnQuickSlotItemLMBClicked,
            OnQuickSlotItemUnequipConfirmed,
            OnRemoveAllConfirmed;
        public Action<int, int>
            OnRemoveQuantityConfirmed,
            OnQuickSlotEquipConfirmed,
            OnItemDropRequest,
            OnMainVSMainSwapRequest,
            OnInventoryItemMovingToEmptySlot,
            OnInventoryItemMovingToEmptyEquipSlot;
        //public Action<int, string>

        public Action<int, List<UIMainItem.EquipSlotType>>
            OnQuickSlotItemDragStarted,
            OnItemDragStarted;

        private List<UIMainItem> uiItemsList = new List<UIMainItem>();
        private List<UIMainItem> uiQuickSlotsItems = new List<UIMainItem>();
        private List<UIMainItem> uiEquipmentItems = new List<UIMainItem>();

        private const int LEFT_EQUIP_SLOTS = 7;
        private const int RIGHT_EQUIP_SLOTS = 3;
        private const int AMMO_EQUIP_SLOTS = 1;
        public void SetInventoryActive(bool value)
        {
            gameObject.SetActive(value);
        }



        public void InitializeInventoryData(List<InventoryItem> itemList)
        {
            //for (int i = 0; i < itemList.Count; i++)
            //{
            //    if (itemList[i].IsEmpty)
            //    {
            //        UIMainItem uiItem = CreateItem();
            //        uiItem.transform.SetParent(contentPanel);
            //        uiItem.SetData();
            //        uiItem.OnItemRMBClicked += HandleRMBClick;
            //        uiItem.OnItemLMBClicked += HandleLMBClick;
            //        uiItem.OnItemDragStart += HandleDragStart;
            //        uiItem.OnItemDragEnd += HandleDragEnd;
            //        uiItem.OnItemDroppedOn += HandleItemDroppedOn;
            //        uiItemsList.Add(uiItem);
            //    }
            //    else
            //    {
            //        UIMainItem uiItem = CreateItem();
            //        uiItem.transform.SetParent(contentPanel);
            //        uiItem.SetData(itemList[i].item.ItemImage, itemList[i].quantity, UIMainItem.SlotType.MAIN);
            //        foreach (ItemSO.EquipSlots item in itemList[i].item.SlotsToEquipIn)
            //        {
            //            uiItem.slotTypesOfItem.Add((UIMainItem.EquipSlotType)item);
            //        }
            //        uiItem.OnItemRMBClicked += HandleRMBClick;
            //        uiItem.OnItemLMBClicked += HandleLMBClick;
            //        uiItem.OnItemDragStart += HandleDragStart;
            //        uiItem.OnItemDragEnd += HandleDragEnd;
            //        uiItem.OnItemDroppedOn += HandleItemDroppedOn;
            //        uiItemsList.Add(uiItem);
            //    }
            //}
        }
        public void InitializeQuickSlotsData(List<QuickSlotItem> quickSlotItemList)
        {
            //for (int i = 0; i < quickSlotItemList.Count; i++)
            //{
            //    if (quickSlotItemList[i].IsEmpty)
            //    {
            //        UIMainItem uiItem = CreateItem();
            //        uiItem.transform.SetParent(quickSlotPanel);
            //        uiItem.SetData(UIMainItem.SlotType.QUICK_SLOT);
            //        uiItem.OnItemRMBClicked += HandleRMBClick;
            //        uiItem.OnItemLMBClicked += HandleLMBClick;
            //        uiItem.OnItemDragStart += HandleDragStart;
            //        uiItem.OnItemDragEnd += HandleDragEnd;
            //        uiItem.OnItemDroppedOn += HandleItemDroppedOn;
            //        uiQuickSlotsItems.Add(uiItem);
            //    }
            //    else
            //    {
            //        UIMainItem uiItem = CreateItem();
            //        uiItem.transform.SetParent(quickSlotPanel);
            //        uiItem.SetData(quickSlotItemList[i].item.ItemImage, quickSlotItemList[i].quantity, UIMainItem.SlotType.QUICK_SLOT);
            //        uiItem.OnItemRMBClicked += HandleRMBClick;
            //        uiItem.OnItemLMBClicked += HandleLMBClick;
            //        uiItem.OnItemDragStart += HandleDragStart;
            //        uiItem.OnItemDragEnd += HandleDragEnd;
            //        uiItem.OnItemDroppedOn += HandleItemDroppedOn;
            //        uiQuickSlotsItems.Add(uiItem);
            //    }
            //}
        }
        public void InitializeEquipmentSlotsData(List<EquipmentItem> equipmentItemsList)
        {
            //for (int i = 0; i < LEFT_EQUIP_SLOTS; i++)
            //{
            //    SetupEquipmentInitialization(i, equipmentItemsList, leftEquipmentPanel, true);
            //}
            //for (int i = LEFT_EQUIP_SLOTS; i < LEFT_EQUIP_SLOTS + RIGHT_EQUIP_SLOTS; i++)
            //{
            //    SetupEquipmentInitialization(i, equipmentItemsList, rightEquipmentPanel, true);
            //}
            //for (int i = LEFT_EQUIP_SLOTS + RIGHT_EQUIP_SLOTS; i < LEFT_EQUIP_SLOTS + RIGHT_EQUIP_SLOTS + AMMO_EQUIP_SLOTS; i++)
            //{
            //    SetupEquipmentInitialization(i, equipmentItemsList, rightEquipmentPanel, false);
            //}
        }
        public void SetupEquipmentInitialization(int i, List<EquipmentItem> equipmentItemsList, RectTransform transformPanel, bool isQuantityDisabled)
        {
            //if (equipmentItemsList[i].IsEmpty)
            //{
            //    UIMainItem uiItem = CreateItem();
            //    uiItem.SetData((UIMainItem.EquipSlotType)equipmentItemsList[i].slotType);
            //    SetUpEquipmentData(uiItem, transformPanel, true);
            //}
            //else
            //{
            //    UIMainItem uiItem = CreateItem();
            //    uiItem.SetData(equipmentItemsList[i].item.ItemImage, equipmentItemsList[i].quantity, (UIMainItem.EquipSlotType)equipmentItemsList[i].slotType);
            //    foreach (ItemSO.EquipSlots item in equipmentItemsList[i].item.SlotsToEquipIn)
            //    {
            //        uiItem.slotTypesOfItem.Add((UIMainItem.EquipSlotType)item);
            //    }
            //    SetUpEquipmentData(uiItem, transformPanel, isQuantityDisabled);
            //}
        }
        public void SetUpEquipmentData(UIMainItem uiItem, RectTransform transformPanel, bool isQuantityDisabled)
        {
            //uiItem.transform.SetParent(transformPanel);
            //uiItem.OnItemRMBClicked += HandleRMBClick;
            //uiItem.OnItemLMBClicked += HandleLMBClick;
            //uiItem.OnItemDragStart += HandleDragStart;
            //uiItem.OnItemDragEnd += HandleDragEnd;
            //uiItem.OnItemDroppedOn += HandleItemDroppedOn;
            //if (isQuantityDisabled)
            //    uiItem.ToggleQuantityPanel(false);
            //uiEquipmentItems.Add(uiItem);
        }
        private UIMainItem CreateItem() => Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);



        public void DeselectAllItems()
        {
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
            //if (newDictionary.Count == uiItemsList.Count)
            //{
            //    DeselectAllItems();
            //    ToggleMouseFollower(false);
            //    for (int i = 0; i < newDictionary.Count; i++)
            //    {
            //        if (newDictionary[i].IsEmpty)
            //        {
            //            if (uiItemsList[i].slotTypesOfItem.Count > 0)
            //                uiItemsList[i].slotTypesOfItem.Clear();
            //            uiItemsList[i].SetData();
            //        }
            //        else
            //        {
            //            if (uiItemsList[i].slotTypesOfItem.Count > 0)
            //                uiItemsList[i].slotTypesOfItem.Clear();
            //            uiItemsList[i].SetData(newDictionary[i].item.ItemImage, newDictionary[i].quantity, UIMainItem.SlotType.MAIN);
            //            foreach (ItemSO.EquipSlots item in newDictionary[i].item.SlotsToEquipIn)
            //            {
            //                uiItemsList[i].slotTypesOfItem.Add((UIMainItem.EquipSlotType)item);
            //            }
            //        }
            //    }
            //    return;
            //}
            //else if (newDictionary.Count > uiItemsList.Count)
            //{
            //    DeselectAllItems();
            //    ToggleMouseFollower(false);
            //    int startIndex = uiItemsList.Count;
            //    for (int i = 0; i < uiItemsList.Count; i++)
            //    {
            //        if (newDictionary[i].IsEmpty)
            //        {
            //            if (uiItemsList[i].slotTypesOfItem.Count > 0)
            //                uiItemsList[i].slotTypesOfItem.Clear();
            //            uiItemsList[i].SetData();
            //        }
            //        else
            //        {
            //            if (uiItemsList[i].slotTypesOfItem.Count > 0)
            //                uiItemsList[i].slotTypesOfItem.Clear();
            //            uiItemsList[i].SetData(newDictionary[i].item.ItemImage, newDictionary[i].quantity, UIMainItem.SlotType.MAIN);
            //            foreach (ItemSO.EquipSlots item in newDictionary[i].item.SlotsToEquipIn)
            //            {
            //                uiItemsList[i].slotTypesOfItem.Add((UIMainItem.EquipSlotType)item);
            //            }
            //        }
            //    }
            //    for (int i = startIndex; i < newDictionary.Count; i++)
            //    {
            //        if (newDictionary[i].IsEmpty)
            //        {
            //            UIMainItem newItem = CreateItem();
            //            newItem.transform.SetParent(contentPanel);
            //            newItem.SetData();
            //            newItem.OnItemRMBClicked += HandleRMBClick;
            //            newItem.OnItemLMBClicked += HandleLMBClick;
            //            newItem.OnItemDragStart += HandleDragStart;
            //            newItem.OnItemDragEnd += HandleDragEnd;
            //            newItem.OnItemDroppedOn += HandleItemDroppedOn;
            //            uiItemsList.Add(newItem);
            //        }
            //        else
            //        {
            //            UIMainItem newItem = CreateItem();
            //            newItem.transform.SetParent(contentPanel);
            //            newItem.SetData(newDictionary[i].item.ItemImage, newDictionary[i].quantity, UIMainItem.SlotType.MAIN);
            //            foreach (ItemSO.EquipSlots item in newDictionary[i].item.SlotsToEquipIn)
            //            {
            //                newItem.slotTypesOfItem.Add((UIMainItem.EquipSlotType)item);
            //            }
            //            newItem.OnItemRMBClicked += HandleRMBClick;
            //            newItem.OnItemLMBClicked += HandleLMBClick;
            //            newItem.OnItemDragStart += HandleDragStart;
            //            newItem.OnItemDragEnd += HandleDragEnd;
            //            newItem.OnItemDroppedOn += HandleItemDroppedOn;
            //            uiItemsList.Add(newItem);
            //        }
            //    }
            //    return;
            //}
            //else
            //{
            //    DeselectAllItems();
            //    ToggleMouseFollower(false);
            //    int reminder = newDictionary.Count - 1;
            //    int startPoint = uiItemsList.Count - 1;
            //    for (int i = 0; i < newDictionary.Count; i++)
            //    {
            //        if (newDictionary[i].IsEmpty)
            //        {
            //            if (uiItemsList[i].slotTypesOfItem.Count > 0)
            //                uiItemsList[i].slotTypesOfItem.Clear();
            //            uiItemsList[i].SetData();
            //        }
            //        else
            //        {
            //            if (uiItemsList[i].slotTypesOfItem.Count > 0)
            //                uiItemsList[i].slotTypesOfItem.Clear();
            //            uiItemsList[i].SetData(newDictionary[i].item.ItemImage, newDictionary[i].quantity, UIMainItem.SlotType.MAIN);
            //            foreach (ItemSO.EquipSlots item in newDictionary[i].item.SlotsToEquipIn)
            //            {
            //                uiItemsList[i].slotTypesOfItem.Add((UIMainItem.EquipSlotType)item);
            //            }
            //        }
            //    }
            //    for (int j = startPoint; j > reminder; j--)
            //    {
            //        uiItemsList[j].OnItemRMBClicked -= HandleRMBClick;
            //        uiItemsList[j].OnItemLMBClicked -= HandleLMBClick;
            //        uiItemsList[j].OnItemDragStart -= HandleDragStart;
            //        uiItemsList[j].OnItemDragEnd -= HandleDragEnd;
            //        uiItemsList[j].OnItemDroppedOn -= HandleItemDroppedOn;
            //        uiItemsList[j].slotTypesOfItem.Clear();
            //        uiItemsList[j].DeleteObject();
            //        uiItemsList.RemoveAt(j);
            //    }
            //}
        }
        public void UpdateQuickSlotsUI(List<QuickSlotItem> quickSlotsList)
        {
            //for (int i = 0; i < quickSlotsList.Count; i++)
            //{
            //    if (quickSlotsList[i].IsEmpty)
            //    {
            //        uiQuickSlotsItems[i].SetData(UIMainItem.SlotType.QUICK_SLOT);
            //    }
            //    else
            //    {
            //        uiQuickSlotsItems[i].SetData(quickSlotsList[i].item.ItemImage, quickSlotsList[i].quantity, UIMainItem.SlotType.QUICK_SLOT);
            //    }
            //}
            //DeselectAllItems();
        }
        public void InitializeUpdateEquipmentUI(List<EquipmentItem> equipmentList)
        {
            //for (int i = 0; i < LEFT_EQUIP_SLOTS; i++)
            //{
            //    SetupUpdateEquipmentUI(i, equipmentList, true);
            //}
            //for (int i = LEFT_EQUIP_SLOTS; i < LEFT_EQUIP_SLOTS + RIGHT_EQUIP_SLOTS; i++)
            //{
            //    SetupUpdateEquipmentUI(i, equipmentList, true);
            //}
            //for (int i = LEFT_EQUIP_SLOTS + RIGHT_EQUIP_SLOTS; i < LEFT_EQUIP_SLOTS + RIGHT_EQUIP_SLOTS + AMMO_EQUIP_SLOTS; i++)
            //{
            //    SetupUpdateEquipmentUI(i, equipmentList, false);
            //}
        }
        private void SetupUpdateEquipmentUI(int i, List<EquipmentItem> equipmentList, bool isQuantityDisabled)
        {
            //if (equipmentList[i].IsEmpty)
            //{
            //    if (uiEquipmentItems[i].slotTypesOfItem.Count > 0)
            //        uiEquipmentItems[i].slotTypesOfItem.Clear();
            //    uiEquipmentItems[i].SetData((UIMainItem.EquipSlotType)equipmentList[i].slotType);
            //    if (isQuantityDisabled)
            //        uiEquipmentItems[i].ToggleQuantityPanel(false);
            //}
            //else
            //{
            //    uiEquipmentItems[i].SetData(equipmentList[i].item.ItemImage, equipmentList[i].quantity, (UIMainItem.EquipSlotType)equipmentList[i].slotType);
            //    if (uiEquipmentItems[i].slotTypesOfItem.Count > 0)
            //        uiEquipmentItems[i].slotTypesOfItem.Clear();
            //    foreach (ItemSO.EquipSlots item in equipmentList[i].item.SlotsToEquipIn)
            //    {
            //        uiEquipmentItems[i].slotTypesOfItem.Add((UIMainItem.EquipSlotType)item);
            //    }
            //    if (isQuantityDisabled)
            //        uiEquipmentItems[i].ToggleQuantityPanel(false);
            //}
        }


        private void HandleRMBClick(UIMainItem obj)
        {
            //if (obj.mainSlotType == UIMainItem.SlotType.MAIN)
            //{
            //    if (IsIndexNotValidInInventorySlot(obj, out int index))
            //        return;
            //    //int index = uiItemsList.IndexOf(obj);
            //    //if (index == -1)
            //    //    return;
            //    OnItemRMBClicked?.Invoke(index);
            //}
            //if (obj.mainSlotType == UIMainItem.SlotType.QUICK_SLOT)
            //{
            //    if (IsIndexNotValidInQuickSlot(obj, out int index))
            //        return;
            //    //int index = uiQuickSlotsItems.IndexOf(obj);
            //    //if (index == -1)
            //    //    return;
            //    OnQuickSlotItemRMBClicked?.Invoke(index);
            //}
        }
        private void HandleLMBClick(UIMainItem obj)
        {
            //if (obj.mainSlotType == UIMainItem.SlotType.MAIN)
            //{
            //    if (IsIndexNotValidInInventorySlot(obj, out int index))
            //        return;
            //    //int index = uiItemsList.IndexOf(obj);
            //    //if (index == -1)
            //    //    return;
            //    DeselectAllItems();
            //    obj.SelectItem();
            //    //OnItemLMBClicked?.Invoke(index);
            //}
            //if (obj.mainSlotType == UIMainItem.SlotType.QUICK_SLOT)
            //{
            //    if (IsIndexNotValidInQuickSlot(obj, out int index))
            //        return;
            //    //int index = uiQuickSlotsItems.IndexOf(obj);
            //    //if (index == -1)
            //    //    return;
            //    DeselectAllItems();
            //    obj.SelectItem();
            //    //OnQuickSlotItemLMBClicked?.Invoke(index);
            //}
        }
        private void HandleDragStart(UIMainItem obj)
        {
            //if (obj.mainSlotType == UIMainItem.SlotType.MAIN)
            //{
            //    if (IsIndexNotValidInInventorySlot(obj, out int index))
            //        return;
            //    //int index = uiItemsList.IndexOf(obj);
            //    //if (index == -1)
            //    //    return;
            //    obj.SelectItem();
            //    OnItemDragStarted?.Invoke(index, obj.GetEquipSlotList());
            //}
            //if (obj.mainSlotType == UIMainItem.SlotType.QUICK_SLOT)
            //{
            //    if (IsIndexNotValidInQuickSlot(obj, out int index))
            //        return;
            //    //int index = uiQuickSlotsItems.IndexOf(obj);
            //    //if (index == -1)
            //    //    return;
            //    obj.SelectItem();
            //    OnQuickSlotItemDragStarted?.Invoke(index, obj.GetEquipSlotList());
            //}

        }
        private void HandleDragEnd(UIMainItem obj) // returns the dragged item origin// SHOULD REMAKE TO JUST DESELECT AND TOGGLE
        {
            //if (obj.mainSlotType == UIMainItem.SlotType.MAIN)
            //{
            //    ToggleMouseFollower(false);
            //    DeselectAllItems();
            //}
            //if (obj.mainSlotType == UIMainItem.SlotType.QUICK_SLOT)
            //{
            //    ToggleMouseFollower(false);
            //    DeselectAllItems();
            //}

        }
        private void HandleItemDroppedOn(UIMainItem obj)
        {
            //if (obj.IsEmpty)
            //{
            //    if (obj.mainSlotType == UIMainItem.SlotType.DEFAULT)
            //    {
            //        if (IsIndexNotValidInInventorySlot(obj, out int index))
            //            return;
            //        //int index = uiItemsList.IndexOf(obj);
            //        //if (index == -1)
            //        //    return;
            //        OnInventoryItemMovingToEmptySlot?.Invoke(GetMouseFollowerIndex(), index);
            //        return;
            //    }
            //    if (obj.mainSlotType == UIMainItem.SlotType.EQUIP_SLOT)
            //    {
            //        if (IsIndexNotValidInEquipSlot(obj, out int index))
            //            return;
            //        foreach (UIMouseFollower.EquipSlots slot in GetMouseFollowerEquipSlots())
            //        {
            //            if (obj.mainEquipSlotType.ToString() == slot.ToString())
            //            {
            //                OnInventoryItemMovingToEmptyEquipSlot?.Invoke(GetMouseFollowerIndex(), index);
            //                return;
            //            }
            //        }
            //        uiNotifications.ThrowNotification(UINotifications.Notifications.WRONG_ITEM_TYPE);
            //        return;
            //    }
            //    if(obj.mainSlotType==UIMainItem.SlotType.QUICK_SLOT)
            //    {
            //        if (IsIndexNotValidInQuickSlot(obj, out int index))
            //            return;
            //        foreach(UIMouseFollower.EquipSlots slot in GetMouseFollowerEquipSlots())
            //        {
            //            if (QuickSlotItem.acceptedSlots.Contains(slot.ToString()))
            //            {
            //                Debug.Log("Can be equipped to quick slot");
            //                return;
            //            }
            //        }
            //        uiNotifications.ThrowNotification(UINotifications.Notifications.WRONG_ITEM_TYPE);
            //        return;
            //    }
            //}
            //Debug.Log("Dropped on mainslottype  = " + obj.mainSlotType);
            //Debug.Log("Dropped on mainequip slot type  = " + obj.mainEquipSlotType); // this one for quick slot
            //Debug.Log("GetMouseFollowerType " + GetMouseFollowerType());
            //foreach (UIMouseFollower.EquipSlots slotType in mouseFollower.slotsToEquip)
            //{
            //    Debug.Log("can be equipped in " + slotType);
            //}

        }




        public void ToggleActionPanel(bool value)
        {
            itemActionPanel.TogglePanel(value);
        }
        public void AddButton(string name, Action onClickAction)
        {
            itemActionPanel.AddButton(name, onClickAction);
        }

        public void CreateMouseFollower(InventoryItem item, List<UIMainItem.EquipSlotType> slotTypes, int index)
        {
            //mouseFollower.ToggleMouseFollower(true);
            //mouseFollower.SetUpData(item.item.ItemImage, item.quantity);
            //mouseFollower.FollowerType = item.item.ItemType.ToString();
            //foreach (UIMainItem.EquipSlotType slotType in slotTypes)
            //{
            //    mouseFollower.slotsToEquip.Add((UIMouseFollower.EquipSlots)slotType);
            //}
            //mouseFollower.ItemIndex = index;
        }
        public void CreateMouseFollower(QuickSlotItem item, List<UIMainItem.EquipSlotType> slotTypes, int index)
        {
            //mouseFollower.ToggleMouseFollower(true);
            //mouseFollower.SetUpData(item.item.ItemImage, item.quantity);
            //mouseFollower.FollowerType = item.item.ItemType.ToString();
            //foreach (UIMainItem.EquipSlotType slotType in slotTypes)
            //{
            //    mouseFollower.slotsToEquip.Add((UIMouseFollower.EquipSlots)slotType);
            //}
            //mouseFollower.ItemIndex = index;
        }
        public void CreateMouseFollower(QuickSlotItem item, string slotType, int index)
        {
            //mouseFollower.ToggleMouseFollower(true);
            //mouseFollower.SetUpData(item.item.ItemImage, item.quantity);
            //mouseFollower.FollowerType = item.item.ItemType.ToString();
            ////mouseFollower.FollowerSlotType = slotType;
            //mouseFollower.ItemIndex = index;
        }
        public bool IsMouseFollowerActive() => mouseFollower.IsActive();
        public void ToggleMouseFollower(bool value) => mouseFollower.ToggleMouseFollower(value);
        private string GetMouseFollowerType() => mouseFollower.FollowerType;
        private int GetMouseFollowerIndex() => mouseFollower.ItemIndex;
        private List<UIMouseFollower.EquipSlots> GetMouseFollowerEquipSlots() => mouseFollower.slotsToEquip;

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
        private bool IsIndexNotValidInInventorySlot(UIMainItem item, out int index)
        {
            index = uiItemsList.IndexOf(item);
            Debug.Log(index);
            return index == -1;
        }
        private bool IsIndexNotValidInQuickSlot(UIMainItem item, out int index)
        {
            index = uiQuickSlotsItems.IndexOf(item);
            Debug.Log(index);
            return index == -1;
        }
        private bool IsIndexNotValidInEquipSlot(UIMainItem item, out int index)
        {
            index = uiEquipmentItems.IndexOf(item);
            Debug.Log(index);
            return index == -1;
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
}