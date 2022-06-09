using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private UIMainInventory uiMainInventory;
    [SerializeField] private MainCharacter mainCharacter;
    private InventorySO mainInventorySO;

    private void Awake()
    {
        InitializeInventorySO();
        ToggleInventory(false);
        InitializeMainUI();
        mainInventorySO.OnInventoryUpdated += HandleInventoryChange;
    }

    //Debug()
    public void RemoveItem()
    {
        mainInventorySO.RemoveItem("WEAPON");
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
    //private void ClearMainUI()
    //{
    //    uiMainInventory.ClearInventory();
    //}



    private void HandleInventoryChange()
    {
        //This is working.
        //ClearMainUI();
        //InitializeMainUI();
        uiMainInventory.UpdateInventoryUI(mainInventorySO.GetCurrentInventoryState());
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
