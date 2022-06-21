using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory.SO;

public class PickUpController : MonoBehaviour
{
    [SerializeField] private InventorySO mainInventory;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)  // Items
        {
            //Item item = collision.gameObject.GetComponentInParent<Item>();
            //int reminder = mainInventory.AddItem(item.GetItem(), item.GetQuantity());
            //if (reminder == 0)
            //{
            //    item.DeleteItem();
            //}
            //else
            //{
            //    item.SetQuantity(reminder);
            //}
        }
    }
}
