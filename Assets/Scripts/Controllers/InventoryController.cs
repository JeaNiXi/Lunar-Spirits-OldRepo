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
        FindInventory();
        ToggleInventory(false);
        InitializeMainUI();
        mainInventorySO.OnInventoryUpdated += HandleInventoryChange;
    }



    private void FindInventory()
    {
        mainInventorySO = mainCharacter.GetComponent<MainCharacter>().GetInventorySO();
    }
    private void InitializeMainUI()
    {
        uiMainInventory.InitializeInventoryData(mainInventorySO.GetItemList());
    }
    private void ClearMainUI()
    {
        uiMainInventory.ClearInventory();
    }



    private void HandleInventoryChange()
    {
        ClearMainUI();
        InitializeMainUI();
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
