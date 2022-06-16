using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIMainItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Component imageComponent;
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

    public event Action<UIMainItem>
        OnItemRMBClicked,
        OnItemLMBClicked;

    private bool IsEmpty { get; set; }

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
    public void SelectItem()
    {
        borderImage.enabled = true;
    }
    public void DeselectItem()
    {
        borderImage.enabled = false;
    }
    public void DeleteObject()
    {
        Destroy(gameObject);
    }

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
    
}