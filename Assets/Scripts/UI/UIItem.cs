using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItem : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image borderImage;

    public event Action<UIItem>
        OnDescriptionRequested,
        OnDescriptionClosed,
        OnItemClicked,
        OnItemRMBClicked,
        OnBeginDragging,
        OnEndDragging,
        OnItemDropped;

    private void Awake()
    {
        Deselect();
    }
    public void Deselect()
    {
        borderImage.enabled = false;
    }
    public void Select()
    {
        borderImage.enabled = true;
    }
    public void SetData(Sprite sprite, int quantity)
    {
        this.itemImage.sprite = sprite;
        this.quantityText.text = quantity + "";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnDescriptionRequested?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnDescriptionClosed?.Invoke(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            OnItemClicked?.Invoke(this);
        else if (eventData.button == PointerEventData.InputButton.Right)
            OnItemRMBClicked?.Invoke(this);
        else
            return;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnBeginDragging?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("ended drag");
        OnEndDragging?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnDrop(PointerEventData eventData)
    {
        OnItemDropped?.Invoke(this);
    }
}
