using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;


using Inventory.SO;

namespace Inventory.UI
{
    public class UIMouseFollower : MonoBehaviour
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text quantity;

        public int ItemIndex { get; set; }

        #region Getters
        public InventoryItem InvItem { get; set; }
        public QuickSlotItem QSItem { get; set; }
        public EquipmentItem EItem { get; set; }
        public InventoryItem.SlotType InvSlotType { get; set; }
        public QuickSlotItem.SlotType QSSlotType { get; set; }
        public EquipmentItem.SlotType ESlotType { get; set; }
        public InventoryItem.ItemContainer InvCont { get; set; }
        public QuickSlotItem.ItemContainer QSCont { get; set; }
        public EquipmentItem.ItemContainer ECont { get; set; }
        public string ContainerString { get; set; }
        #endregion

        public bool IsActive() => isActiveAndEnabled == true;
        public int Index { get; set; }
        public void ToggleMouseFollower(bool value)
        {
            gameObject.SetActive(value);
        }
        public void InitFollower(InventoryItem item, InventoryItem.SlotType slotType, InventoryItem.ItemContainer itemContainer, int index)
        {
            itemImage.sprite = item.item.ItemImage;
            quantity.text = item.quantity.ToString();
            InvItem = item;
            InvSlotType = slotType;
            ContainerString = itemContainer.ToString();
            //InvCont = itemContainer;
            Index = index;
        }
        public void InitFollower(QuickSlotItem item, QuickSlotItem.SlotType slotType, QuickSlotItem.ItemContainer itemContainer, int index)
        {
            itemImage.sprite = item.item.ItemImage;
            quantity.text = item.quantity.ToString();
            QSItem = item;
            QSSlotType = slotType;
            ContainerString = itemContainer.ToString();
            //QSCont = itemContainer;
            Index = index;
        }
        public void InitFollower(EquipmentItem item, EquipmentItem.SlotType slotType, EquipmentItem.ItemContainer itemContainer, int index)
        {
            itemImage.sprite = item.item.ItemImage;
            quantity.text = item.quantity.ToString();
            EItem = item;
            ESlotType = slotType;
            ContainerString = itemContainer.ToString();
            //ECont = itemContainer;
            Index = index;
        }

        private void Update()
        {
            gameObject.transform.position = Mouse.current.position.ReadValue();
        }
    }
}