using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMainItem : MonoBehaviour
{
    [SerializeField] private Component imageComponent;
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text quantityText;

    private bool IsEmpty { get; set; }

    public void SetData()
    {
        this.itemImage = null;
        this.quantityText.text = "";
        this.IsEmpty = true;
    }
    public void SetData(Sprite sprite, int quantity)
    {
        imageComponent.gameObject.SetActive(true);
        this.itemImage.sprite = sprite;
        this.quantityText.text = quantity.ToString();
        this.IsEmpty = false;
    }
}