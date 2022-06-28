using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


using Inventory.SO;

namespace Inventory.UI
{
    public class UIMainItem : MonoBehaviour, 
        IPointerClickHandler, 
        IBeginDragHandler, 
        IEndDragHandler, 
        IDragHandler, 
        IDropHandler,
        IPointerEnterHandler
    {
        [SerializeField] private Component imageComponent;
        [SerializeField] private Component quantityComponent;
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text quantityText;
        [SerializeField] private Image borderImage;
        public bool IsEmpty { get; set; }

        public event Action<UIMainItem>
            OnItemRMBClicked,
            OnItemLMBClicked,
            OnItemDragStart,
            OnItemDrag,
            OnItemDragEnd,
            OnItemDroppedOn,
            OnPointerHoveringOver;

        public enum UIItemSlots
        {
            MAIN_SLOT,
            HEAD,
            MEDALION,
            RING1,
            RING2,
            ARMOR,
            BRACERS,
            BOOTS,
            WEAPON_MAIN,
            WEAPON_SECONDARY,
            RANGED,
            AMMO,
            QUICK_SLOT,
        }
        public enum ItemContainer
        {
            Container,
            QSContainer,
            EquipmentContainer,
        }


        public UIItemSlots UISlot;
        public ItemContainer ItemSlotContainer;

        public List<ItemSO.ItemSlots> ItemSlots = new List<ItemSO.ItemSlots>();
        
        public void SelectItem()
        {
            borderImage.enabled = true;
        }
        public void DeselectItem()
        {
            borderImage.enabled = false;
        }
        public void ToggleQuantityPanel(bool value)
        {
            quantityComponent.gameObject.SetActive(value);
        }
        public void DisableImageAlpha(bool value)
        {
            int temp;
            if (value)
                temp = 0;
            else
                temp = 1;
            Color tmpColor = itemImage.color;
            tmpColor.a = temp;
            itemImage.color = tmpColor;
        }

        public void InitItem(string slotType, string itemContainer)
        {
            itemImage.sprite = null;
            DisableImageAlpha(true);
            quantityText.text = "";
            IsEmpty = true;
            UISlot = (UIItemSlots)Enum.Parse(typeof(UIItemSlots), slotType);
            ItemSlotContainer = (ItemContainer)Enum.Parse(typeof(ItemContainer), itemContainer);
            if (ItemSlots.Count > 0)
                ItemSlots.Clear();
            ToggleQuantityPanel(false);
        }
        public void InitItem(Sprite sprite, int quantity, string slotType, List<ItemSO.ItemSlots> itemSlotList, string itemContainer)
        {
            itemImage.sprite = sprite;
            DisableImageAlpha(false);
            quantityText.text = quantity.ToString();
            IsEmpty = false;
            UISlot = (UIItemSlots)Enum.Parse(typeof(UIItemSlots), slotType);
            ItemSlotContainer = (ItemContainer)Enum.Parse(typeof(ItemContainer), itemContainer);
            if (ItemSlots.Count > 0)
                ItemSlots.Clear();
            foreach(ItemSO.ItemSlots slot in itemSlotList)
            {
                ItemSlots.Add(slot);
            }    
            ToggleQuantityPanel(true);
        }

        #region Actions
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnItemLMBClicked?.Invoke(this);
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                OnItemRMBClicked?.Invoke(this);
            }
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
                return;
            OnItemDragStart?.Invoke(this);
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            OnItemDragEnd?.Invoke(this);
        }
        public void OnDrag(PointerEventData eventData)
        {
            //throw new NotImplementedException();
        }
        public void OnDrop(PointerEventData eventData)
        {
            OnItemDroppedOn?.Invoke(this);
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            OnPointerHoveringOver?.Invoke(this);
        }
        #endregion

        public void DeleteUIObject()
        {
            Destroy(gameObject);
        }


    }
}