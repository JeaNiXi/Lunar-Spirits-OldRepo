using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIMainItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] private Component imageComponent;
    [SerializeField] private Component quantityComponent;
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image borderImage;

    public enum SlotType
    {
        DEFAULT,
        MAIN,
        QUICK_SLOT,
        EQUIP_SLOT,
    }
    public SlotType mainSlotType = SlotType.DEFAULT;

    public enum EquipSlotType
    {
        _0_DEFAULT,
        _1_HEAD,
        _2_MEDALION,
        _3_RING1,
        _4_RING2,
        _5_ARMOR,
        _6_BRACERS,
        _7_BOOTS,
        _8_WEAPON_MAIN,
        _9_WEAPON_SECONDARY,
        _10_RANGED,
        _11_AMMO,
    }
    public EquipSlotType mainEquipSlotType = EquipSlotType._0_DEFAULT;
    public List<EquipSlotType> slotTypesOfItem = new List<EquipSlotType>();

    public event Action<UIMainItem>
        OnItemRMBClicked,
        OnItemLMBClicked,
        OnItemDragStart,
        OnItemDrag,
        OnItemDragEnd,
        OnItemDroppedOn;

    public bool IsEmpty { get; set; }

    public void SetData()
    {
        this.itemImage.sprite = null;
        this.quantityText.text = "";
        this.IsEmpty = true;
        this.mainSlotType = SlotType.DEFAULT;
        imageComponent.gameObject.SetActive(false);
    }
    public void SetData(Sprite sprite, int quantity, SlotType type)
    {
        imageComponent.gameObject.SetActive(true);
        this.itemImage.sprite = sprite;
        this.quantityText.text = quantity.ToString();
        this.mainSlotType = type;
        this.IsEmpty = false;
    }




    public void SetData(EquipSlotType type)
    {
        imageComponent.gameObject.SetActive(true);
        this.itemImage.sprite = null;
        Color tmpColor = itemImage.color;
        tmpColor.a = 0;
        this.itemImage.color = tmpColor;
        this.IsEmpty = true;
        this.mainSlotType = SlotType.EQUIP_SLOT;
        this.mainEquipSlotType = type;
    }
    public void SetData(Sprite sprite, int quantity, EquipSlotType type)
    {
        imageComponent.gameObject.SetActive(true);
        this.itemImage.sprite = sprite;
        Color tmpColor = itemImage.color;
        tmpColor.a = 1;
        this.itemImage.color = tmpColor;
        this.quantityText.text = quantity.ToString();
        this.mainSlotType = SlotType.EQUIP_SLOT;
        this.mainEquipSlotType = type;
        this.IsEmpty = false;
    }



    public void SelectItem()
    {
        borderImage.enabled = true;
    }
    public void DeselectItem()
    {
        borderImage.enabled = false;
    }
    public void DisableQuantityPanel()
    {
        quantityComponent.gameObject.SetActive(false);
    }
    public void DeleteObject()
    {
        Destroy(gameObject);
    }


    public List<EquipSlotType> GetEquipSlotList() => slotTypesOfItem;

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
}