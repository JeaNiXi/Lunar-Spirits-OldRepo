using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private UIMainInventory uiMainInventory;
    [SerializeField] private MainCharacter mainCharacter;
    private InventorySO mainInventorySO;
    private enum ActionButtons
    {
        Use,
        Remove,
        RemoveAll,
    }
    private readonly string[] ActionButtonsStrings =
    {
        "Use Item",
        "Remove",
        "Remove All"
    };
    private void Awake()
    {
        InitializeInventorySO();
        ToggleInventory(false);
        InitializeMainUI();
        mainInventorySO.OnInventoryUpdated += HandleInventoryChange;
        uiMainInventory.OnItemRMBClicked += HandleItemRMBClick;
        uiMainInventory.OnRemoveAllConfirmed += HandleRemoveAllConfirmation;
        uiMainInventory.OnRemoveQuantityConfirmed += HandleRemoveQuantityConfirmation;
    }



    private void InitializeInventorySO()
    {
        mainInventorySO = mainCharacter.GetComponent<MainCharacter>().GetInventorySO();
        mainInventorySO.CorrectQuantity();
        mainInventorySO.CheckForInventoryGridEnd();
    }
    private void InitializeMainUI()
    {
        uiMainInventory.InitializeInventoryData(mainInventorySO.GetItemList());
    }



    private void HandleInventoryChange()
    {
        uiMainInventory.UpdateInventoryUI(mainInventorySO.GetCurrentInventoryState());
    }
    private void HandleItemRMBClick(int index)
    {
        if (uiMainInventory.IsPanelActive() == true)
        {
            uiMainInventory.ToggleActionPanel(false);
            return;
        }
        if (uiMainInventory.IsConfirmationPanelActive() == true)
        {
            return;
        }
        if (uiMainInventory.IsConfirmationQuantityPanelActive() == true)
        {
            return;
        }
        InventoryItem inventoryItem = mainInventorySO.GetItemAt(index);
        if (inventoryItem.IsEmpty)
        {
            return;
        }
        uiMainInventory.ToggleActionPanel(true);

        if (inventoryItem.item is IUsable iUsable)
        {
            uiMainInventory.AddButton(ActionButtonsStrings[(int)ActionButtons.Use], () => iUsable.UseItem(mainCharacter.gameObject, mainInventorySO, index));
        }
        if(inventoryItem.item is IRemovableQuantity iRemovableQuantity)
        {
            uiMainInventory.AddButton(ActionButtonsStrings[(int)ActionButtons.Remove], () => uiMainInventory.ToggleConfirmQuantityPanel(true, index, inventoryItem.quantity));
        }
        if(inventoryItem.item is IRemovable iRemovable)
        {
            uiMainInventory.AddButton(ActionButtonsStrings[(int)ActionButtons.RemoveAll], () => uiMainInventory.ToggleConfirmationPanel(true, index));
        }
    }
    private void HandleRemoveAllConfirmation(int index)
    {
        mainInventorySO.RemoveItem(index);
    }
    private void HandleRemoveQuantityConfirmation(int index, int quantity)
    {
        mainInventorySO.RemoveItem(index, quantity);
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
