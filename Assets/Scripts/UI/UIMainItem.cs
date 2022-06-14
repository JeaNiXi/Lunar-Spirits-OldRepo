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

    public event Action<UIMainItem>
        OnItemRMBClicked;

    private bool IsEmpty { get; set; }

    public void SetData()
    {
        this.itemImage.sprite = null;
        this.quantityText.text = "";
        this.IsEmpty = true;
        imageComponent.gameObject.SetActive(false);
    }
    public void SetData(Sprite sprite, int quantity)
    {
        imageComponent.gameObject.SetActive(true);
        this.itemImage.sprite = sprite;
        this.quantityText.text = quantity.ToString();
        this.IsEmpty = false;
    }

    public void DeleteObject()
    {
        Destroy(gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("LMB Clicked");
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnItemRMBClicked?.Invoke(this);
        }
    }
}