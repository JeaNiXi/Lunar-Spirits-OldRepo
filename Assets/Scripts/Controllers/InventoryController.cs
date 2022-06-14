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
    }
    private readonly string[] ActionButtonsStrings =
    {
        "Use Item",
        "Remove All"
    };
    private void Awake()
    {
        InitializeInventorySO();
        ToggleInventory(false);
        InitializeMainUI();
        mainInventorySO.OnInventoryUpdated += HandleInventoryChange;
        uiMainInventory.OnItemRMBClicked += HandleItemRMBClick;
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
        if(inventoryItem.item is IRemovable iRemovable)
        {
            uiMainInventory.AddButton(ActionButtonsStrings[(int)ActionButtons.Remove], () => iRemovable.RemoveItem(mainInventorySO, index));
        }
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
