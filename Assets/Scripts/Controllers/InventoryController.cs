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
        
        public UIMainInventory uiMainInventory { get; private set; }
        public void SetUIMainInventory(UIMainInventory inventoryUI)
        {
            uiMainInventory = inventoryUI;
        }
        public InventorySO mainInventorySO { get; private set; }
        public void SetMainInventorySO(InventorySO inventorySO)
        {
            mainInventorySO = inventorySO;
        }
        public CharacterManager mainCharacter { get; private set; }
        public void SetMainCharacter(CharacterManager character)
        {
            mainCharacter = character;
        }
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

        //private void Awake()
        private void Start()
        {
            InitializeInventorySO();
            ToggleInventory(false);
            InitializeMainUI();
            InitializeQuickSlotUI();
            InitializeEqipmentUI();
            InitializeStatsUI();
            ToggleUIComponents(false);
            mainInventorySO.OnInventoryUpdated += HandleInventoryChange;
            mainInventorySO.OnQuickSlotUpdated += HandleQuickSlotChange;
            mainInventorySO.OnEquipmentUpdated += HandleEquipmentChange;
            mainInventorySO.ThrowNotification += HandleNotificationRequest;

            uiMainInventory.OnItemRMBClicked += HandleItemRMBClick;

            uiMainInventory.OnItemDragStarted += HandleItemDragStart;
            uiMainInventory.OnItemDropRequest += HandleItemDropRequest;

            uiMainInventory.OnQuickSlotEquipConfirmed += HandleQuickSlotEquipConfirmation;
            uiMainInventory.OnRemoveAllConfirmed += HandleRemoveAllConfirmation;
            uiMainInventory.OnRemoveQuantityConfirmed += HandleRemoveQuantityConfirmation;

            uiMainInventory.OnWeaponEquipRequst += HandleWeaponEquipRequest;

            mainCharacter.GetActorSO().OnStatUpdate += HandleStatUIUpdateRequest;
        }








        #region Initializations
        private void InitializeInventorySO()
        {
            //mainInventorySO = mainCharacter.GetComponent<CharacterManager>().GetInventorySO();
            mainInventorySO.CorrectQuantity();
            mainInventorySO.CheckForInventoryGridEnd();
            mainInventorySO.CorrectQuickSlotQuantity();
            mainInventorySO.CorrectEquipSlotsQuantity();
        }
        private void InitializeMainUI()
        {
            uiMainInventory.InitializeInventoryData(mainInventorySO.GetItemList());
        }
        private void InitializeQuickSlotUI()
        {
            uiMainInventory.InitializeQuickSlotsData(mainInventorySO.GetQuickSlotList());
        }
        private void InitializeEqipmentUI()
        {
            uiMainInventory.InitializeEquipmentSlotsData(mainInventorySO.GetEquipmentItemsList());
            HandleWeaponEquipRequest();
        }
        private void InitializeStatsUI()
        {
            uiMainInventory.InitializeStatsUI(mainCharacter.GetActorSO());
        }
        #endregion

        #region ToggleUI
        private void ToggleUIComponents(bool value)
        {
            if (uiMainInventory.IsMouseFollowerActive())
                uiMainInventory.ToggleMouseFollower(value);
        }
        private void ToggleActionPanel(bool value)
        {
            uiMainInventory.ToggleActionPanel(value);
        }
        #endregion

        #region Handlers
        private void HandleItemRMBClick(int index, int containerType)
        {
            switch (containerType)
            {
                case 0:
                    InventoryItem inventoryItem = mainInventorySO.GetItemAt(index);
                    HandleActionPanelRequest(inventoryItem, index, "Container");
                    break;
                case 1:
                    QuickSlotItem qsItem = mainInventorySO.GetQuickSlotItemAt(index);
                    HandleActionPanelRequest(qsItem, index, "QSContainer");
                    break;
                case 2:
                    EquipmentItem equipItem = mainInventorySO.GetEquipmentItemAt(index);
                    HandleActionPanelRequest(equipItem, index, "EquipmentContainer");
                    break;
                default:
                    break;
            }
        }

        private void HandleItemDragStart(int itemIndex, int containerType)
        {
            switch (containerType)
            {
                case 0:
                    InventoryItem inventoryItem = mainInventorySO.GetItemAt(itemIndex);
                    uiMainInventory.CreateMouseFollower(inventoryItem, inventoryItem.slotType, inventoryItem.itemContainer, itemIndex);
                    break;
                case 1:
                    QuickSlotItem qsItem = mainInventorySO.GetQuickSlotItemAt(itemIndex);
                    uiMainInventory.CreateMouseFollower(qsItem, qsItem.slotType, qsItem.itemContainer, itemIndex);
                    break;
                case 2:
                    EquipmentItem equipItem = mainInventorySO.GetEquipmentItemAt(itemIndex);
                    uiMainInventory.CreateMouseFollower(equipItem, equipItem.slotType, equipItem.itemContainer, itemIndex);
                    break;
                default:
                    break;
            }
        }
        private void HandleItemDropRequest(string originContainer, int originIndex, string destContainer, int destIndex)
        {
            mainInventorySO.SwapItemsHandler(originContainer, originIndex, destContainer, destIndex);
        }
        private void HandleInventoryChange()
        {
            uiMainInventory.UpdateInventoryUI(mainInventorySO.GetCurrentInventoryState());
        }
        private void HandleQuickSlotChange()
        {
            uiMainInventory.UpdateQuickSlotsUI(mainInventorySO.GetQuickSlotList());
        }
        private void HandleEquipmentChange()
        {
            uiMainInventory.InitializeUpdateEquipmentUI(mainInventorySO.GetEquipmentItemsList());
        }

        private void HandleActionPanelRequest(InventoryItem item, int index, string containerType)
        {
            if (item.item is IUsable iUsable)
            {
                uiMainInventory.AddButton(ActionButtonsStrings[(int)ActionButtons.Use], () => iUsable.UseItem(mainCharacter, mainInventorySO, index, containerType));
            }
            if (item.item is IEquipable iEquipable)
            {
                uiMainInventory.AddButton(ActionButtonsStrings[(int)ActionButtons.Equip], () => iEquipable.EquipItem(mainCharacter, mainInventorySO, index, containerType));
            }
            if (item.item is IQuickEquipable iQuickEquipable)
            {
                uiMainInventory.AddButton(ActionButtonsStrings[(int)ActionButtons.QuickEquip], () => uiMainInventory.ToggleConfirmQuickSlotPanel(true, index, containerType));
            }
            if (item.item is IRemovableQuantity iRemovableQuantity)
            {
                uiMainInventory.AddButton(ActionButtonsStrings[(int)ActionButtons.Remove], () => uiMainInventory.ToggleConfirmQuantityPanel(true, index, item.quantity, containerType));
            }
            if (item.item is IRemovable iRemovable)
            {
                uiMainInventory.AddButton(ActionButtonsStrings[(int)ActionButtons.RemoveAll], () => uiMainInventory.ToggleConfirmationPanel(true, index, containerType));
            }
        }
        private void HandleActionPanelRequest(QuickSlotItem item, int index, string containerType)
        {
            if (item.item is IUsable iUsable)
            {
                uiMainInventory.AddButton(ActionButtonsStrings[(int)ActionButtons.Use], () => iUsable.UseItem(mainCharacter, mainInventorySO, index, containerType));
            }
            if (item.item is IQuickEquipable iQuickEquipable)
            {
                uiMainInventory.AddButton(ActionButtonsStrings[(int)ActionButtons.QuickEquip], () => uiMainInventory.ToggleConfirmQuickSlotPanel(true, index, containerType));
            }
            if (item.item is IRemovableQuantity iRemovableQuantity)
            {
                uiMainInventory.AddButton(ActionButtonsStrings[(int)ActionButtons.Remove], () => uiMainInventory.ToggleConfirmQuantityPanel(true, index, item.quantity, containerType));
            }
            if (item.item is IRemovable iRemovable)
            {
                uiMainInventory.AddButton(ActionButtonsStrings[(int)ActionButtons.RemoveAll], () => uiMainInventory.ToggleConfirmationPanel(true, index, containerType));
            }
        }
        private void HandleActionPanelRequest(EquipmentItem item, int index, string containerType)
        {
            if (item.item is IEquipable iEquipable)
            {
                uiMainInventory.AddButton(ActionButtonsStrings[(int)ActionButtons.Unequip], () => iEquipable.EquipItem(mainCharacter, mainInventorySO, index, containerType));
            }
            if (item.item is IRemovable iRemovable)
            {
                uiMainInventory.AddButton(ActionButtonsStrings[(int)ActionButtons.RemoveAll], () => uiMainInventory.ToggleConfirmationPanel(true, index, containerType));
            }
        }
        private void HandleQuickSlotEquipConfirmation(int index, int slotIndex, string containerType)
        {
            switch (containerType)
            {
                case "Container":
                    InventoryItem inventoryItem = mainInventorySO.GetItemAt(index);
                    QuickSlotItem quickSlotItem = mainInventorySO.GetQuickSlotItemAt(slotIndex);
                    mainInventorySO.SwapItems(inventoryItem, index, quickSlotItem, slotIndex);
                    break;
                case "QSContainer":
                    QuickSlotItem qsOriginItem = mainInventorySO.GetQuickSlotItemAt(index);
                    QuickSlotItem qsDestItem = mainInventorySO.GetQuickSlotItemAt(slotIndex);
                    mainInventorySO.SwapItems(qsOriginItem, index, qsDestItem, slotIndex);
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
                    mainInventorySO.RemoveItem(mainInventorySO.GetItemList(), index);
                    break;
                case "QSContainer":
                    mainInventorySO.RemoveItem(mainInventorySO.GetQuickSlotList(), index);
                    break;
                case "EquipmentContainer":
                    mainInventorySO.RemoveItem(mainInventorySO.GetEquipmentItemsList(), index);
                    break;
                default:
                    break;
            }
        }
        private void HandleRemoveQuantityConfirmation(int index, int quantity, string containerType)
        {
            mainInventorySO.RemoveItem(index, quantity, containerType);
        }
        private void HandleNotificationRequest(UINotifications.Notifications notification)
        {
            uiMainInventory.ThrowNotification(notification);
        }
        private void HandleStatUIUpdateRequest()
        {
            uiMainInventory.UpdateStatsUI(mainCharacter.GetActorSO());
        }
        private void HandleWeaponEquipRequest()
        {
            mainCharacter.SetUpEquipment(mainInventorySO.GetEquipmentItemsList());
        }
        #endregion


        //Activating/Deactivating Inventory Screen
        public void ToggleInventory()
        {
            if (uiMainInventory.gameObject.activeSelf)
                ToggleInventory(false);
            else
                ToggleInventory(true);
        }
        public void ToggleInventory(bool value) =>
            uiMainInventory.SetInventoryActive(value);
        public void ToggleMouseClick()
        {
            if(uiMainInventory.gameObject.activeSelf)
            {
                if(!uiMainInventory.IsPointerOverUI())
                {
                    if(uiMainInventory.IsActionPanelActive())
                    {
                        ToggleActionPanel(false);
                        uiMainInventory.DeselectAllItems();
                        return;
                    }
                    else
                    {
                        uiMainInventory.DeselectAllItems();
                    }
                }
            }
        }
    }
}