using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Inventory.SO;
using Inventory.UI;
using Interfaces;
using Character;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        public static InventoryController Instance;
        [field: SerializeField] public UIMainInventory UIMainInventory { get; private set; }
        [field: SerializeField] public InventorySO MainInventorySO { get; private set; }
        [field: SerializeField] public CharacterManager MainCharacter { get; private set; }
        private enum ActionButtons
        {
            Use,
            QuickEquip,
            Equip,
            Unequip,
            Remove,
            RemoveAll,
        }
        private readonly string[] ActionButtonsStrings =
        {
        "Use Item",
        "Equip to Quick Slot",
        "Equip Item",
        "Unequip Item",
        "Remove",
        "Remove All"
        };

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            Debug.Log(UnityEngine.Random.Range(0, 1));
        }
        private void Start()
        {
            InitializeInventorySO();
            ToggleInventory(false);
            InitializeMainUI();
            InitializeQuickSlotUI();
            InitializeEqipmentUI();
            //InitializeStatsUI();
            ToggleUIComponents(false);
            MainInventorySO.OnInventoryUpdated += HandleInventoryChange;
            MainInventorySO.OnQuickSlotUpdated += HandleQuickSlotChange;
            MainInventorySO.OnEquipmentUpdated += HandleEquipmentChange;
            MainInventorySO.ThrowNotification += HandleNotificationRequest;

            MainInventorySO.OnItemEquipped += HandleItemEquippedRequest;

            UIMainInventory.OnItemRMBClicked += HandleItemRMBClick;

            UIMainInventory.OnItemDescriptionRequested += HandleItemDescriptionRequest;
            UIMainInventory.OnItemDragStarted += HandleItemDragStart;
            UIMainInventory.OnItemDropRequest += HandleItemDropRequest;

            UIMainInventory.OnQuickSlotEquipConfirmed += HandleQuickSlotEquipConfirmation;
            UIMainInventory.OnRemoveAllConfirmed += HandleRemoveAllConfirmation;
            UIMainInventory.OnRemoveQuantityConfirmed += HandleRemoveQuantityConfirmation;

            UIMainInventory.OnWeaponEquipRequst += HandleWeaponEquipRequest;
            UIMainInventory.OnLootListUpdated += HandleLootListUpdateRequest;

            //MainCharacter.GetActorSO().OnStatUpdate += HandleStatUIUpdateRequest;
        }














        #region Initializations
        private void InitializeInventorySO()
        {
            MainInventorySO.CorrectQuantity();
            MainInventorySO.CheckForInventoryGridEnd();
            MainInventorySO.CorrectQuickSlotQuantity();
            MainInventorySO.CorrectEquipSlotsQuantity();
            //Debug
            MainInventorySO.CorrectContainerSlotItemParameters();
        }
        private void InitializeMainUI()
        {
            UIMainInventory.InitializeInventoryData(MainInventorySO.GetItemList());
        }
        private void InitializeQuickSlotUI()
        {
            UIMainInventory.InitializeQuickSlotsData(MainInventorySO.GetQuickSlotList());
        }
        private void InitializeEqipmentUI()
        {
            UIMainInventory.InitializeEquipmentSlotsData(MainInventorySO.GetEquipmentItemsList());
            HandleWeaponEquipRequest();
        }
        //private void InitializeStatsUI()
        //{
        //    UIMainInventory.InitializeStatsUI(MainCharacter.GetActorSO());
        //}
        #endregion

        #region ToggleUI
        private void ToggleUIComponents(bool value)
        {
            if (UIMainInventory.IsMouseFollowerActive())
                UIMainInventory.ToggleMouseFollower(value);
        }
        private void ToggleActionPanel(bool value)
        {
            UIMainInventory.ToggleActionPanel(value);
        }
        public void ShowLootPanel(List<InventoryItem> itemList)
        {
            UIMainInventory.ToggleLootPanel(true);
            UIMainInventory.UpdateLootPanel(itemList);
        }
        public void HideLootPanel()
        {
            UIMainInventory.ClearLootPanel();
            UIMainInventory.ToggleLootPanel(false);
        }
        public void DisableDescriptionPanel()
        {
            UIMainInventory.ClearLootPanel();
            UIMainInventory.ToggleDescriptionPanel(false);
        }
        #endregion

        #region Handlers
        private void HandleItemRMBClick(int index, int containerType)
        {
            switch (containerType)
            {
                case 0:
                    InventoryItem inventoryItem = MainInventorySO.GetItemAt(index);
                    HandleActionPanelRequest(inventoryItem, index, "Container");
                    break;
                case 1:
                    QuickSlotItem qsItem = MainInventorySO.GetQuickSlotItemAt(index);
                    HandleActionPanelRequest(qsItem, index, "QSContainer");
                    break;
                case 2:
                    EquipmentItem equipItem = MainInventorySO.GetEquipmentItemAt(index);
                    HandleActionPanelRequest(equipItem, index, "EquipmentContainer");
                    break;
                default:
                    break;
            }
        }
        private void HandleItemDescriptionRequest(int itemIndex, int containerType)
        {
            switch(containerType)
            {
                case 0:
                    InventoryItem inventoryItem = MainInventorySO.GetItemAt(itemIndex);
                    UIMainInventory.InitializeDescriptionPanel(inventoryItem.item.name, inventoryItem.itemRarity.ToString(), inventoryItem.item.Description, inventoryItem.itemParameters);
                    break;
                case 3:
                    InventoryItem lootItem = MainInventorySO.GetLootItemAt(itemIndex);
                    UIMainInventory.InitializeDescriptionPanel(lootItem.item.name, lootItem.itemRarity.ToString(), lootItem.item.Description, lootItem.itemParameters);
                    break;
                //case 1:
                //    QuickSlotItem qsItem = MainInventorySO.GetQuickSlotItemAt(itemIndex);
                //    InventoryItem newItem = new InventoryItem(qsItem.item,qsItem.quantity, InventoryItem.SlotType.MAIN_SLOT)
                default:
                    break;
            }
        }
        private void HandleItemDragStart(int itemIndex, int containerType)
        {
            switch (containerType)
            {
                case 0:
                    InventoryItem inventoryItem = MainInventorySO.GetItemAt(itemIndex);
                    UIMainInventory.CreateMouseFollower(inventoryItem, inventoryItem.slotType, inventoryItem.itemContainer, itemIndex);
                    break;
                case 1:
                    QuickSlotItem qsItem = MainInventorySO.GetQuickSlotItemAt(itemIndex);
                    UIMainInventory.CreateMouseFollower(qsItem, qsItem.slotType, qsItem.itemContainer, itemIndex);
                    break;
                case 2:
                    EquipmentItem equipItem = MainInventorySO.GetEquipmentItemAt(itemIndex);
                    UIMainInventory.CreateMouseFollower(equipItem, equipItem.slotType, equipItem.itemContainer, itemIndex);
                    break;
                default:
                    break;
            }
        }
        private void HandleItemDropRequest(string originContainer, int originIndex, string destContainer, int destIndex)
        {
            MainInventorySO.SwapItemsHandler(originContainer, originIndex, destContainer, destIndex);
        }
        private void HandleInventoryChange()
        {
            UIMainInventory.UpdateInventoryUI(MainInventorySO.GetCurrentInventoryState());
        }
        private void HandleQuickSlotChange()
        {
            UIMainInventory.UpdateQuickSlotsUI(MainInventorySO.GetQuickSlotList());
        }
        private void HandleEquipmentChange()
        {
            UIMainInventory.InitializeUpdateEquipmentUI(MainInventorySO.GetEquipmentItemsList());
        }

        private void HandleActionPanelRequest(InventoryItem item, int index, string containerType)
        {
            if (item.item is IUsable iUsable)
            {
                UIMainInventory.AddButton(ActionButtonsStrings[(int)ActionButtons.Use], () => iUsable.UseItem(MainCharacter, MainInventorySO, index, containerType));
            }
            if (item.item is IEquipable iEquipable)
            {
                //UIMainInventory.AddButton(ActionButtonsStrings[(int)ActionButtons.Equip], () => iEquipable.EquipItem(MainCharacter, MainInventorySO, index, containerType));
                UIMainInventory.AddButton(ActionButtonsStrings[(int)ActionButtons.Equip], () => MainInventorySO.EquipItemHandler(index, containerType));
            }
            if (item.item is IQuickEquipable iQuickEquipable)
            {
                UIMainInventory.AddButton(ActionButtonsStrings[(int)ActionButtons.QuickEquip], () => UIMainInventory.ToggleConfirmQuickSlotPanel(true, index, containerType));
            }
            if (item.item is IRemovableQuantity iRemovableQuantity)
            {
                UIMainInventory.AddButton(ActionButtonsStrings[(int)ActionButtons.Remove], () => UIMainInventory.ToggleConfirmQuantityPanel(true, index, item.quantity, containerType));
            }
            if (item.item is IRemovable iRemovable)
            {
                UIMainInventory.AddButton(ActionButtonsStrings[(int)ActionButtons.RemoveAll], () => UIMainInventory.ToggleConfirmationPanel(true, index, containerType));
            }
        }
        private void HandleActionPanelRequest(QuickSlotItem item, int index, string containerType)
        {
            if (item.item is IUsable iUsable)
            {
                UIMainInventory.AddButton(ActionButtonsStrings[(int)ActionButtons.Use], () => iUsable.UseItem(MainCharacter, MainInventorySO, index, containerType));
            }
            if (item.item is IQuickEquipable iQuickEquipable)
            {
                UIMainInventory.AddButton(ActionButtonsStrings[(int)ActionButtons.QuickEquip], () => UIMainInventory.ToggleConfirmQuickSlotPanel(true, index, containerType));
            }
            if (item.item is IRemovableQuantity iRemovableQuantity)
            {
                UIMainInventory.AddButton(ActionButtonsStrings[(int)ActionButtons.Remove], () => UIMainInventory.ToggleConfirmQuantityPanel(true, index, item.quantity, containerType));
            }
            if (item.item is IRemovable iRemovable)
            {
                UIMainInventory.AddButton(ActionButtonsStrings[(int)ActionButtons.RemoveAll], () => UIMainInventory.ToggleConfirmationPanel(true, index, containerType));
            }
        }
        private void HandleActionPanelRequest(EquipmentItem item, int index, string containerType)
        {
            if (item.item is IEquipable iEquipable)
            {
                //UIMainInventory.AddButton(ActionButtonsStrings[(int)ActionButtons.Unequip], () => iEquipable.EquipItem(MainCharacter, MainInventorySO, index, containerType));
                UIMainInventory.AddButton(ActionButtonsStrings[(int)ActionButtons.Unequip], () => MainInventorySO.EquipItemHandler(index, containerType));
            }
            if (item.item is IRemovable iRemovable)
            {
                UIMainInventory.AddButton(ActionButtonsStrings[(int)ActionButtons.RemoveAll], () => UIMainInventory.ToggleConfirmationPanel(true, index, containerType));
            }
        }
        private void HandleQuickSlotEquipConfirmation(int index, int slotIndex, string containerType)
        {
            switch (containerType)
            {
                case "Container":
                    InventoryItem inventoryItem = MainInventorySO.GetItemAt(index);
                    QuickSlotItem quickSlotItem = MainInventorySO.GetQuickSlotItemAt(slotIndex);
                    MainInventorySO.SwapItems(inventoryItem, index, quickSlotItem, slotIndex);
                    break;
                case "QSContainer":
                    QuickSlotItem qsOriginItem = MainInventorySO.GetQuickSlotItemAt(index);
                    QuickSlotItem qsDestItem = MainInventorySO.GetQuickSlotItemAt(slotIndex);
                    MainInventorySO.SwapItems(qsOriginItem, index, qsDestItem, slotIndex);
                    break;
                //case "EquipmentContainer":
                //    EquipmentItem equipItem = mainInventorySO.GetEquipmentItemAt(index);
                //    break;
                default:
                    break;
            }
        }
        private void HandleRemoveAllConfirmation(int index, string containerType)
        {
            switch (containerType)
            {
                case "Container":
                    MainInventorySO.RemoveItem(MainInventorySO.GetItemList(), index);
                    break;
                case "QSContainer":
                    MainInventorySO.RemoveItem(MainInventorySO.GetQuickSlotList(), index);
                    break;
                case "EquipmentContainer":
                    MainInventorySO.RemoveItem(MainInventorySO.GetEquipmentItemsList(), index);
                    break;
                default:
                    break;
            }
        }
        private void HandleRemoveQuantityConfirmation(int index, int quantity, string containerType)
        {
            MainInventorySO.RemoveItem(index, quantity, containerType);
        }
        private void HandleNotificationRequest(UINotifications.Notifications notification)
        {
            UIMainInventory.ThrowNotification(notification);
        }
        //private void HandleStatUIUpdateRequest()
        //{
        //    UIMainInventory.UpdateStatsUI(MainCharacter.GetActorSO());
        //}
        private void HandleWeaponEquipRequest()
        {
            MainCharacter.SetUpEquipment(MainInventorySO.GetEquipmentItemsList());
        }
        private void HandleItemEquippedRequest(EquipmentItem item)
        {
            if (item.item is IEquipable iEquipable)
                iEquipable.EquipItem(MainCharacter);
        }
        private void HandleLootListUpdateRequest(List<InventoryItem> lootList)
        {
            MainInventorySO.SetLootContainer(lootList);
        }
        #endregion


        //Activating/Deactivating Inventory Screen
        public void ToggleInventory()
        {
            if (UIMainInventory.IsInventoryActive)
                ToggleInventory(false);
            else
                ToggleInventory(true);
        }
        public void ToggleInventory(bool value) =>
            UIMainInventory.SetInventoryActive(value);
        public void ToggleMouseClick()
        {
            if(UIMainInventory.gameObject.activeSelf)
            {
                if(!UIMainInventory.IsPointerOverUI())
                {
                    if(UIMainInventory.IsActionPanelActive())
                    {
                        ToggleActionPanel(false);
                        UIMainInventory.DeselectAllItems();
                        return;
                    }
                    else
                    {
                        UIMainInventory.DeselectAllItems();
                    }
                }
            }
        }
    }
}