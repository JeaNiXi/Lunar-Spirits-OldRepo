using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Inventory.SO;
using Inventory.UI;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private UIMainInventory uiMainInventory;
        [SerializeField] private MainCharacter mainCharacter;
        private InventorySO mainInventorySO;
        private enum ActionButtons
        {
            Use,
            QuickEquip,
            Unequip,
            Remove,
            RemoveAll,
        }
        private readonly string[] ActionButtonsStrings =
        {
        "Use Item",
        "Equip to Quick Slot",
        "Unequip Item",
        "Remove",
        "Remove All"
    };

        private void Awake()
        {
            //InitializeInventorySO();
            //ToggleInventory(false);
            //InitializeMainUI();
            //InitializeQuickSlotUI();
            //InitializeEqipmentUI();
            //ToggleUIComponents(false);
            //mainInventorySO.OnInventoryUpdated += HandleInventoryChange;
            //mainInventorySO.OnQuickSlotUpdated += HandleQuickSlotChange;
            //mainInventorySO.OnEquipmentUpdated += HandleEquipmentChange;
            //uiMainInventory.OnItemRMBClicked += HandleItemRMBClick;
            //uiMainInventory.OnQuickSlotItemRMBClicked += HandleQuickSlotRMBClick;
            //uiMainInventory.OnRemoveAllConfirmed += HandleRemoveAllConfirmation;
            //uiMainInventory.OnRemoveQuantityConfirmed += HandleRemoveQuantityConfirmation;
            //uiMainInventory.OnQuickSlotEquipConfirmed += HandleQuickSlotEquipConfirmation;
            //uiMainInventory.OnQuickSlotItemUnequipConfirmed += HandleQuickSlotUnequipConfirmation;
            //uiMainInventory.OnItemDragStarted += HandleItemDragStart;
            //uiMainInventory.OnQuickSlotItemDragStarted += HandleQuickSlotItemDragStart;

            //uiMainInventory.OnInventoryItemMovingToEmptySlot += HandleInventoryMovingToEmptySlot;
            //uiMainInventory.OnInventoryItemMovingToEmptyEquipSlot += HandleInventoryMovingToEmptyEquipmentSlot;
            //uiMainInventory.OnMainVSMainSwapRequest += HandleSwapMainVSMain;
        }



        private void InitializeInventorySO()
        {
            mainInventorySO = mainCharacter.GetComponent<MainCharacter>().GetInventorySO();
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
        }


        private void ToggleUIComponents(bool value)
        {
            if (uiMainInventory.IsMouseFollowerActive())
                uiMainInventory.ToggleMouseFollower(value);
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


        private void HandleItemRMBClick(int index)
        {
            //if (AreAllPanelsClosed())
            //{
            //    InventoryItem inventoryItem = mainInventorySO.GetItemAt(index);
            //    if (inventoryItem.IsEmpty)
            //    {
            //        return;
            //    }
            //    uiMainInventory.ToggleActionPanel(true);

            //    if (inventoryItem.item is IUsable iUsable)
            //    {
            //        uiMainInventory.AddButton(ActionButtonsStrings[(int)ActionButtons.Use], () => iUsable.UseItem(mainCharacter.gameObject, mainInventorySO, index));
            //    }
            //    if (inventoryItem.item is IQuickEquipable iQuickEquipable)
            //    {
            //        uiMainInventory.AddButton(ActionButtonsStrings[(int)ActionButtons.QuickEquip], () => uiMainInventory.ToggleConfirmQuickSlotPanel(true, index));
            //    }
            //    if (inventoryItem.item is IRemovableQuantity iRemovableQuantity)
            //    {
            //        uiMainInventory.AddButton(ActionButtonsStrings[(int)ActionButtons.Remove], () => uiMainInventory.ToggleConfirmQuantityPanel(true, index, inventoryItem.quantity));
            //    }
            //    if (inventoryItem.item is IRemovable iRemovable)
            //    {
            //        uiMainInventory.AddButton(ActionButtonsStrings[(int)ActionButtons.RemoveAll], () => uiMainInventory.ToggleConfirmationPanel(true, index));
            //    }
            //}
        }
        private void HandleQuickSlotRMBClick(int index)
        {
            //if (AreAllPanelsClosed())
            //{
            //    QuickSlotItem quickSlotItem = mainInventorySO.GetQuickSlotItemAt(index);
            //    if (quickSlotItem.IsEmpty)
            //    {
            //        return;
            //    }
            //    uiMainInventory.ToggleActionPanel(true);
            //    if (quickSlotItem.item is IUsableQuickSlot iUsableQuickSlot)
            //    {
            //        uiMainInventory.AddButton(ActionButtonsStrings[(int)ActionButtons.Use], () => iUsableQuickSlot.UseItemInQuickSlot(mainCharacter.gameObject, mainInventorySO, index));
            //    }
            //    if (quickSlotItem.item is IQuickUnequipable iQuickUnequipable)
            //    {
            //        uiMainInventory.AddButton(ActionButtonsStrings[(int)ActionButtons.Unequip], () => uiMainInventory.UnequipQuickSlot(index));
            //    }
            //}
        }
        private void HandleItemDragStart(int index, List<UIMainItem.EquipSlotType> slotTypes)
        {
            InventoryItem item = mainInventorySO.GetItemAt(index);
            uiMainInventory.CreateMouseFollower(item, slotTypes, index);
        }
        private void HandleQuickSlotItemDragStart(int index, List<UIMainItem.EquipSlotType> slotTypes)
        {
            QuickSlotItem item = mainInventorySO.GetQuickSlotItemAt(index);
            uiMainInventory.CreateMouseFollower(item, slotTypes, index);
        }


        private void HandleInventoryMovingToEmptySlot(int followerIndex, int newIndex)
        {
            mainInventorySO.MoveInventoryItemToEmptyInventorySlot(followerIndex, newIndex);
        }
        private void HandleInventoryMovingToEmptyEquipmentSlot(int followerIndex, int newIndex)
        {
            mainInventorySO.MoveInventoryItemToEmptyEquipmentSlot(followerIndex, newIndex);
        }




        private bool AreAllPanelsClosed()
        {
            if (uiMainInventory.IsPanelActive() == true)
            {
                uiMainInventory.ToggleActionPanel(false);
                return false;
            }
            if (uiMainInventory.IsConfirmationPanelActive() == true)
            {
                return false;
            }
            if (uiMainInventory.IsConfirmationQuantityPanelActive() == true)
            {
                return false;
            }
            if (uiMainInventory.IsQuickSlotSelectionPanelActive())
            {
                return false;
            }
            return true;
        }
        private void HandleRemoveAllConfirmation(int index)
        {
            mainInventorySO.RemoveItem(index);
        }
        private void HandleRemoveQuantityConfirmation(int index, int quantity)
        {
            mainInventorySO.RemoveItem(index, quantity);
        }
        private void HandleQuickSlotEquipConfirmation(int index, int slotIndex)
        {
            mainInventorySO.EquipToQuickSlot(index, slotIndex);
        }
        private void HandleQuickSlotUnequipConfirmation(int index)
        {
            mainInventorySO.UnequipQuitSlot(index);
        }
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
    }
}