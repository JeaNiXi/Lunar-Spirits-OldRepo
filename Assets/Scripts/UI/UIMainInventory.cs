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

        public event Action<int, int>
            OnItemDragStarted;
        public event Action<string, int, string, int>
            OnItemDropRequest;




        //public Action<int>




        //OnItemRMBClicked;
        //OnQuickSlotItemRMBClicked;
        //OnItemLMBClicked,
        //OnQuickSlotItemLMBClicked,
        //OnQuickSlotItemUnequipConfirmed,
        //OnRemoveAllConfirmed;





        //OnRemoveQuantityConfirmed,
        //OnQuickSlotEquipConfirmed,
        //OnMainVSMainSwapRequest,
        //OnInventoryItemMovingToEmptySlot,
        //OnInventoryItemMovingToEmptyEquipSlot;






        //public Action<int, string>

        //public Action<int, List<UIMainItem.EquipSlotType>>
        //    OnQuickSlotItemDragStarted,
        //    OnItemDragStarted;

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

        #region UIInitialization
        private UIMainItem CreateItem() => Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
        public void InitializeInventoryData(List<InventoryItem> itemList)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i].IsEmpty)
                {
                    UIMainItem uiItem = CreateItem();
                    uiItem.transform.SetParent(contentPanel);
                    uiItem.InitItem(itemList[i].slotType.ToString(), itemList[i].itemContainer.ToString());
                    InitializeActions(uiItem);
                    uiItemsList.Add(uiItem);
                }
                else
                {
                    UIMainItem uiItem = CreateItem();
                    uiItem.transform.SetParent(contentPanel);
                    uiItem.InitItem(itemList[i].item.ItemImage, itemList[i].quantity, itemList[i].slotType.ToString(), itemList[i].item.CanBeInSlots, itemList[i].itemContainer.ToString());
                    InitializeActions(uiItem);
                    uiItemsList.Add(uiItem);
                }
            }
        }
        public void InitializeQuickSlotsData(List<QuickSlotItem> quickSlotItemList)
        {
            for (int i = 0; i < quickSlotItemList.Count; i++)
            {
                if (quickSlotItemList[i].IsEmpty)
                {
                    UIMainItem uiItem = CreateItem();
                    uiItem.transform.SetParent(quickSlotPanel);
                    uiItem.InitItem(quickSlotItemList[i].slotType.ToString(), quickSlotItemList[i].itemContainer.ToString());
                    InitializeActions(uiItem);
                    uiQuickSlotsItems.Add(uiItem);
                }
                else
                {
                    UIMainItem uiItem = CreateItem();
                    uiItem.transform.SetParent(quickSlotPanel);
                    uiItem.InitItem(quickSlotItemList[i].item.ItemImage, quickSlotItemList[i].quantity, quickSlotItemList[i].slotType.ToString(), quickSlotItemList[i].item.CanBeInSlots, quickSlotItemList[i].itemContainer.ToString());
                    InitializeActions(uiItem);
                    uiQuickSlotsItems.Add(uiItem);
                }
            }
        }
        public void InitializeEquipmentSlotsData(List<EquipmentItem> equipmentItemsList)
        {
            for (int i = 0; i < LEFT_EQUIP_SLOTS; i++)
            {
                SetupEquipmentInitialization(i, equipmentItemsList, leftEquipmentPanel, true);
            }
            for (int i = LEFT_EQUIP_SLOTS; i < LEFT_EQUIP_SLOTS + RIGHT_EQUIP_SLOTS; i++)
            {
                SetupEquipmentInitialization(i, equipmentItemsList, rightEquipmentPanel, true);
            }
            for (int i = LEFT_EQUIP_SLOTS + RIGHT_EQUIP_SLOTS; i < LEFT_EQUIP_SLOTS + RIGHT_EQUIP_SLOTS + AMMO_EQUIP_SLOTS; i++)
            {
                SetupEquipmentInitialization(i, equipmentItemsList, rightEquipmentPanel, false);
            }
        }
        public void SetupEquipmentInitialization(int i, List<EquipmentItem> equipmentItemsList, RectTransform transformPanel, bool isQuantityDisabled)
        {
            if (equipmentItemsList[i].IsEmpty)
            {
                UIMainItem uiItem = CreateItem();
                uiItem.InitItem(equipmentItemsList[i].slotType.ToString(), equipmentItemsList[i].itemContainer.ToString());
                SetUpEquipmentData(uiItem, transformPanel, true);
            }
            else
            {
                UIMainItem uiItem = CreateItem();
                uiItem.InitItem(equipmentItemsList[i].item.ItemImage, equipmentItemsList[i].quantity, equipmentItemsList[i].slotType.ToString(), equipmentItemsList[i].item.CanBeInSlots, equipmentItemsList[i].itemContainer.ToString());
                SetUpEquipmentData(uiItem, transformPanel, isQuantityDisabled);
            }
        }
        public void SetUpEquipmentData(UIMainItem uiItem, RectTransform transformPanel, bool isQuantityDisabled)
        {
            uiItem.transform.SetParent(transformPanel);
            InitializeActions(uiItem);
            if (isQuantityDisabled)
                uiItem.ToggleQuantityPanel(false);
            uiEquipmentItems.Add(uiItem);
        }
        private void InitializeActions(UIMainItem uiItem)
        {
            //uiItem.OnItemRMBClicked += HandleRMBClick;
            uiItem.OnItemLMBClicked += HandleLMBClick;
            uiItem.OnItemDragStart += HandleDragStart;
            uiItem.OnItemDragEnd += HandleDragEnd;
            uiItem.OnItemDroppedOn += HandleItemDroppedOn;
        }
        private void DeleteActions(UIMainItem uiItem)
        {
            //uiItem.OnItemRMBClicked -= HandleRMBClick;
            uiItem.OnItemLMBClicked -= HandleLMBClick;
            uiItem.OnItemDragStart -= HandleDragStart;
            uiItem.OnItemDragEnd -= HandleDragEnd;
            uiItem.OnItemDroppedOn -= HandleItemDroppedOn;
        }
        #endregion

        #region UIUpdate
        public void UpdateInventoryUI(Dictionary<int, InventoryItem> newDictionary)
        {
            if (newDictionary.Count == uiItemsList.Count)
            {
                DeselectAllItems();
                ToggleMouseFollower(false);
                for (int i = 0; i < newDictionary.Count; i++)
                {
                    if (newDictionary[i].IsEmpty)
                        uiItemsList[i].InitItem(newDictionary[i].slotType.ToString(), newDictionary[i].itemContainer.ToString());
                    else
                        uiItemsList[i].InitItem(newDictionary[i].item.ItemImage, newDictionary[i].quantity, newDictionary[i].slotType.ToString(), newDictionary[i].item.CanBeInSlots, newDictionary[i].itemContainer.ToString());
                }
                return;
            }
            else if (newDictionary.Count > uiItemsList.Count)
            {
                DeselectAllItems();
                ToggleMouseFollower(false);
                int startIndex = uiItemsList.Count;
                for (int i = 0; i < uiItemsList.Count; i++)
                {
                    if (newDictionary[i].IsEmpty)
                        uiItemsList[i].InitItem(newDictionary[i].slotType.ToString(), newDictionary[i].itemContainer.ToString());
                    else
                        uiItemsList[i].InitItem(newDictionary[i].item.ItemImage, newDictionary[i].quantity, newDictionary[i].slotType.ToString(), newDictionary[i].item.CanBeInSlots, newDictionary[i].itemContainer.ToString());
                }
                for (int i = startIndex; i < newDictionary.Count; i++)
                {
                    if (newDictionary[i].IsEmpty)
                    {
                        UIMainItem newItem = CreateItem();
                        newItem.transform.SetParent(contentPanel);
                        newItem.InitItem(newDictionary[i].slotType.ToString(), newDictionary[i].itemContainer.ToString());
                        InitializeActions(newItem);
                        uiItemsList.Add(newItem);
                    }
                    else
                    {
                        UIMainItem newItem = CreateItem();
                        newItem.transform.SetParent(contentPanel);
                        newItem.InitItem(newDictionary[i].item.ItemImage, newDictionary[i].quantity, newDictionary[i].slotType.ToString(), newDictionary[i].item.CanBeInSlots, newDictionary[i].itemContainer.ToString());
                        InitializeActions(newItem);
                        uiItemsList.Add(newItem);
                    }
                }
                return;
            }
            else
            {
                DeselectAllItems();
                ToggleMouseFollower(false);
                int reminder = newDictionary.Count - 1;
                int startPoint = uiItemsList.Count - 1;
                for (int i = 0; i < newDictionary.Count; i++)
                {
                    if (newDictionary[i].IsEmpty)
                        uiItemsList[i].InitItem(newDictionary[i].slotType.ToString(), newDictionary[i].itemContainer.ToString());
                    else
                        uiItemsList[i].InitItem(newDictionary[i].item.ItemImage, newDictionary[i].quantity, newDictionary[i].slotType.ToString(), newDictionary[i].item.CanBeInSlots, newDictionary[i].itemContainer.ToString());
                }
                for (int j = startPoint; j > reminder; j--)
                {
                    DeleteActions(uiItemsList[j]);
                    if (uiItemsList[j].ItemSlots.Count > 0)
                        uiItemsList[j].ItemSlots.Clear();
                    uiItemsList[j].DeleteUIObject();
                    uiItemsList.RemoveAt(j);
                }
            }
        }
        public void UpdateQuickSlotsUI(List<QuickSlotItem> quickSlotsList)
        {
            for (int i = 0; i < quickSlotsList.Count; i++)
            {
                if (quickSlotsList[i].IsEmpty)
                    uiQuickSlotsItems[i].InitItem(quickSlotsList[i].slotType.ToString(), quickSlotsList[i].itemContainer.ToString());
                else
                    uiQuickSlotsItems[i].InitItem(quickSlotsList[i].item.ItemImage, quickSlotsList[i].quantity, quickSlotsList[i].slotType.ToString(), quickSlotsList[i].item.CanBeInSlots, quickSlotsList[i].itemContainer.ToString());
            }
            DeselectAllItems();
        }
        public void InitializeUpdateEquipmentUI(List<EquipmentItem> equipmentList)
        {
            for (int i = 0; i < LEFT_EQUIP_SLOTS; i++)
            {
                SetupUpdateEquipmentUI(i, equipmentList, true);
            }
            for (int i = LEFT_EQUIP_SLOTS; i < LEFT_EQUIP_SLOTS + RIGHT_EQUIP_SLOTS; i++)
            {
                SetupUpdateEquipmentUI(i, equipmentList, true);
            }
            for (int i = LEFT_EQUIP_SLOTS + RIGHT_EQUIP_SLOTS; i < LEFT_EQUIP_SLOTS + RIGHT_EQUIP_SLOTS + AMMO_EQUIP_SLOTS; i++)
            {
                SetupUpdateEquipmentUI(i, equipmentList, false);
            }
        }
        private void SetupUpdateEquipmentUI(int i, List<EquipmentItem> equipmentList, bool isQuantityDisabled)
        {
            if (equipmentList[i].IsEmpty)
            {
                uiEquipmentItems[i].InitItem(equipmentList[i].slotType.ToString(), equipmentList[i].itemContainer.ToString());
                if (isQuantityDisabled)
                    uiEquipmentItems[i].ToggleQuantityPanel(false);
            }
            else
            {
                uiEquipmentItems[i].InitItem(equipmentList[i].item.ItemImage, equipmentList[i].quantity, equipmentList[i].slotType.ToString(), equipmentList[i].item.CanBeInSlots, equipmentList[i].itemContainer.ToString());
                if (isQuantityDisabled)
                    uiEquipmentItems[i].ToggleQuantityPanel(false);
            }
        }
        #endregion

        #region Handlers
        private void HandleLMBClick(UIMainItem obj)
        {
            if (obj.IsEmpty)
                DeselectAllItems();
            else
            {
                DeselectAllItems();
                obj.SelectItem();
            }
        }
        private void HandleDragStart(UIMainItem obj)
        {
            if (obj.IsEmpty)
                return;
            switch (obj.ItemSlotContainer)
            {
                case UIMainItem.ItemContainer.Container:
                    OnItemDragStarted?.Invoke(uiItemsList.IndexOf(obj), 0); //Where ARG2 is container Type;
                    break;
                case UIMainItem.ItemContainer.QSContainer:
                    OnItemDragStarted?.Invoke(uiQuickSlotsItems.IndexOf(obj), 1);
                    break;
                case UIMainItem.ItemContainer.EquipmentContainer:
                    OnItemDragStarted?.Invoke(uiEquipmentItems.IndexOf(obj), 2);
                    break;
                default:
                    break;
            }
        }
        private void HandleDragEnd(UIMainItem obj) // Returns Dragged Item
        {
            ToggleMouseFollower(false);
            DeselectAllItems();
        }
        private void HandleItemDroppedOn(UIMainItem obj)
        {
            switch (obj.ItemSlotContainer)
            {
                case UIMainItem.ItemContainer.Container:
                    OnItemDropRequest?.Invoke(mouseFollower.ContainerString, mouseFollower.Index, obj.ItemSlotContainer.ToString(), uiItemsList.IndexOf(obj));
                    break;
                case UIMainItem.ItemContainer.QSContainer:
                    OnItemDropRequest?.Invoke(mouseFollower.ContainerString, mouseFollower.Index, obj.ItemSlotContainer.ToString(), uiQuickSlotsItems.IndexOf(obj));
                    break;
                case UIMainItem.ItemContainer.EquipmentContainer:
                    OnItemDropRequest?.Invoke(mouseFollower.ContainerString, mouseFollower.Index, obj.ItemSlotContainer.ToString(), uiEquipmentItems.IndexOf(obj));
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region MouseFollowerConfigs
        public void CreateMouseFollower(InventoryItem item, InventoryItem.SlotType slotType, InventoryItem.ItemContainer itemContainer, int index)
        {
            ToggleMouseFollower(true);
            mouseFollower.InitFollower(item, slotType, itemContainer, index);
        }
        public void CreateMouseFollower(QuickSlotItem item, QuickSlotItem.SlotType slotType, QuickSlotItem.ItemContainer itemContainer, int index)
        {
            ToggleMouseFollower(true);
            mouseFollower.InitFollower(item, slotType, itemContainer, index);
        }
        public void CreateMouseFollower(EquipmentItem item, EquipmentItem.SlotType slotType, EquipmentItem.ItemContainer itemContainer, int index)
        {
            ToggleMouseFollower(true);
            mouseFollower.InitFollower(item, slotType, itemContainer, index);
        }
        public bool IsMouseFollowerActive() => mouseFollower.IsActive();
        public void ToggleMouseFollower(bool value) => mouseFollower.ToggleMouseFollower(value);
        #endregion

        #region BasicFunctionality
        public void ThrowNotification(UINotifications.Notifications notificationType)
        {
            uiNotifications.ThrowNotification(notificationType);
        }
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
            for (int k = 0; k < uiEquipmentItems.Count; k++)
            {
                uiEquipmentItems[k].DeselectItem();
            }
        }
        #endregion















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








        public void ToggleActionPanel(bool value)
        {
            itemActionPanel.TogglePanel(value);
        }
        public void AddButton(string name, Action onClickAction)
        {
            itemActionPanel.AddButton(name, onClickAction);
        }



        //private string GetMouseFollowerType() => mouseFollower.FollowerType;
        //private int GetMouseFollowerIndex() => mouseFollower.ItemIndex;
        //private List<UIMouseFollower.EquipSlots> GetMouseFollowerEquipSlots() => mouseFollower.slotsToEquip;

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
            //confirmationPanel.ToggleConfirmationPanel(false, -1);
            //OnRemoveAllConfirmed?.Invoke(index);
        }
        public void ToggleConfirmQuantityPanel(bool value, int index, int maxRemoveSize)
        {
            confirmationQuantityPanel.ToggleConfirmQuantityPanel(value, index, maxRemoveSize);
        }
        public void ConfirmRemovingQuantity(int index, int quantity)
        {
            //confirmationQuantityPanel.ToggleConfirmQuantityPanel(false, -1, -1);
            //if (quantity == 0)
            //    return;
            //else
            //    OnRemoveQuantityConfirmed?.Invoke(index, quantity);
        }
        public void ToggleConfirmQuickSlotPanel(bool value, int index)
        {
            confirmationQuickSlotsPanel.ToggleConfirmQuantityPanel(value, index);
        }
        public void ConfirmQuickSlotEquip(int index, int slotIndex)
        {
            //confirmationQuickSlotsPanel.ToggleConfirmQuantityPanel(false, -1);
            //OnQuickSlotEquipConfirmed?.Invoke(index, slotIndex);
        }


        public void UnequipQuickSlot(int index)
        {
            //OnQuickSlotItemUnequipConfirmed?.Invoke(index);
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