using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItem : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image borderImage;

    public event Action<UIItem> OnDescriptionRequested, OnDescriptionClosed;

    private void Awake()
    {
        Deselect();
    }
    private void Deselect()
    {
        borderImage.enabled = false;
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
}
